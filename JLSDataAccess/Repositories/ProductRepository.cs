﻿using JLSDataAccess.Interfaces;
using JLSDataModel.Models.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JLSDataModel.ViewModels;
using System.Security.AccessControl;
using JLSDataModel.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace JLSDataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly JlsDbContext context;
        private readonly IReferenceRepository _referencRepository;
        private readonly string _defaultLang;

        public ProductRepository(JlsDbContext context, IReferenceRepository referencRepository, IConfiguration config)
        {
            this.context = context;
            _referencRepository = referencRepository;
            _defaultLang = config.GetValue<string>("Lang:DefaultLang");
        }

        public async Task<List<ReferenceItemViewModel>> GetProductCategory(String lang)
        {
            var result = await _referencRepository.GetReferenceItemsByCategoryLabels("MainCategory;SecondCategory", lang);
            return result;
        }

        public async Task<List<ReferenceItemViewModel>> GetTaxRate()
        {
            var result = await _referencRepository.GetReferenceItemsByCategoryLabels("TaxRate", null);
            return result;
        }

        public async Task<int> SaveProduct(Product product, List<IFormFile> images, List<ReferenceLabel> labels)
        {
            string imagesPath = "images/" + product.ReferenceItem.Code + "/";

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            if (product.Id == 0)
            {
                context.ReferenceItem.Add(product.ReferenceItem);
                await context.SaveChangesAsync();
                product.ReferenceItemId = product.ReferenceItem.Id;
                context.Product.Add(product);
                await context.SaveChangesAsync();

                labels = _referencRepository.CheckLabels(labels, product.ReferenceItemId);
                foreach (ReferenceLabel label in labels)
                {
                    context.ReferenceLabel.Add(label);
                }
            }
            else
            {
                context.Product.Update(product);

                labels = _referencRepository.CheckLabels(labels, product.ReferenceItemId);
                foreach (ReferenceLabel label in labels)
                {
                    context.ReferenceLabel.Update(label);
                }

            }

            foreach (IFormFile image in images)
            {
                if (await SaveImage(image, imagesPath))
                {
                    context.ProductPhotoPath.Add(
                        new ProductPhotoPath
                        {
                            Path = product.ReferenceItem.Code + "/" + image.FileName,
                            ProductId = product.Id
                        }
                    );
                }
            }
            await context.SaveChangesAsync();
            return 1;
        }

        private async Task<Boolean> SaveImage(IFormFile image, string path)
        {
            try
            {
                var imagePath = Path.Combine(path, image.FileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (FileStream fs = File.Create(imagePath))
                {
                    // 复制文件
                    await image.CopyToAsync(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                return false;
            }


            return true;
        }

        public async Task<List<ProductsListViewModel>> GetAllProduct(string lang, int intervalCount, int size, string orderActive, string orderDirection)
        {
            var request = (from ri in context.ReferenceItem
                          join rc in context.ReferenceCategory on ri.ReferenceCategoryId equals rc.Id
                          from rl in context.ReferenceLabel.Where(p => p.ReferenceItemId == ri.Id && p.Lang == lang).DefaultIfEmpty()
                          where rc.ShortLabel.Equals("Product")
                          join p in context.Product on ri.Id equals p.ReferenceItemId
                          from img in context.ProductPhotoPath.Where(img => p.Id == img.ProductId).Take(1).DefaultIfEmpty()
                          select new ProductsListViewModel
                          {
                              Id = p.Id,
                              Name = rl.Label,
                              Category = (from rlp in context.ReferenceLabel
                                          where rlp.ReferenceItemId == ri.ParentId
                                          select rlp.Label).FirstOrDefault(),
                              Image = img.Path,
                              Price = p.Price,
                              ReferenceCode = ri.Code,
                              Validity = ri.Validity,
                          });

            if (orderActive == "null" || orderActive == "undefined" || orderDirection == "null")
            {
                return await request.Skip(intervalCount * size).Take(size).ToListAsync();
            }

            Expression<Func<ProductsListViewModel, object>> funcOrder;

            switch (orderActive)
            {
                case "reference":
                    funcOrder = p => p.ReferenceCode;
                    break;
                case "name":
                    funcOrder = p => p.Name;
                    break;
                case "categories":
                    funcOrder = p => p.Category;
                    break;
                case "price":
                    funcOrder = p => p.Price;
                    break;
                case "active":
                    funcOrder = p => p.Validity;
                    break;
                default:
                    funcOrder = p => p.Id;
                    break;
            }

            if (orderDirection == "asc")
            {
                request = request.OrderBy(funcOrder);
            }
            else
            {
                request = request.OrderByDescending(funcOrder);
            }

            var result = await request.Skip(intervalCount * size).Take(size).ToListAsync();


            return result;
        }

        public async Task<ProductViewModel> GetProductById(long id)
        {
            var result = await (from ri in context.ReferenceItem
                          from rl in context.ReferenceLabel.Where(p => p.ReferenceItemId == ri.Id).DefaultIfEmpty()
                          join p in context.Product on ri.Id equals p.ReferenceItemId
                          where p.Id == id
                          select new ProductViewModel
                          {
                              Id = p.Id,
                              Category = ri.ParentId,
                              ReferenceCode = ri.Code,
                              Color = p.Color,
                              Description = p.Description,
                              Material = p.Material,
                              Size = p.Size,
                              MinQuantity = p.MinQuantity,
                              Price = p.Price,
                              QuantityPerBox = p.QuantityPerBox,
                              ReferenceItemId = ri.Id,
                          }).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }
            result.Label = await (from rl in context.ReferenceLabel
                           where rl.ReferenceItemId == result.ReferenceItemId
                           select rl).ToListAsync();

            result.Images = await (from img in context.ProductPhotoPath
                                   where img.ProductId == result.Id
                                   select img).ToListAsync();
            return result;
        }

        public async Task<int> RemoveImageById(long id)
        {
            ProductPhotoPath image = await context.ProductPhotoPath.FindAsync(id);

            if(image == null)
            {
                return 0;
            }

            string imagePath = "images/" + image.Path;

            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                context.ProductPhotoPath.Remove(image);

                await context.SaveChangesAsync();
            }catch(Exception e)
            {
                return 0;
            }
            return 1;
        }

        public async Task<List<ProductsListViewModel>> SearchProducts(string lang, string filter)
        {
            var predicate = PredicateBuilder.New<ProductsListViewModel>();
            predicate.Or(p => p.ReferenceCode.Contains(filter));
            predicate.Or(p => p.Name.Contains(filter));
            predicate.Or(p => p.Category.Contains(filter));

            var result = await (from ri in context.ReferenceItem
                          where ri.Code.Contains(filter)
                          join rc in context.ReferenceCategory on ri.ReferenceCategoryId equals rc.Id
                          from rl in context.ReferenceLabel.Where(p => p.ReferenceItemId == ri.Id && p.Lang == lang).DefaultIfEmpty()
                          where rc.ShortLabel.Equals("Product")
                          join p in context.Product on ri.Id equals p.ReferenceItemId
                          from img in context.ProductPhotoPath.Where(img => p.Id == img.ProductId).Take(1).DefaultIfEmpty()
                          select new ProductsListViewModel
                          {
                              Id = p.Id,
                              Name = rl.Label,
                              Category = (from rlp in context.ReferenceLabel
                                          where rlp.ReferenceItemId == ri.ParentId
                                          select rlp.Label).FirstOrDefault(),
                              Image = img.Path,
                              Price = p.Price,
                              ReferenceCode = ri.Code,
                              Validity = ri.Validity,
                          }).Where(predicate).Take(10).ToListAsync();
            return result;
        }
    }
}

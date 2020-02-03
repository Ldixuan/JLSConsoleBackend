using JLSDataAccess.Interfaces;
using JLSDataModel.Models.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JLSDataModel.ViewModels;

namespace JLSDataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly JlsDbContext context;
        private readonly IReferenceRepository _referencRepository;

        public ProductRepository(JlsDbContext context, IReferenceRepository referencRepository)
        {
            this.context = context;
            _referencRepository = referencRepository;
        }

        public class CategoryResult
        {
            public long Id { get; set; }

            public String Label { get; set; }

            public List<CategoryResult> sousCategory { get; set; }
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

        public async Task<int> saveProduct(Product product, List<IFormFile> images)
        {
            String imagesPath = "images/" + product.ReferenceItem.Code;
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            foreach (IFormFile i in images)
            {
                if (!saveImage(i, imagesPath)) return 0;
            }

            if(product.Id == 0)
            {
                context.Product.Add(product);
            }
            else
            {
                context.Product.Update(product);
            }
            await context.SaveChangesAsync();
            return 1;
        }

        private Boolean saveImage(IFormFile image, String path)
        {
            String imageName = path + image.Name;
           
            if (System.IO.File.Exists(imageName))
            {
                System.IO.File.Delete(imageName);
            }

            using (FileStream fs = File.Create(path))
            {
                // 复制文件
                image.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }
            return true;
        }
    }
}

using JLSDataAccess.Interfaces;
using JLSDataModel.Models.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JLSDataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly JlsDbContext context;
        public ProductRepository(JlsDbContext context)
        {
            this.context = context;
        }

        public async Task<int> saveProduct(Product product, IFormFile image)
        {
            if (!saveImage(image))
            {
                return 0;
            }
            return 1;
        }

        private Boolean saveImage(IFormFile image)
        {
            String path = "Resoures/images/" + image.FileName;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
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

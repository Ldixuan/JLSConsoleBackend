using JLSDataModel.Models.Product;
using JLSDataModel.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JLSDataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<int> saveProduct(Product product, List<IFormFile> image);

        Task<List<ReferenceItemViewModel>> GetProductCategory(String lang);

        Task<List<ReferenceItemViewModel>> GetTaxRate();
    }
}

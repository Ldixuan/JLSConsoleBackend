using JLSDataModel.Models;
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
        Task<int> saveProduct(Product product, List<IFormFile> image, List<ReferenceLabel> labels);

        Task<List<ReferenceItemViewModel>> GetProductCategory(string lang);

        Task<List<ReferenceItemViewModel>> GetTaxRate();

        Task<List<ProductsListViewModel>> GetAllProduct(string lang);

        Task<ProductViewModel> GetProductById(long id);
    }
}

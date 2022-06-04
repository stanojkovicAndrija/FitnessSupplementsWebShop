using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IProductRepository
    {
        List<ProductEntity> GetProduct();
        int GetProductCount();
        List<ProductEntity> GetProductByPage(int page, int pageResults, int? manufacturerID, int? categoryID,string sort,string search); 
        ProductEntity GetProductByID(int productID);
        //List<ProductEntity> GetProductsByCategory(string category);
        //List<ProductEntity> GetProductsByManufacturer(string manufacturer);
        //List<ProductEntity> GetProductsByCategoryAndManufacturer(string category, string manufacturer);
        ProductEntity CreateProduct(ProductEntity product);

        void UpdateProduct(ProductEntity product);
        int GetNumberOfProducts();

        void DeleteProduct(int productID);

        bool SaveChanges();

    }
}

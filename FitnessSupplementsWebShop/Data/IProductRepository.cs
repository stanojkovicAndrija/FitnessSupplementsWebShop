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

        ProductEntity GetProductByID(int productID);
        //List<ProductEntity> GetProductsByCategory(string category);
        //List<ProductEntity> GetProductsByManufacturer(string manufacturer);
        //List<ProductEntity> GetProductsByCategoryAndManufacturer(string category, string manufacturer);
        ProductEntity CreateProduct(ProductEntity product);

        void UpdateProduct(ProductEntity product);

        void DeleteProduct(int productID);

        bool SaveChanges();

    }
}

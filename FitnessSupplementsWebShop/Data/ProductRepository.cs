using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public ProductRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public ProductEntity CreateProduct(ProductEntity product)
        {
            context.Product.Add(product);
            return product;
        }

        public void DeleteProduct(int productID)
        {
            context.Product.Remove(context.Product.FirstOrDefault(r => r.ProductID == productID));
        }

        public ProductEntity GetProductByID(int productID)
        {
            return context.Product.FirstOrDefault(r => r.ProductID == productID);
        }
        //public List<ProductEntity> GetProductsByCategory(string category)
        //{
        //    List<ProductEntity> result = new List<ProductEntity>();
        //    foreach(var r in context.Product)
        //    {
        //        if(category==r.Category.Name)
        //        {
        //            result.Add(r);
        //        }
        //    }
        //    return result;
        //}

        //public List<ProductEntity> GetProductsByManufacturer(string manufacturer)
        //{
        //    List<ProductEntity> result = new List<ProductEntity>();
        //    foreach (var r in context.Product)
        //    {
        //        if (manufacturer == r.Manufacturer.Name)
        //        {
        //            result.Add(r);
        //        }
        //    }
        //    return result;
        //}
        //public List<ProductEntity> GetProductsByCategoryAndManufacturer(string category,string manufacturer)
        //{
        //    List<ProductEntity> result = new List<ProductEntity>();
        //    foreach (var r in context.Product)
        //    {
        //        if (manufacturer == r.Manufacturer.Name && category == r.Category.Name)
        //        {
        //            result.Add(r);
        //        }
        //    }
        //    return result;
        //}
        public List<ProductEntity> GetProduct()
        {
            return (from r in context.Product select r).ToList();
        }

        public void UpdateProduct(ProductEntity product)
        {
            throw new NotImplementedException();
        }
    }
}
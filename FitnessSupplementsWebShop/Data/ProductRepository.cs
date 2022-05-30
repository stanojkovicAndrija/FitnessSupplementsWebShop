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
        private readonly ICategoryRepository categoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public ProductRepository(ICategoryRepository categoryRepository, FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.context = context;
            this.mapper = mapper;
        }
        public List<ProductEntity> GetProductByPage(int page, int pageResults, string manufacturer, string category)
        {

            List<ProductEntity> response = (from u in context.Product select u)
                .ToList();
            if (category != null)
            {
                response = GetProductsByFilter(manufacturer,category);   
            }
            return response.Skip((page - 1) * pageResults)
                .Take(pageResults).ToList();             
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
        public int GetProductCount()
        {
            return context.Product.Count();
        }
        public void DeleteProduct(int productID)
        {
            context.Product.Remove(context.Product.FirstOrDefault(r => r.ProductID == productID));
        }

        public ProductEntity GetProductByID(int productID)
        {
            return context.Product.FirstOrDefault(r => r.ProductID == productID);
        }
        public List<ProductEntity> GetProductsByFilter(string manufacturer, string category)
        {
            List<ProductEntity> products = new List<ProductEntity>();
            //if (manufacturer !=null)
            //{
            //    int manufacturerID=manufacturerRepository.GetManufacturerIdByName(manufacturer);
            //    foreach(var r in context.Product)
            //    {
            //        if(r.ManufacturerID==manufacturerID)
            //        {
            //            products.Add(r);
            //        }
            //    }
            //}
            if (category != null)
            {

                int categoryID = categoryRepository.GetCategoryByName(category).CategoryID;
                foreach (var r in context.Product)
                {
                    if (r.CategoryID == categoryID)
                    {
                        products.Add(r);
                    }
                }
                return products;
            }
            return null;

        }
        public List< ProductEntity> GetProduct()
        {
            return (from r in context.Product select r).ToList();
        }

        public void UpdateProduct(ProductEntity product)
        {
            throw new NotImplementedException();
        }
    }
}
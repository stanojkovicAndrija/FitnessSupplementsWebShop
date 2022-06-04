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
        public int NumberOfProducts { get; set; }
        public ProductRepository(ICategoryRepository categoryRepository, FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.context = context;
            this.mapper = mapper;
        }
        public List<ProductEntity> GetProductByPage(int page, int pageResults, int? manufacturerID, int? categoryID,string sort,string search)
        {
            
            List<ProductEntity> response = (from u in context.Product select u)
                .ToList();
            var checkCat = categoryID.ToString();
            var checkMan = manufacturerID.ToString();
            if (checkCat!="" && checkMan!="")
            {
                response = GetProductsByManufacturerAndCategory(manufacturerID, categoryID);
            }
            else if(checkMan!="")
            {
                response = GetProductsByManufacturer(manufacturerID);
            }
            else if (checkCat != "")
            {
                response = GetProductsByCategory(categoryID);
            }
            if (search!= null)
            {
                response = GetProductsBySearch(response, search);
            }
            if (sort!=null)
            {
                response = SortedProductsList(response,sort);
            }

            NumberOfProducts = response.Count;
            return response.Skip((page - 1) * pageResults)
                .Take(pageResults)
                .ToList();             
        }

        private List<ProductEntity> GetProductsBySearch(List<ProductEntity> products,string search)
        {
            List<ProductEntity> searchedProducts = new();
            foreach(var v in products)
            {
                if(v.Name.ToLower().Contains(search.ToLower()))
                {
                    searchedProducts.Add(v);
                }
            }
            return searchedProducts;
        }

        public int GetNumberOfProducts()
        {
            return NumberOfProducts;
        }
        private List<ProductEntity> GetProductsByManufacturerAndCategory(int? manufacturerID, int? categoryID)
        {
            List<ProductEntity> temp=new List<ProductEntity>();
            foreach(var v in context.Product)
            {
                if(v.CategoryID==categoryID && manufacturerID == v.ManufacturerID)
                {
                    temp.Add(v);
                }
            }
            return temp;
        }

        private List<ProductEntity> GetProductsByManufacturer(int? manufacturerID)
        {
            List<ProductEntity> temp = new List<ProductEntity>();
            foreach (var v in context.Product)
            {
                if (manufacturerID == v.ManufacturerID)
                {
                    temp.Add(v);
                }
            }
            return temp;
        }
        private List<ProductEntity> GetProductsByCategory(int? categoryID)
        {
            List<ProductEntity> temp = new List<ProductEntity>();
            foreach (var v in context.Product)
            {
                if (categoryID == v.CategoryID)
                {
                    temp.Add(v);
                }
            }
            return temp;
        }
        private List<ProductEntity> SortedProductsList(List<ProductEntity>unsortedProducts,string sort)
        {
            switch (sort)
            {
                case "priceAsc":
                    var sorted = unsortedProducts.OrderBy(p => p.Price).ToList();
                    return sorted;
                case "priceDesc":
                     sorted = unsortedProducts.OrderByDescending(p => p.Price).ToList();
                    return sorted;
                case "nameDesc":
                    sorted = unsortedProducts.OrderByDescending(p => p.Name).ToList();
                    return sorted;
                default:
                    sorted = unsortedProducts.OrderBy(p => p.Name).ToList();
                    return sorted;
            }

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
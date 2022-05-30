using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public CategoryRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public CategoryEntity GetCategoryByName(string name)
        {
            return context.Category.FirstOrDefault(r => r.Name == name);
        }
        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public CategoryEntity CreateCategory(CategoryEntity category)
        {
            context.Category.Add(category);
            return category;
        }

        public void DeleteCategory(int categoryID)
        {
            context.Category.Remove(context.Category.FirstOrDefault(r => r.CategoryID == categoryID));
        }

        public CategoryEntity GetCategoryByID(int categoryID)
        {
            return context.Category.FirstOrDefault(r => r.CategoryID == categoryID);
        }

        public List<CategoryEntity> GetCategory()
        {
            return (from r in context.Category select r).ToList();
        }

        public void UpdateCategory(CategoryEntity category)
        {
            throw new NotImplementedException();
        }
    }
}
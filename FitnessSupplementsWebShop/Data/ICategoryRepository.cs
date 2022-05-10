using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface ICategoryRepository
    {
        List<CategoryEntity> GetCategory();

        CategoryEntity GetCategoryByID(int categoryID);

        CategoryEntity CreateCategory(CategoryEntity category);

        void UpdateCategory(CategoryEntity category);

        void DeleteCategory(int categoryID);

        bool SaveChanges();

    }
}

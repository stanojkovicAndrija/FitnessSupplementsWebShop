using FitnessSupplementsWebShop.Auth;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IUsersRepository
    {
        List<UsersEntity> GetUsers();

        UsersEntity GetUserByID(int userID);

        UsersEntity CreateUser(UsersEntity user);

        void UpdateUser(UsersEntity user);

        void DeleteUser(int userID);

        bool SaveChanges();

        string HashPassword(string password);
        UsersEntity GetUserByEmail(LoginDto login);

    }
}

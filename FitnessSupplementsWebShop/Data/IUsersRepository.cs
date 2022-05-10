using FitnessSupplementsWebShop.Auth;
using FitnessSupplementsWebShop.Entities;
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

        UsersEntity GetUserByEmail(LoginDto login);

    }
}

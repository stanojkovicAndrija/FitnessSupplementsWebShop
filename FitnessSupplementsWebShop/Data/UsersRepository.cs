using AutoMapper;
using FitnessSupplementsWebShop.Auth;
using FitnessSupplementsWebShop.Entities;
using FitnessSupplementsWebShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public UsersRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public UsersEntity CreateUser(UsersEntity user)
        {
            string hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;
            context.Users.Add(user);
            return user;
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);           
        }
        public void DeleteUser(int userID)
        {
            context.Users.Remove(context.Users.FirstOrDefault(u => u.UserID == userID));
        }

        public UsersEntity GetUserByID(int userID)
        {
            return context.Users.FirstOrDefault(u => u.UserID == userID);
        }

        public UsersEntity GetUserByEmail(LoginDto login)
        {
            return context.Users.FirstOrDefault(u => u.Email == login.Email);
        }
        public List<UsersEntity> GetUsers()
        {
            return (from u in context.Users select u)
                .ToList();
        }
        public void UpdateUser(UsersEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
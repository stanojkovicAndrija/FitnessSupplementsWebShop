using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IOrdersRepository
    {
        List<OrdersEntity> GetOrder();

        OrdersEntity GetOrderByID(int orderID);

        OrdersEntity CreateOrder(OrdersEntity order);

        void UpdateOrder(OrdersEntity order);

        void DeleteOrder(int orderID);

        bool SaveChanges();

    }
}

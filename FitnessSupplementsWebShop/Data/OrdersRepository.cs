using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public OrdersRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public OrdersEntity CreateOrder(OrdersEntity order)
        {
            context.Orders.Add(order);
            return order;
        }

        public void DeleteOrder(int orderID)
        {
            context.Orders.Remove(context.Orders.FirstOrDefault(r => r.OrderID == orderID));
        }

        public OrdersEntity GetOrderByID(int orderID)
        {
            return context.Orders.FirstOrDefault(r => r.OrderID == orderID);
        }

        public List<OrdersEntity> GetOrder()
        {
            return (from r in context.Orders select r).ToList();
        }

        public void UpdateOrder(OrdersEntity order)
        {
            throw new NotImplementedException();
        }
    }
}
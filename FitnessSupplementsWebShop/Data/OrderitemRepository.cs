using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class OrderitemRepository : IOrderitemRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IProductRepository productRepository;

        public OrderitemRepository(FitnessSupplementsWebShopContext context, IProductRepository productRepository)
        {
            this.context = context;
            this.productRepository = productRepository;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public OrderitemEntity CreateOrderItem(OrderitemEntity orderItem)
        {
            context.Orderitem.Add(orderItem);
            return orderItem;
        }

        public void DeleteOrderItem(int orderItemID)
        {
            context.Orderitem.Remove(context.Orderitem.FirstOrDefault(r => r.OrderItemID == orderItemID));
        }

        public OrderitemEntity GetOrderItemByID(int orderItemID)
        {
            return context.Orderitem.FirstOrDefault(r => r.OrderItemID == orderItemID);
        }

        public List<OrderitemEntity> GetOrderItem()
        {
            return (from r in context.Orderitem select r).ToList();
        }

        public List<ProductEntity> GetProductsByOrderID(int orderID)
        {
            List<ProductEntity> lista = new();
            foreach(var v in context.Orderitem)
            {
                if(v.OrderID==orderID)
                {
                    lista.Add(productRepository.GetProductByID(v.ProductID));
                }                
            }
            return lista;
        }
        public void UpdateOrderItem(OrderitemEntity orderItem)
        {
            throw new NotImplementedException();
        }
    }
}
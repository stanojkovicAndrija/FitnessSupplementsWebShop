using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FitnessSupplementsWebShop.Data
{
    public interface IOrderitemRepository
    {
        List<OrderitemEntity> GetOrderItem();

        OrderitemEntity GetOrderItemByID(int orderItemID);

        OrderitemEntity CreateOrderItem(OrderitemEntity orderItem);

        void UpdateOrderItem(OrderitemEntity orderItem);

        void DeleteOrderItem(int orderItem);

        bool SaveChanges();

    }
}

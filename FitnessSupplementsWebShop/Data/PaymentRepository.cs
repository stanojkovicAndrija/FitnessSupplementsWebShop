using AutoMapper;
using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly FitnessSupplementsWebShopContext context;
        private readonly IMapper mapper;

        public PaymentRepository(FitnessSupplementsWebShopContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public PaymentEntity CreatePayment(PaymentEntity payment)
        {
            context.Payment.Add(payment);
            return payment;
        }

        public void DeletePayment(int paymentID)
        {
            context.Payment.Remove(context.Payment.FirstOrDefault(r => r.PaymentID == paymentID));
        }

        public PaymentEntity GetPaymentByID(int paymentID)
        {
            return context.Payment.FirstOrDefault(r => r.PaymentID == paymentID);
        }

        public List<PaymentEntity> GetPayment()
        {
            return (from r in context.Payment select r).ToList();
        }

        public void UpdatePayment(PaymentEntity payment)
        {
            throw new NotImplementedException();
        }
    }
}
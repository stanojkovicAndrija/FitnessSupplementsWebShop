using FitnessSupplementsWebShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Data
{
    public interface IPaymentRepository
    {
        List<PaymentEntity> GetPayment();

        PaymentEntity GetPaymentByID(int paymentID);

        PaymentEntity CreatePayment(PaymentEntity payment);

        void UpdatePayment(PaymentEntity payment);

        void DeletePayment(int paymentID);

        bool SaveChanges();

    }
}

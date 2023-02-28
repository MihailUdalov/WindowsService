using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class PaymentReport
    {
        public List<PaymentTransactions> Transactions { get; set; }

        public PaymentReport()
        {
            Transactions = new List<PaymentTransactions>();
        }
       
        public void Add(DataContext dataContext)
        {
            Payer payer = new Payer(dataContext.Name, dataContext.Payment, dataContext.Date, dataContext.AccountNumber);

            Service service = new Service(dataContext.ServiceName, new List<Payer>(), 0);
            service.AddPayer(payer);

            PaymentTransactions paymentTransactions = new PaymentTransactions(dataContext.City, new List<Service>(), 0);
            paymentTransactions.AddService(service);

            if (Transactions.Any(s => s.City == paymentTransactions.City))
                Transactions.First(s => s.City == paymentTransactions.City).AddService(paymentTransactions.Services.First());

            else
               Transactions.Add(paymentTransactions);
        }

    }
}

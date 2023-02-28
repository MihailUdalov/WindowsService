using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class PaymentTransactions
    {
        public string City { get; set; }
        public List<Service> Services { get; set; }
        public decimal Total { get; set; }

        public PaymentTransactions(string city, List<Service> services, decimal total)
        {
            City = city;
            Services = services;
            Total = total;
        }

        public void AddService(Service service)
        {
            if (Services.Any(s => s.Name == service.Name))
            {
                Services.First(s => s.Name == service.Name).AddPayer(service.Payers.First());
                Total = Services.Sum(s => s.Total);
                return;
            }
            Services.Add(service);
            Total = Services.Sum(s => s.Total);
        }
        
    }
}

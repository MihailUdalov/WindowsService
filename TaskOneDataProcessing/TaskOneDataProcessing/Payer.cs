using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class Payer
    {
        public string Name { get; set; }
        public decimal Payment { get; set; } 
        public DateTime Date { get; set; }        
        public long AccountNumber { get; set; }

        public Payer(string name, decimal payment, DateTime date, long accountNumber) 
        {
            Name = name;
            Payment = payment;                
            Date = date;
            AccountNumber = accountNumber;   
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class Service
    {
        public string Name { get; set; }
        public List<Payer> Payers { get; set; }
        public decimal Total { get; set; }

        public Service(string name, List<Payer> payers, decimal total)
        {
            Name= name;
            Payers= payers;
            Total= total;
        }

        public void AddPayer(Payer payer)
        {
            Payers.Add(payer);
            Total = Payers.Sum(p => p.Payment);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class DataContext
    {
        public string Name { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string ServiceName { get; set; }
        public string City { get; set; }

        public DataContext()
        {

        }

        public void AttachingData(List<string> data)
        {
            City = (data[0]).Replace("\"", "");
            Name = (data[1] + " " + data[2]).Replace("\"","");
            Payment = decimal.Parse(data[3]);
            AccountNumber = long.Parse(data[5]);
            Date = DateTime.ParseExact(data[4], "yyyy-dd-MM",
                               CultureInfo.InvariantCulture, DateTimeStyles.None);
            ServiceName = (data[6]).Replace("\"", "");
        }

    }
}

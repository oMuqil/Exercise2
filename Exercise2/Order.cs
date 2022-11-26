using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    internal class Order
    {
        [Name("OrdId")]
        public string OrdId { get; set; }
        [Name("OrdArea")]
        public string OrdArea { get; set; }
        [Name("PrdName")]
        public string PrdName { get; set; }
        [Name("PrdQuantity")]
        public int PrdQuantity { get; set; }
        [Name("PrdBrand")]
        public string PrdBrand { get; set; }
    }
}

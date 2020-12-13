using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class CurrencyRateDto
    {
        public int CurrencyRateId { get; set; }
        public DateTime CurrencyDate { get; set; }
        public decimal Rate { get; set; }
    }
}

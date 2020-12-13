using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class CountryWithoutCurrencyRateDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
    }
}

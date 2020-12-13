using CurrencyInfoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public int numberOfCurrencyRates
        {
            get
            {
                return CurrencyRate.Count;
            }
        }

        public ICollection<CurrencyRateDto> CurrencyRate { get; set; } = new List<CurrencyRateDto>();
    }
}

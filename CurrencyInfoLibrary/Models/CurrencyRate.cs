using System;
using System.Collections.Generic;

namespace CurrencyInfoLibrary.Models
{
    public partial class CurrencyRate
    {
        public int CurrencyRateId { get; set; }
        public DateTime CurrencyDate { get; set; }
        public decimal Rate { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CurrencyInfoLibrary.Models
{
    public partial class Country
    {
        public Country()
        {
            CurrencyRate = new HashSet<CurrencyRate>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }

        public virtual ICollection<CurrencyRate> CurrencyRate { get; set; }
    }
}

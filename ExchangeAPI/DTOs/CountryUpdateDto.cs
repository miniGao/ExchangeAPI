using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class CountryUpdateDto
    {
        [Required(ErrorMessage = "You should provide a country name.")]
        [MaxLength(50)]
        public string CountryName { get; set; }
        [Required(ErrorMessage = "You should provide a currency code.")]
        [MaxLength(5)]
        public string CurrencyCode { get; set; }
    }
}

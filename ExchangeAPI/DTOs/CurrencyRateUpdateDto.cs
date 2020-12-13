using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.DTOs
{
    public class CurrencyRateUpdateDto
    {
        [Required(ErrorMessage = "You should provide a date for exchange rate.")]
        public DateTime CurrencyDate { get; set; }
        [Required(ErrorMessage = "You should provide a exchange rate value.")]
        public decimal Rate { get; set; }
    }
}

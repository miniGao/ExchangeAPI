using AutoMapper;
using CurrencyInfoLibrary.Models;
using ExchangeAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryDto>();
            CreateMap<Country, CountryWithoutCurrencyRateDto>();
            CreateMap<CountryCreateDto, Country>();
            CreateMap<CountryUpdateDto, Country>();
            
            CreateMap<CurrencyRate, CurrencyRateDto>();
            CreateMap<CurrencyRateCreateDto, CurrencyRate>();
            CreateMap<CurrencyRateUpdateDto, CurrencyRate>();
        }
    }
}

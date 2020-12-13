using CurrencyInfoLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.Services
{
    public interface ICurrencyInfoRepository
    {
        Task<bool> IsCountryExists(int countryId);
        Task<IEnumerable<Country>> GetAllCountries();
        Task<Country> GetCountryById(int countryId, bool isCurrencyRateIncluded);
        Task<IEnumerable<CurrencyRate>> GetAllCurrencyRatesByCountry(int countryId);
        Task<CurrencyRate> GetSpecificCurrencyRateByCountry(int countryId, int currencyRateId);
        Task<CurrencyRate> GetLastestCurrencyRateByCountry(int countryId);
        Task<CurrencyRate> GetCurrencyRateByDate(int countryId, DateTime date);
        Task<IEnumerable<CurrencyRate>> GetCurrencyRateBetweenDate(int countryId, DateTime startDate, DateTime endDate);
        void AddNewCountry(Country country);
        void DeleteCountry(Country country);
        Task AddNewCurrencyRate(int countryId, CurrencyRate currencyRate);
        void DeleteCurrencyRate(CurrencyRate currencyRate);
        Task<bool> Save();
    }
}

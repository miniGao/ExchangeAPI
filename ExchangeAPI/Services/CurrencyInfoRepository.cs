using CurrencyInfoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.Services
{
    public class CurrencyInfoRepository : ICurrencyInfoRepository
    {
        private CurrencyInfoDBContext context;

        public CurrencyInfoRepository(CurrencyInfoDBContext _context)
        {
            context = _context;
        }

        public void AddNewCountry(Country country)
        {
            context.Add(country);
        }

        public async Task AddNewCurrencyRate(int countryId, CurrencyRate currencyRate)
        {
            var country = await GetCountryById(countryId, false);
            country.CurrencyRate.Add(currencyRate);
        }

        public void DeleteCountry(Country country)
        {
            var countryToDelete = context.Country.Include(contxt => contxt.CurrencyRate).FirstOrDefault(c => c.CountryId == country.CountryId);
            context.Country.Remove(countryToDelete);
        }

        public void DeleteCurrencyRate(CurrencyRate currencyRate)
        {
            context.CurrencyRate.Remove(currencyRate);
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var result = context.Country.OrderBy(c => c.CountryName);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<CurrencyRate>> GetAllCurrencyRatesByCountry(int countryId)
        {
            IQueryable<CurrencyRate> result = context.CurrencyRate.Where(r => r.CountryId == countryId);
            return await result.ToListAsync();
        }

        public async Task<Country> GetCountryById(int countryId, bool isCurrencyRateIncluded)
        {
            IQueryable<Country> result;
            if (isCurrencyRateIncluded)
            {
                result = context.Country.Include(c => c.CurrencyRate).Where(c => c.CountryId == countryId);
            }
            else
            {
                result = context.Country.Where(c => c.CountryId == countryId);
            }
            return await result.FirstOrDefaultAsync();
        }

        public async Task<CurrencyRate> GetCurrencyRateByDate(int countryId, DateTime date)
        {
            IQueryable<CurrencyRate> result = context.CurrencyRate.Where(r => r.CountryId == countryId && r.CurrencyDate == date);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CurrencyRate>> GetCurrencyRateBetweenDate(int countryId, DateTime startDate, DateTime endDate)
        {
            IQueryable<CurrencyRate> result = context.CurrencyRate.Where(r => r.CountryId == countryId && r.CurrencyDate >= startDate && r.CurrencyDate <= endDate);
            return await result.ToListAsync();
        }

        public async Task<CurrencyRate> GetLastestCurrencyRateByCountry(int countryId)
        {
            IQueryable<CurrencyRate> result = context.CurrencyRate.Where(r => r.CountryId == countryId).OrderByDescending(r => r.CurrencyDate);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<CurrencyRate> GetSpecificCurrencyRateByCountry(int countryId, int currencyRateId)
        {
            IQueryable<CurrencyRate> result = context.CurrencyRate.Where(r => r.CountryId == countryId && r.CurrencyRateId == currencyRateId);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> IsCountryExists(int countryId)
        {
            return await context.Country.AnyAsync<Country>(c => c.CountryId == countryId);
        }

        public async Task<bool> Save()
        {
            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

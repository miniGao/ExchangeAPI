using AutoMapper;
using CurrencyInfoLibrary.Models;
using ExchangeAPI.DTOs;
using ExchangeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeAPI.Controllers
{
    [Route("currencyapi")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private ICurrencyInfoRepository currencyInfoRepo;
        private readonly IMapper mapper;

        public CountryController(ICurrencyInfoRepository _currencyInfoRepo, IMapper _mapper)
        {
            currencyInfoRepo = _currencyInfoRepo;
            mapper = _mapper;
        }

        // GET: currencyapi/countrylist
        [HttpGet]
        //[Route("countrylist")]
        [Route("countrylist.{format}"), FormatFilter]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountriesAsync()
        {
            var countries = await currencyInfoRepo.GetAllCountries();
            var result = mapper.Map<IEnumerable<CountryWithoutCurrencyRateDto>>(countries);
            return Ok(result);
        }

        // GET currencyapi/countryinfo/{id}(?isCurrencyRateIncluded=true)
        [HttpGet("countryinfo/{id}")]
        public async Task<ActionResult<Country>> GetCountryById(int id, bool isCurrencyRateIncluded = false)
        {
            var country = await currencyInfoRepo.GetCountryById(id, isCurrencyRateIncluded);
            if (country == null)
            {
                return NotFound();
            }
            if (isCurrencyRateIncluded)
            {
                var result = mapper.Map<CountryDto>(country);
                return Ok(result);
            }
            var countryOnly = mapper.Map<CountryWithoutCurrencyRateDto>(country);
            return Ok(countryOnly);
        }

        // POST currencyapi/addcountry
        [HttpPost("addcountry")]
        public async Task<ActionResult<Country>> AddNewCountry([FromBody] CountryCreateDto newCountry)
        {
            if (newCountry == null)
            {
                return BadRequest();
            }
            if (!Regex.IsMatch(newCountry.CountryName, @"^[a-zA-Z ]+$"))
            {
                ModelState.AddModelError("Country Name Invalid", "The provided country name should contains letters and spaces only (e.g. New Zealand)");
            }
            if (!Regex.IsMatch(newCountry.CurrencyCode, @"^[a-zA-Z]+$"))
            {
                ModelState.AddModelError("Currency Code Invalid", "The provided currency code should be only in format of 3 letters (e.g. CAD)");
            }
            if (newCountry.CurrencyCode.Length != 3)
            {
                ModelState.AddModelError("Currency Code Invalid", "The provided currency code should be only in format of 3 letters (e.g. CAD)");
            }
            newCountry.CurrencyCode = newCountry.CurrencyCode.ToUpper();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryToAdd = mapper.Map<Country>(newCountry);
            currencyInfoRepo.AddNewCountry(countryToAdd);
            if(!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            var returnCountryToAdd = mapper.Map<CountryDto>(countryToAdd);
            return CreatedAtAction("GetCountryById", new { id = countryToAdd.CountryId }, returnCountryToAdd);
        }

        // PUT currencyapi/updatecountry/{id}
        [HttpPut("updatecountry/{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryUpdateDto countryToUpdate)
        {
            if (countryToUpdate == null)
            {
                return BadRequest();
            }
            if (!Regex.IsMatch(countryToUpdate.CountryName, @"^[a-zA-Z ]+$"))
            {
                ModelState.AddModelError("Country Name Invalid", "The provided country name should contains letters and spaces only (e.g. New Zealand)");
            }
            if (!Regex.IsMatch(countryToUpdate.CurrencyCode, @"^[a-zA-Z]+$"))
            {
                ModelState.AddModelError("Currency Code Invalid", "The provided currency code should be only in format of 3 letters (e.g. CAD)");
            }
            if (countryToUpdate.CurrencyCode.Length != 3)
            {
                ModelState.AddModelError("Currency Code Invalid", "The provided currency code should be only in format of 3 letters (e.g. CAD)");
            }
            countryToUpdate.CurrencyCode = countryToUpdate.CurrencyCode.ToUpper();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await currencyInfoRepo.IsCountryExists(id))
            {
                return NotFound();
            }
            Country existingCountry = await currencyInfoRepo.GetCountryById(id, false);
            if (existingCountry == null)
            {
                return NotFound();
            }
            mapper.Map(countryToUpdate, existingCountry);
            if(!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }

        // DELETE currencyapi/deletecountry/{id}
        [HttpDelete("deletecountry/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var countryToDelete = await currencyInfoRepo.GetCountryById(id, false);
            if (countryToDelete == null)
            {
                return NotFound();
            }
            currencyInfoRepo.DeleteCountry(countryToDelete);
            if(!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }
    }
}

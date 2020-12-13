using AutoMapper;
using CurrencyInfoLibrary.Models;
using ExchangeAPI.DTOs;
using ExchangeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeAPI.Controllers
{
    [Route("currencyapi/[controller]")]
    [ApiController]
    public class CurrencyRateController : ControllerBase
    {
        private ICurrencyInfoRepository currencyInfoRepo;
        private readonly IMapper mapper;

        public CurrencyRateController(ICurrencyInfoRepository _currencyInfoRepo, IMapper _mapper)
        {
            currencyInfoRepo = _currencyInfoRepo;
            mapper = _mapper;
        }

        // GET: currencyapi/currencyrate/country/{countryId}
        [HttpGet("country/{countryId}")]
        public async Task<ActionResult<CurrencyRate>> GetAllCurrencyRatesByCountry(int countryId)
        {
            if (!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }

            var currencyRatesByCountry = await currencyInfoRepo.GetAllCurrencyRatesByCountry(countryId);
            var result = mapper.Map<IEnumerable<CurrencyRateDto>>(currencyRatesByCountry);
            return Ok(result);
        }

        // GET currencyapi/currencyrate/country/{countryId}/latest
        [HttpGet("country/{countryId}/latest")]
        public async Task<ActionResult<CurrencyRate>> GetLastestCurrencyRate(int countryId)
        {
            if (!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            var currencyRate = await currencyInfoRepo.GetLastestCurrencyRateByCountry(countryId);
            if (currencyRate == null)
            {
                return NotFound();
            }
            var result = mapper.Map<CurrencyRateDto>(currencyRate);
            return Ok(result);
        }

        // GET currencyapi/currencyrate/country/{countryId}/ondate/2020-11-17(T00:00:00)
        [HttpGet("country/{countryId}/ondate/{date}")]
        public async Task<ActionResult<CurrencyRate>> GetCurrencyRateByDate(int countryId, DateTime date)
        {
            if(!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            var currencyRate = await currencyInfoRepo.GetCurrencyRateByDate(countryId, date);
            if(currencyRate == null)
            {
                return NotFound();
            }
            var result = mapper.Map<CurrencyRateDto>(currencyRate);
            return Ok(result);
        }

        // GET currencyapi/currencyrate/country/{countryId}/datebetween/2020-11-17(T00:00:00)to2020-11-20(T00:00:00)
        [HttpGet("country/{countryId}/datebetween/{startDate}to{endDate}")]
        public async Task<ActionResult<CurrencyRate>> GetCurrencyRateByDate(int countryId, DateTime startDate, DateTime endDate)
        {
            if (!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            var currencyRates = await currencyInfoRepo.GetCurrencyRateBetweenDate(countryId, startDate, endDate);
            var result = mapper.Map<IEnumerable<CurrencyRateDto>>(currencyRates);
            return Ok(result);
        }

        // GET currencyapi/currencyrate/country/{countryId}/getrate/{id}
        [HttpGet("country/{countryId}/getrate/{id}")]
        public async Task<ActionResult<CurrencyRate>> GetCurrencyRateById(int countryId, int id)
        {
            if(!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            var currencyRate = await currencyInfoRepo.GetSpecificCurrencyRateByCountry(countryId, id);
            if(currencyRate == null)
            {
                return NotFound();
            }
            var result = mapper.Map<CurrencyRateDto>(currencyRate);
            return Ok(result);
        }

        // POST currencyapi/currencyrate/country/{countryId}/addrate
        [HttpPost("country/{countryId}/addrate")]
        public async Task<ActionResult<CurrencyRate>> AddCurrencyRateToCountry(int countryId, [FromBody] CurrencyRateCreateDto newCurrencyRate)
        {
            if (newCurrencyRate == null)
            {
                return BadRequest();
            }
            if(DateTime.Compare(DateTime.Now, newCurrencyRate.CurrencyDate) >= 0)
            {
                ModelState.AddModelError("Currency Date", "The added currency date must be present or future time.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            var currencyRateToAdd = mapper.Map<CurrencyRate>(newCurrencyRate);
            await currencyInfoRepo.AddNewCurrencyRate(countryId, currencyRateToAdd);
            if(!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            var returnCurrencyRateToAdd = mapper.Map<CurrencyRateDto>(currencyRateToAdd);
            return CreatedAtAction("GetCurrencyRateById", new { countryId = countryId, id = returnCurrencyRateToAdd.CurrencyRateId }, returnCurrencyRateToAdd);
        }

        // PUT currencyapi/currencyrate/country/{countryId}/updaterate/{id}
        [HttpPut("country/{countryId}/updaterate/{id}")]
        public async Task<IActionResult> UpdateCurrencyRate(int countryId, int id, [FromBody] CurrencyRateUpdateDto currencyRateToUpdate)
        {
            if (currencyRateToUpdate == null)
            {
                return BadRequest();
            }
            if (DateTime.Compare(DateTime.Now, currencyRateToUpdate.CurrencyDate) >= 0)
            {
                ModelState.AddModelError("Currency Date", "The added currency date must be present or future time.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!await currencyInfoRepo.IsCountryExists(countryId))
            {
                return NotFound();
            }
            CurrencyRate existingCurrencyRate = await currencyInfoRepo.GetSpecificCurrencyRateByCountry(countryId, id);
            if (existingCurrencyRate == null)
            {
                return NotFound();
            }
            mapper.Map(currencyRateToUpdate, existingCurrencyRate);
            if(!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }

        // DELETE currencyapi/currencyrate/country/{countryId}/deleterate/{id}
        [HttpDelete("country/{countryId}/deleterate/{id}")]
        public async Task<IActionResult> DeleteCurrencyRate(int countryId, int id)
        {
            var currencyRateToDelete = await currencyInfoRepo.GetSpecificCurrencyRateByCountry(countryId, id);
            if(currencyRateToDelete == null)
            {
                return NotFound();
            }
            currencyInfoRepo.DeleteCurrencyRate(currencyRateToDelete);
            if (!await currencyInfoRepo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            return NoContent();
        }
    }
}

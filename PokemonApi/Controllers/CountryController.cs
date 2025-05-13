using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]

    public class CountryController : Controller {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper) {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries() {
            var contries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(contries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId) {
            if (!_countryRepository.CountryExists(countryId)) {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }
        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId) {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate) {
            if (countryUpdate == null) {
                return BadRequest(ModelState);
            }
            if (countryId != countryUpdate.Id) {
                return BadRequest(ModelState);
            }
            if (!_countryRepository.CountryExists(countryId)) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var countryMap = _mapper.Map<Country>(countryUpdate);

            if (!_countryRepository.UpdateCountry(countryMap)) {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate) {
            if (countryCreate == null)
                return BadRequest("Category data is null");

            var country = _countryRepository.GetCountries()
                .FirstOrDefault(r => r.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper());

            if (country != null)
                return BadRequest("Category already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap)) {
                ModelState.AddModelError("", "Something went wrong when saving the reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");

        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId) {
            if (!_countryRepository.CountryExists(countryId)) {
                return NotFound();
            }
            var reviewerDelete = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(reviewerDelete)) {
                ModelState.AddModelError("", "Something went wrong trying to delete data");
            }
            return NoContent();

        }
    }
}


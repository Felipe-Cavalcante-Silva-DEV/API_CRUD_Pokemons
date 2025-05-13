using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository) {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners() {
            var owner = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId) {
            if (!_ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId) {
            if (!_ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }
            var pokemons = _ownerRepository.GetPokemonsByOwner(ownerId);
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemonsDto);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerUpdate) {
            if (ownerUpdate == null) {
                return BadRequest(ModelState);
            }
            if (ownerId != ownerUpdate.Id) {
                return BadRequest(ModelState);
            }
            if (!_ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(ownerUpdate);

            if (!_ownerRepository.UpdateOwner(ownerMap)) {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate) {
            if (ownerCreate == null)
                return BadRequest("Category data is null");

            var owner = _ownerRepository.GetOwners()
                .FirstOrDefault(r => r.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper());

            if (owner != null)
                return BadRequest("Category already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap)) {
                ModelState.AddModelError("", "Something went wrong when saving the reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");

        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId) {
            if (!_ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }
            var ownerDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerDelete)) {
                ModelState.AddModelError("", "Something went wrong trying to delete data");
            }
            return NoContent();

        }
    }
}

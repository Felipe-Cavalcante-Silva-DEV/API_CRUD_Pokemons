﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository,
            IReviewerRepository reviewerRepository,
            IMapper mapper) {
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons() {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) {
            if (!_pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId) {
            if (!_pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate) {
            if (pokemonCreate == null)
                return BadRequest(ModelState);


            var pokemonExists = _pokemonRepository.GetPokemons()
                    .Any(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper());

            if (pokemonExists) {
                return BadRequest(new { error = "Pokemon already exists" });
            }




            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)) {
                ModelState.AddModelError("", $"Something went wrong when saving the record {pokemonMap.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created Pokemon");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonUpdate) {
            if (pokemonUpdate == null) {
                return BadRequest(ModelState);
            }
            if (pokeId != pokemonUpdate.Id) {
                return BadRequest(ModelState);
            }
            if (!_pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var pokeMap = _mapper.Map<Pokemon>(pokemonUpdate);

            if (!_pokemonRepository.UpdatePokemon(pokeId, catId, pokeMap)) {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int Id) {
            if (!_pokemonRepository.PokemonExists(Id))
                return NotFound();

            var pokeDelete = _pokemonRepository.GetPokemon(Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_pokemonRepository.DeletePokemon(pokeDelete)) {
                ModelState.AddModelError("", "Something went wrong trying to delete data");
                return StatusCode(500, ModelState); // <-- Corrigido aqui
            }

            return NoContent();
        }

    }
}

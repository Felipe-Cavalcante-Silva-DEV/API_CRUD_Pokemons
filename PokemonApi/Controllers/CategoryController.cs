using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : Controller {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories() {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId) {
            if (!_categoryRepository.CategoryExists(categoryId)) {
                return NotFound();
            }

            var categories = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId) {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate) {
            if (categoryUpdate == null) {
                return BadRequest(ModelState);
            }
            if (categoryId != categoryUpdate.Id) {
                return BadRequest(ModelState);
            }
            if (!_categoryRepository.CategoryExists(categoryId)) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var categoryMap = _mapper.Map<Category>(categoryUpdate);
            if (!_categoryRepository.UpdateCategory(categoryMap)) {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate) {
            if (categoryCreate == null)
                return BadRequest("Category data is null");

            var category = _categoryRepository.GetCategories()
                .FirstOrDefault(r => r.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper());

            if (category != null)
                return BadRequest("Category already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap)) {
                ModelState.AddModelError("", "Something went wrong when saving the reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId) {
            if (!_categoryRepository.CategoryExists(categoryId)) {
                return NotFound();
            }
            var categoryDelete = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryDelete)) {
                ModelState.AddModelError("", "Something went wrong trying to delete data");
            }
            return NoContent();

        }
    }
}
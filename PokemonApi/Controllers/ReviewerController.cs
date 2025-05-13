using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interfaces;

namespace PokemonApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller {
        private readonly IReviewerRepository _reviwerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepository reviwerRepository, IMapper mapper) {
            _reviwerRepository = reviwerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers() {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviwerRepository.GetReviewers());

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{reviwerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviwerId) {
            if (!_reviwerRepository.ReviewerExists(reviwerId)) {
                return NotFound();
            }

            var reviewers = _mapper.Map<ReviewerDto>(_reviwerRepository.GetReviewer(reviwerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviwerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviwerId) {
            if (!_reviwerRepository.ReviewerExists(reviwerId)) {
                return NotFound();
            }

            var reviews = _mapper.Map<List<ReviewDto>>(_reviwerRepository.GetReviewsByReviewer(reviwerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate) {
            if (reviewerCreate == null)
                return BadRequest("Reviewer data is null");

            var reviewer = _reviwerRepository.GetReviewers()
                .FirstOrDefault(r => r.LastName.Trim().ToUpper() == reviewerCreate.LastName.Trim().ToUpper());

            if (reviewer != null)
                return BadRequest("Reviewer already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviwerRepository.CreateReviewer(reviewerMap)) {
                ModelState.AddModelError("", "Something went wrong when saving the reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerUpdate) {
            if (reviewerUpdate == null) {
                return BadRequest(ModelState);
            }
            if (reviewerId != reviewerUpdate.Id) {
                return BadRequest(ModelState);
            }
            if (!_reviwerRepository.ReviewerExists(reviewerId)) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);

            if (!_reviwerRepository.UpdateReviewer(reviewerMap)) {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId) {
            if (!_reviwerRepository.ReviewerExists(reviewerId)) {
                return NotFound();
            }
            var reviewerDelete = _reviwerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviwerRepository.DeleteReviewer(reviewerDelete)) {
                ModelState.AddModelError("", "Something went wrong trying to delete data");
            }
            return NoContent();

        }

    }
}



using AWP.Business.Interfaces;
using AWP.Business.Models;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuzzleMapController : ControllerBase
    {
        private readonly IBusinessPuzzleMap _businessPuzzleMap;

        public PuzzleMapController(IBusinessPuzzleMap businessPuzzleMap)
        {
            _businessPuzzleMap = businessPuzzleMap;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PuzzleMapDTO>> GetAll()
        {
            return Ok(_businessPuzzleMap.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<PuzzleMapDTO> GetById(string id)
        {
            var puzzleMap = _businessPuzzleMap.GetByID(id);
            if (puzzleMap == null)
            {
                return NotFound();
            }
            return Ok(puzzleMap);
        }

        [HttpPost]
        public ActionResult<PuzzleMapDTO> Create(PuzzleMapDTO puzzleMapDto)
        {
            if (_businessPuzzleMap.Insert(puzzleMapDto))
            {
                return CreatedAtAction(nameof(GetById), new { id = puzzleMapDto.Id }, puzzleMapDto);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, PuzzleMapDTO puzzleMapDto)
        {
            if (_businessPuzzleMap.Update(puzzleMapDto, id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (_businessPuzzleMap.Delete(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<PuzzleMapDTO>> GetList()
        {
            return Ok(_businessPuzzleMap.GetAll());
        }
    }
}

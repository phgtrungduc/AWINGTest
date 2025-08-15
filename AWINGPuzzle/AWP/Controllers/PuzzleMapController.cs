using AWP.Business.Interfaces;
using AWP.Business.Models;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace AWP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuzzleMapController : BaseController<PuzzleMap, PuzzleMapDTO>
    {
        private readonly IBusinessPuzzleMap _businessPuzzleMap;

        public PuzzleMapController(IBusinessPuzzleMap businessPuzzleMap) 
            : base(businessPuzzleMap)
        {
            _businessPuzzleMap = businessPuzzleMap;
        }

        // Base CRUD operations are inherited from BaseController

        [HttpGet("list")]
        public ActionResult<IEnumerable<PuzzleMapDTO>> GetList()
        {
            return Ok(_businessPuzzleMap.GetAll());
        }

        [HttpPost("solve")]
        public ActionResult<TreasureHuntResultDTO> SolveTreasureHunt(TreasureHuntInputDTO input)
        {
            try
            {
                // Basic input validation
                if (input == null)
                {
                    return BadRequest("Input data cannot be null");
                }

                // Solve the treasure hunt problem (validation is now handled inside the business layer)
                var result = _businessPuzzleMap.SolveTreasureHunt(input);

                // Save the puzzle to database
                var puzzleMapDto = new PuzzleMapDTO
                {
                    Id = Guid.NewGuid(),
                    Row = input.Rows,
                    Columnn = input.Columns,
                    MaxTarget = input.MaxTarget,
                    Matrix = JsonConvert.SerializeObject(input.Matrix),
					Result = result.MinimumFuel,
					CreatedAt = DateTime.Now
                };

                if (_businessPuzzleMap.Insert(puzzleMapDto))
                {
                    result.PuzzleId = puzzleMapDto.Id;
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

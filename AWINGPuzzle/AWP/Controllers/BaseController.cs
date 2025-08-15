using AWP.Business.Interfaces;
using AWP.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AWP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TDTO> : ControllerBase where TEntity : class
    {
        protected readonly IBusinessBase<TEntity, TDTO> _businessBase;

        public BaseController(IBusinessBase<TEntity, TDTO> businessBase)
        {
            _businessBase = businessBase;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of records</returns>
        [HttpGet]
        public virtual ActionResult<IEnumerable<TDTO>> GetAll()
        {
            return Ok(_businessBase.GetAll());
        }

        /// <summary>
        /// Get record by ID
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Record with specified ID</returns>
        [HttpGet("{id}")]
        public virtual ActionResult<TDTO> GetById(string id)
        {
            var entity = _businessBase.GetByID(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns>Created entity</returns>
        [HttpPost]
        public virtual ActionResult<TDTO> Create(TDTO entity)
        {
            try
            {
                if (_businessBase.Insert(entity))
                {
                    // Assuming the entity has an Id property that can be accessed
                    var id = GetEntityId(entity);
                    if (id != null)
                    {
                        return CreatedAtAction(nameof(GetById), new { id = id }, entity);
                    }
                    return Ok(new ServiceResult { Data = entity, Message = "Created successfully" });
                }
                
                return BadRequest(new ServiceResult 
                { 
                    StatusCode = (int)Enumeration.ResultCode.Failed, 
                    Message = "Failed to create entity" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult 
                { 
                    StatusCode = (int)Enumeration.ResultCode.Failed, 
                    Message = $"An error occurred: {ex.Message}" 
                });
            }
        }

        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <param name="entity">Updated entity</param>
        /// <returns>Result of operation</returns>
        [HttpPut("{id}")]
        public virtual IActionResult Update(string id, TDTO entity)
        {
            if (_businessBase.Update(entity, id))
            {
                return NoContent();
            }
            return BadRequest(new ServiceResult 
            { 
                StatusCode = (int)Enumeration.ResultCode.Failed, 
                Message = "Failed to update entity" 
            });
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Result of operation</returns>
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(string id)
        {
            if (_businessBase.Delete(id))
            {
                return NoContent();
            }
            return NotFound(new ServiceResult 
            { 
                StatusCode = (int)Enumeration.ResultCode.Failed, 
                Message = "Entity not found or could not be deleted" 
            });
        }

        /// <summary>
        /// Get records with paging
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged result</returns>
        [HttpGet("paging")]
        public virtual ActionResult GetByPaging(int page = 1, int pageSize = 10)
        {
            var result = _businessBase.GetByPaging(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Handle service result and return appropriate HTTP response
        /// </summary>
        /// <param name="result">Service result</param>
        /// <returns>HTTP response</returns>
        protected IActionResult HandleResponse(ServiceResult result)
        {
            switch (result.StatusCode)
            {
                case (int)Enumeration.ResultCode.Success:
                    return Ok(result);
                case 404: // NotFound is not in the enum
                    return NotFound(result);
                case (int)Enumeration.ResultCode.Failed:
                default:
                    return BadRequest(result);
            }
        }

        /// <summary>
        /// Helper method to get entity ID
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity ID as string</returns>
        private string GetEntityId(TDTO entity)
        {
            // Try to get ID property using reflection
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var value = idProperty.GetValue(entity);
                if (value != null)
                {
                    return value.ToString();
                }
            }
            return null;
        }
    }
}

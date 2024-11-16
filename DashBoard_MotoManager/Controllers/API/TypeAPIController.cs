using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace DashBoard_MotoManager.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeAPIController : ControllerBase
    {
        private readonly MotoWebsiteContext _context;
        private readonly IMapper _mapper;

        public TypeAPIController(MotoWebsiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        [HttpGet("Types")]
        public async Task<IActionResult> GetMotoTypes()
        {
            var types = await _context.MotoTypes.ToListAsync();
            var mappedResult = _mapper.Map<List<MotoTypeVM>>(types);
            return Ok(mappedResult);
        }

        [HttpGet("Types/{id}")]
        public async Task<IActionResult> GetMotoType(string id)
        {
            var type = await _context.MotoTypes.FindAsync(id);
            if (type == null)
            {
                return NotFound("Moto type not found.");
            }
            var mappedResult = _mapper.Map<MotoTypeVM>(type);
            return Ok(mappedResult);
        }


        [HttpPost("Types")]
        public async Task<IActionResult> CreateMotoType([FromBody] MotoTypeVM typeVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var type = _mapper.Map<MotoType>(typeVM);
            _context.MotoTypes.Add(type);
            await _context.SaveChangesAsync();

            var createdType = _mapper.Map<MotoTypeVM>(type);
            return CreatedAtAction(nameof(GetMotoType), new { id = createdType.MaLoai }, createdType);
        }

        // PUT: api/TypeAPI/{id}
        [HttpPut("Types/{id}")]
        public async Task<IActionResult> UpdateMotoType(string id, [FromBody] MotoTypeVM typeVM)
        {
            if (id != typeVM.MaLoai)
            {
                return BadRequest("ID mismatch.");
            }

            var existingType = await _context.MotoTypes.FindAsync(id);
            if (existingType == null)
            {
                return NotFound("Moto type not found.");
            }

            _mapper.Map(typeVM, existingType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/TypeAPI/{id}
        [HttpPatch("Types/{id}")]
        public async Task<IActionResult> UpdatePartialMotoType(string id, [FromBody] JsonPatchDocument<MotoTypeVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingType = await _context.MotoTypes.FindAsync(id);
            if (existingType == null)
            {
                return NotFound("Moto type not found.");
            }

            var typeToPatch = _mapper.Map<MotoTypeVM>(existingType);
            patchDoc.ApplyTo(typeToPatch, ModelState);

            if (!TryValidateModel(typeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(typeToPatch, existingType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/TypeAPI/{id}
        [HttpDelete("Types/{id}")]
        public async Task<IActionResult> DeleteMotoType(string id)
        {
            var type = await _context.MotoTypes.FindAsync(id);
            if (type == null)
            {
                return NotFound("Moto type not found.");
            }

            _context.MotoTypes.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

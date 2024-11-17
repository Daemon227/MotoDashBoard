using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace DashBoard_MotoManager.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoAPIController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public MotoAPIController(MotoWebsiteContext context, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Motos")]
        public async Task<IActionResult> GetMotos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.MotoBikes
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var motos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedResult = _mapper.Map<List<MotoDetailVM>>(motos);

            var paginationMetadata = new
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(mappedResult);
        }


        [HttpGet("Motos/{id}")]
        public async Task<IActionResult> GetMoto(string id)
        {
            var moto = await _db.MotoBikes
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .FirstOrDefaultAsync(m => m.MaXe == id);

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }

            var mappedResult = _mapper.Map<MotoDetailVM>(moto);
            return Ok(mappedResult);
        }

        [HttpPut("Motos/{id}")]
        public async Task<IActionResult> UpdateMoto(string id, [FromBody] MotoDetailVM motoVM)
        {
            if (id != motoVM.MaXe)
            {
                return BadRequest("ID mismatch.");
            }

            var existingMoto = await _db.MotoBikes.Include(m => m.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                    .ThenInclude(vc => vc.VersionImages)
                    .Include(m => m.MaLibraryNavigation)
                    .ThenInclude(l => l.LibraryImages)
                    .FirstOrDefaultAsync(m => m.MaXe == id);

            if (existingMoto == null)
            {
                return NotFound("Moto not found.");
            }

            _mapper.Map(motoVM, existingMoto);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotoExists(motoVM.MaXe))
                {
                    return NotFound("Moto not found.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPatch("Motos/{id}")]
        public async Task<IActionResult> UpdatePartialMoto(string id, [FromBody] JsonPatchDocument<MotoDetailVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingMoto = await _db.MotoBikes.Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .FirstOrDefaultAsync(m => m.MaXe == id);

            if (existingMoto == null)
            {
                return NotFound("Moto not found.");
            }

            var motoToPatch = _mapper.Map<MotoDetailVM>(existingMoto);
            patchDoc.ApplyTo(motoToPatch, ModelState);

            if (!TryValidateModel(motoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(motoToPatch, existingMoto);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotoExists(id))
                {
                    return NotFound("Moto not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MotoExists(string id)
        {
            return _db.MotoBikes.Any(e => e.MaXe == id);
        }

        [HttpDelete("Motos/{id}")]
        public async Task<IActionResult> DeleteMoto(string id)
        {
            var moto = await _db.MotoBikes
                .Include(m => m.MotoVersions)
                .ThenInclude(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .Include(m => m.MaLibraryNavigation)
                .ThenInclude(l => l.LibraryImages)
                .FirstOrDefaultAsync(m => m.MaXe == id);

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }

            _db.MotoBikes.Remove(moto);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Motos")]
        public async Task<IActionResult> CreateMoto([FromBody] MotoDetailVM motoVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMoto = _mapper.Map<MotoBike>(motoVM);

            _db.MotoBikes.Add(newMoto);
            await _db.SaveChangesAsync();

            var createdMoto = _mapper.Map<MotoDetailVM>(newMoto);

            return CreatedAtAction(nameof(GetMoto), new { id = newMoto.MaXe }, createdMoto);
        }

    }
}

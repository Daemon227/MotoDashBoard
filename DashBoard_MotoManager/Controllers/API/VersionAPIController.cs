using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionAPIController : ControllerBase
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public VersionAPIController(MotoWebsiteContext context, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Version")] 
        public async Task<IActionResult> GetVersions([FromQuery] int pageNumber =1, [FromQuery] int pageSize = 10) 
        {
            var versions = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .ToListAsync();
            var list = versions.ToPagedList(pageNumber, pageSize);
            var mappedResult = _mapper.Map<List<MotoVersionVM>>(list); 
            return Ok(mappedResult); 
        }

        [HttpGet("Version/{id}")]
        public async Task<IActionResult> GetVersion(string id)
        {
            var version = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (version != null)
            {
                var result = _mapper.Map<MotoVersionVM>(version);
                return Ok(result);
            }
            else
            {
                return NotFound("Khong tim thay");
            }
        }

        [HttpPost("Version")]
        public async Task<IActionResult> CreateVersion([FromBody] MotoVersionVM versionVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newVersion = _mapper.Map<MotoVersion>(versionVM);

            _db.MotoVersions.Add(newVersion);
            await _db.SaveChangesAsync();

            var createdVersion = _mapper.Map<MotoVersionVM>(newVersion);

            return CreatedAtAction(nameof(GetVersion), new { id = newVersion.MaXe }, createdVersion);
        }

        [HttpPut("Version/{id}")]
        public async Task<IActionResult> UpdateVersion(string id, [FromBody] MotoVersionVM versionVM)
        {
            if (id != versionVM.MaVersion)
            {
                return BadRequest("ID mismatch.");
            }

            var existingVersion = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (existingVersion == null)
            {
                return NotFound("Version not found.");
            }

            _mapper.Map(versionVM, existingVersion);

            // Xóa các đối tượng con hiện tại
            _db.VersionColors.RemoveRange(existingVersion.VersionColors);
            _db.VersionImages.RemoveRange(existingVersion.VersionColors.SelectMany(vc => vc.VersionImages));

            // Thêm các đối tượng con mới
            foreach (var color in existingVersion.VersionColors)
            {
                _db.Entry(color).State = EntityState.Added;
                foreach (var image in color.VersionImages)
                {
                    _db.Entry(image).State = EntityState.Added;
                }
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("Version/{id}")]
        public async Task<IActionResult> UpdatePartialVersion(string id, [FromBody] JsonPatchDocument<MotoVersionVM> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var existingVersion = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (existingVersion == null)
            {
                return NotFound("Version not found.");
            }

            var versionToPatch = _mapper.Map<MotoVersionVM>(existingVersion);
            patchDoc.ApplyTo(versionToPatch, ModelState);

            if (!TryValidateModel(versionToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // Xóa theo dõi các đối tượng con hiện tại khỏi ngữ cảnh
            _db.ChangeTracker.Clear();

            _mapper.Map(versionToPatch, existingVersion);

            // Xử lý các đối tượng con
            foreach (var colorVM in versionToPatch.VersionColorsVM)
            {
                var existingColor = existingVersion.VersionColors.FirstOrDefault(vc => vc.MaVersionColor == colorVM.MaVersionColor);
                if (existingColor == null)
                {
                    var newColor = _mapper.Map<VersionColor>(colorVM);
                    existingVersion.VersionColors.Add(newColor);
                }
                else
                {
                    _mapper.Map(colorVM, existingColor);
                    foreach (var imageVM in colorVM.versionImageVM)
                    {
                        var existingImage = existingColor.VersionImages.FirstOrDefault(img => img.ImageId == imageVM.ImageId);
                        if (existingImage == null)
                        {
                            var newImage = _mapper.Map<VersionImage>(imageVM);
                            existingColor.VersionImages.Add(newImage);
                        }
                        else
                        {
                            _mapper.Map(imageVM, existingImage);
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("Version/{id}")]
        public async Task<IActionResult> DeleteVersion(string id)
        {
            var version = await _db.MotoVersions
                .Include(v => v.VersionColors)
                .ThenInclude(vc => vc.VersionImages)
                .FirstOrDefaultAsync(v => v.MaVersion == id);

            if (version == null)
            {
                return NotFound("Version not found.");
            }

            // Xóa các đối tượng con trước khi xóa đối tượng cha
            _db.VersionColors.RemoveRange(version.VersionColors);
            _db.VersionImages.RemoveRange(version.VersionColors.SelectMany(vc => vc.VersionImages));

            _db.MotoVersions.Remove(version);
            await _db.SaveChangesAsync();
            return NoContent();
        }



    }
}

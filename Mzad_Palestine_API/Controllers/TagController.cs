using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Tag;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly JsonSerializerOptions _jsonOptions;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tags = await _tagService.GetAllAsync();
                return Ok(new { success = true, data = tags });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var tag = await _tagService.GetByIdAsync(id);
                if (tag == null)
                    return NotFound(new { success = false, error = "الوسم غير موجود" });

                return Ok(new { success = true, data = tag });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new { success = false, error = "اسم الوسم مطلوب" });
                }

                var tag = await _tagService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = tag.Id }, new { success = true, data = tag });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTagDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new { success = false, error = "اسم الوسم مطلوب" });
                }

                var tag = await _tagService.UpdateAsync(id, dto);
                if (tag == null)
                    return NotFound(new { success = false, error = "الوسم غير موجود" });

                return Ok(new { success = true, data = tag });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var success = await _tagService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { success = false, error = "الوسم غير موجود" });

                return Ok(new { success = true, message = "تم حذف الوسم بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { success = false, error = "الرجاء إدخال نص للبحث" });
                }

                var tags = await _tagService.SearchAsync(query);
                return Ok(new { success = true, data = tags });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("listing/{listingId:int}")]
        public async Task<IActionResult> AddTagToListing(int listingId, [FromBody] ListingTagDto dto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                if (listingId != dto.ListingId)
                {
                    return BadRequest(new { success = false, error = "معرف القائمة غير صحيح" });
                }

                var success = await _tagService.AddTagToListingAsync(dto);
                if (!success)
                    return NotFound(new { success = false, error = "الوسم أو القائمة غير موجودة" });

                return Ok(new { success = true, message = "تم إضافة الوسم للقائمة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("listing/{listingId:int}/tag/{tagId:int}")]
        public async Task<IActionResult> RemoveTagFromListing(int listingId, int tagId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { success = false, error = "الرجاء تسجيل الدخول" });
                }

                var success = await _tagService.RemoveTagFromListingAsync(listingId, tagId);
                if (!success)
                    return NotFound(new { success = false, error = "الوسم أو القائمة غير موجودة" });

                return Ok(new { success = true, message = "تم إزالة الوسم من القائمة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
} 
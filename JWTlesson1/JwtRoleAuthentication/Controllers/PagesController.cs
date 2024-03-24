using Asp.Versioning;
using JWTlesson1.API.Enums.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTlesson1.API.JwtRoleAuthentication.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public PagesController(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("new")]
        public async Task<ActionResult<Page>> CreatePage(PageDto pageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var page = new Page
            {
                Author = pageDto.Author,
                Body = pageDto.Body,
                Title = pageDto.Title
            };
            };

            _dbContext.Pages.Add(page);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageDto>> GetPage(int id)
        {
            var page = await _dbContext.Pages.FindAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            var pageDto = new PageDto
            {
                Id = page.Id,
                Author = page.Author,
                Body = page.Body,
                Title = page.Title
            };

            return pageDto;
        }


        [HttpGet]
        public async Task<PagesDto> ListPages()
        {
            var pagesFromDb = await _dbContext.Pages.ToListAsync();

            var pagesDto = new PagesDto();

            foreach (var page in pagesFromDb)
            {
                var pageDto = new PageDto
                {
                    Id = page.Id,
                    Author = page.Author,
                    Body = page.Body,
                    Title = page.Title
                };

                pagesDto.Pages.Add(pageDto);
            }

            return pagesDto;
        }
    }
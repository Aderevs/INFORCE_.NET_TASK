using AutoMapper;
using INFORCE_.NET_TASK.Server.DbLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Server.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlShortenerContext _context;
        private readonly IMapper _mapper;
        public UrlController(UrlShortenerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllUrls")]
        public async Task<IEnumerable<ShortenedUrlDTO>> Index()
        {
            var allUrls = await _context.Urls
                .Include(u => u.User)
                .ToListAsync();
            var allUrlsDto = _mapper.Map<List<ShortenedUrlDTO>>(allUrls);
            return allUrlsDto;
        }

        [HttpPost("CreateShortenedUrl")]
        [Authorize]
        public async Task<IActionResult> CreateUrl([FromBody] UrlModel model)
        {
            ShortenedUrlDTO urlDto = _mapper.Map<ShortenedUrlDTO>(model);

            if (!Uri.IsWellFormedUriString(urlDto.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL");
            }
            var urlAlreadyExists = await _context.Urls
                .AnyAsync(u => u.OriginalUrl == urlDto.OriginalUrl);
            if (urlAlreadyExists)
            {
                return BadRequest(
                    new
                    {
                        error = "Shortened url for this url already exists"
                    });
            }
            bool ifShortUrlAlreadyExists;
            string shortUrl;
            do
            {
                shortUrl = GenerateShortUrl();
                ifShortUrlAlreadyExists = await _context.Urls
                    .AnyAsync(u => u.ShortUrl == shortUrl);
            } while (ifShortUrlAlreadyExists);
            urlDto.ShortUrl = shortUrl;
            if (urlDto.Id == null)
            {
                urlDto.Id = Guid.NewGuid();
            }
            if (urlDto.CreatedDate == null)
            {
                var now = DateTime.Now;
                urlDto.CreatedDate = new(now.Year, now.Month, now.Year);
            }
            var url = _mapper.Map<ShortenedUrl>(urlDto);

            url.User = await _context.Users.FirstAsync(u => u.Login == User.Identity.Name);
            url.UserId = url.User.Id;

            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
            return Ok(urlDto);
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
        {
            shortUrl = "https://localhost:7073/api/url/" + shortUrl;
            var urlOrNull = await _context.Urls
                .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
            if (urlOrNull is ShortenedUrl url)
            {
                return Redirect(url.OriginalUrl);
            }
            return NotFound("URL not found");
        }

        [Authorize]
        [HttpGet("GetUrl/{id}")]
        public async Task<IActionResult> GetCertainUrl(Guid id)
        {
            var urlOrNull = await _context.Urls
                .FirstOrDefaultAsync(u => u.Id == id);
            if (urlOrNull is ShortenedUrl url)
            {
                var urlDto = _mapper.Map<ShortenedUrlDTO>(url);
                return Ok(urlDto);
            }
            else
            {
                return NotFound("URL with this identifier was not found");
            }
        }

        [Authorize]
        [HttpDelete("DeleteUrl/{id}")]
        public async Task<IActionResult> DeleteUrl(Guid id)
        {
            bool hasPermission = false;
            var urlOrNull = await _context.Urls
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(u => u.Id == id);
            if (urlOrNull is ShortenedUrl url)
            {
                if (User.IsInRole("Admin"))
                {
                    hasPermission = true;
                }
                else
                {
                    if (url.User.Login == User.Identity.Name)
                    {
                        hasPermission = true;
                    }

                }
            }
            else
            {
                return NotFound("No url with such id");
            }
            

            if (hasPermission)
            {
                _context.Urls.Remove(url);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Forbid("You do not have permission to perform this action");
            }
            
        }

        private string GenerateShortUrl()
        {
            return "https://localhost:7073/api/url/" + Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}

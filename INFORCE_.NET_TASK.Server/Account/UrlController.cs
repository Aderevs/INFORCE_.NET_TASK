﻿using AutoMapper;
using INFORCE_.NET_TASK.Server.DbLogic;
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

        [HttpGet(Name = "GetAllUrls")]
        public async Task<IEnumerable<ShortenedUrlDTO>> Index()
        {
            var allUrls = await _context.Urls
                .Include(u => u.User)
                .ToListAsync();
            var allUrlsDto = _mapper.Map<List<ShortenedUrlDTO>>(allUrls);
            return allUrlsDto;
        }

        [HttpPost("CreateShortenedUrl")]
        public async Task<IActionResult> CreateUrl([FromBody] ShortenedUrlDTO urlDto)
        {
            if (!Uri.IsWellFormedUriString(urlDto.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL");
            }
            bool ifUrlAlreadyExists;
            string shortUrl;
            do
            {
                shortUrl = GenerateShortUrl();
                ifUrlAlreadyExists = await _context.Urls
                    .AnyAsync(u => u.ShortUrl == shortUrl);
            } while (ifUrlAlreadyExists);
            urlDto.ShortedUrl = shortUrl;
            var url = _mapper.Map<ShortenedUrl>(urlDto);
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
            return Ok(urlDto);
        }
        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
        {
            var urlOrNull = await _context.Urls
                .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
            if(urlOrNull is ShortenedUrl url)
            {
                return Redirect(url.OriginalUrl);
            }
            return NotFound("URL not found");
        }


        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
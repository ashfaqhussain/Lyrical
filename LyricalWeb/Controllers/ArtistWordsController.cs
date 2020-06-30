using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LyricalWeb.Services;
using LyricalWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyricalWeb.Controllers
{
    [ApiController]
    [Route("artist-words")]
    public class ArtistWordsController : ControllerBase
    {
        private readonly ILogger<ArtistWordsController> _logger;
        private readonly IArtistWordsService _artistWordsService;

        public ArtistWordsController(ILogger<ArtistWordsController> logger, IArtistWordsService artistWordsService)
        {
            _logger = logger;
            _artistWordsService = artistWordsService;
        }

        [HttpGet]
        public async Task<object> Get([Required]string artistName)
        {
            if (string.IsNullOrWhiteSpace(artistName))
            {
                ModelState.AddModelError("artistName", "Artist name must be provided");
                return BadRequest();
            }

            var averageWordCount = await _artistWordsService.FetchAverageWordCount(artistName);

            return new ArtistWordsViewModel
            {
                AverageWordCount = averageWordCount
            };
        }
    }
}

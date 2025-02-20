using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Health.DesafioBack.Models;
using Health.DesafioBack.Data;
using Microsoft.Extensions.Configuration;

namespace Health.DesafioBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubeController : ControllerBase
    {
        private readonly string? _apiKey;
        private readonly string? _youtubeApiUrl;
        private readonly AppDbContext _context;
        private List<Video> videos;

        public YoutubeController(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _apiKey = configuration["YouTubeSettings:ApiKey"];
            _youtubeApiUrl = configuration["YouTubeSettings:ApiUrl"];
        }

        [HttpGet("buscar-videos")]
        public async Task<IActionResult> BuscarVideos()
        {
            videos = new List<Video>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string query = "manipulação de medicamentos";
                    string url = $"{_youtubeApiUrl}?part=snippet&q={Uri.EscapeDataString(query)}&regionCode=BR&type=video&publishedAfter=2025-01-01T00:00:00Z&publishedBefore=2025-12-31T23:59:59Z&key={_apiKey}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return StatusCode((int)response.StatusCode, "Erro na requisição à API do YouTube");

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    videos = ProcessarResposta(jsonResponse);

                    if (videos != null && videos.Count > 0)
                    {
                        // Adiciona somente vídeos que ainda não estão no banco
                        foreach (var video in videos)
                        {
                            if (!_context.Videos.Any(v => v.VideoId == video.VideoId))
                            {
                                _context.Videos.Add(video);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    return Ok(videos);
                }
            }
            catch (Exception ex)
            {
                // Retorna erro 500 em vez de 400 para exceções internas
                return StatusCode(500, $"Erro ao processar a requisição: {ex.Message}");
            }
        }

        private List<Video> ProcessarResposta(string json)
        {
            List<Video> listaVideos = new List<Video>();
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            try
            {
                foreach (JsonElement item in root.GetProperty("items").EnumerateArray())
                {
                    var video = new Video
                    {
                        VideoId = item.GetProperty("id").GetProperty("videoId").GetString(),
                        Titulo = item.GetProperty("snippet").GetProperty("title").GetString(),
                        Descricao = item.GetProperty("snippet").GetProperty("description").GetString(),
                        Canal = item.GetProperty("snippet").GetProperty("channelTitle").GetString(),
                        Url = $"https://www.youtube.com/watch?v={item.GetProperty("id").GetProperty("videoId").GetString()}"
                    };

                    listaVideos.Add(video);
                }

                return listaVideos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar resposta do YouTube: {ex.Message}");
                return null;
            }
        }

    }
}

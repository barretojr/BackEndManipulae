using Health.DesafioBack.Data;
using Health.DesafioBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Health.DesafioBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VideoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Video (Filtragem)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> Listar(
            string? titulo,
            int? duracao,
            string? autor,
            DateTime? dataCriacao,
            string? q)
        {
            var query = _context.Videos.AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(v => v.Titulo.Contains(titulo));

            if (duracao.HasValue)
                query = query.Where(v => v.Duracao == duracao.Value);

            if (!string.IsNullOrEmpty(autor))
                query = query.Where(v => v.Autor.Contains(autor));

            if (dataCriacao.HasValue)
                query = query.Where(v => v.DataCriacao >= dataCriacao.Value);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(v => v.Titulo.Contains(q) || v.Descricao.Contains(q) || v.Autor.Contains(q));

            return await query.Where(v => !v.Excluido).ToListAsync();
        }

        // GET api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> Filtrar(Video form)
        {
            var video = await _context.Videos.FindAsync(form.Id);

            if (video == null || video.Excluido)
                return NotFound("Vídeo não encontrado.");

            return video;
        }

        // POST api/Video (Inserção)
        [HttpPost]
        public async Task<ActionResult<Video>> Inserir([FromBody] Video video)
        {
            video.DataCriacao = DateTime.UtcNow;
            video.Excluido = false;

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Filtrar), new { id = video.Id }, video);
        }

        // PUT api/Video/5 (Atualização)
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Video videoAtualizado)
        {
            var videoExistente = await _context.Videos.FindAsync(id);
            if (videoExistente == null || videoExistente.Excluido)
                return NotFound("Vídeo não encontrado.");

            videoExistente.Titulo = videoAtualizado.Titulo;
            videoExistente.Descricao = videoAtualizado.Descricao;
            videoExistente.Autor = videoAtualizado.Autor;
            videoExistente.Duracao = videoAtualizado.Duracao;
            videoExistente.DataCriacao = videoAtualizado.DataCriacao;

            _context.Videos.Update(videoExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/Video/5 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null || video.Excluido)
                return NotFound("Vídeo não encontrado.");

            video.Excluido = true;
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

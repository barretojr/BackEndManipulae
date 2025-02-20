using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health.DesafioBack.Models
{
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Autor { get; set; }

        public int Duracao { get; set; }

        public DateTime DataCriacao { get; set; }

        public string VideoId { get; set; }

        public string Canal { get; set; }

        public string Url { get; set; }

        public bool Excluido { get; set; }
    }
}

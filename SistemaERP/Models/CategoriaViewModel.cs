using System.ComponentModel.DataAnnotations;

namespace SistemaERP.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

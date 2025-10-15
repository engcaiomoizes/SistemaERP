using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SistemaERP.Models
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaViewModel Categoria { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Preco { get; set; }
        public bool ControlarEstoque { get; set; }
        public string Embalagem { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public EstoqueViewModel Estoque { get; set; }
    }
}

namespace SistemaERP.Models
{
    public class ProdutoCategoriaRequestModel
    {
        public ProdutoRequestModel Produto { get; set; }
        public IEnumerable<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
    }
}

namespace SistemaERP.Models
{
    public class ProdutoRequestModel
    {
        public string Nome { get; set; }
        public int CategoriaId { get; set; }
        public string CodigoBarras { get; set; }
        public string Preco { get; set; }
        public string Embalagem { get; set; }
        public bool ControlarEstoque { get; set; }
    }
}

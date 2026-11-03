
namespace SisVenda.Models
{
    public class Pedidos
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public Cliente? Cliente { get; set; }
        public string? Observacao { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public decimal Total => Itens.Sum(item => item.Total);
        public string ClienteNome => Cliente?.Nome ?? "Cliente não informado";
        public string TotalFormatado => Total.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
    }
    public class ItemPedido
    {
        public int Id { get; set; }
        public Produto? Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Total => Quantidade * PrecoUnitario;
        public string ProdutoNome => Produto?.Nome ?? string.Empty;
        public string PrecoUnitarioFormatado => PrecoUnitario.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
        public string TotalFormatado => Total.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
    }
}

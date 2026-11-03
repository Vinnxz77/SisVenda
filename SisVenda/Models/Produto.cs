namespace SisVenda.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
        public decimal Preco { get; set; }
        public string? Descricao { get; set; }
        public int Quantidade { get; set; }

        public string PrecoFormatado => Preco.ToString("C2", System.Globalization.CultureInfo.CurrentCulture);
    }
}

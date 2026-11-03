namespace SisVenda.Models;

public class EstadoBrasil
{
    public int Id { get; set; }
    public string Sigla { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public List<string> Cidades { get; set; } = new();
    public string Exibicao => $"{Nome} ({Sigla})";
}

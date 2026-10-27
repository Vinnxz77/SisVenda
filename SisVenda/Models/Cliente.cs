namespace SisVenda.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public string? CEP { get; set; }

        // Helper properties for display in the grid
        public string CpfCnpj => !string.IsNullOrWhiteSpace(CPF) ? CPF! : (CNPJ ?? string.Empty);
        public string TelefoneOuCelular => !string.IsNullOrWhiteSpace(Celular) ? Celular! : (Telefone ?? string.Empty);
    }
}

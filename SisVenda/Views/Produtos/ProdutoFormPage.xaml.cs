using System.Globalization;
using Microsoft.Maui.Controls;
using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda
{
    [QueryProperty(nameof(ProductId), "id")]
    public partial class ProdutoFormPage : ContentPage
    {
        int productId = 0;

        public string ProductId
        {
            get => productId.ToString();
            set
            {
                if (int.TryParse(value, out var id))
                {
                    productId = id;
                }
            }
        }

        public ProdutoFormPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (productId > 0)
            {
                var p = ProdutoRepository.GetById(productId);
                if (p != null)
                {
                    PopulateForm(p);
                }
            }
            else
            {
                ClearForm();
            }
        }

        private async void Salvar_Clicked(object sender, EventArgs e)
        {
            var nome = nomeEntry.Text?.Trim();
            var descricao = descricaoEditor.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                await DisplayAlert("Validação", "Nome é obrigatório.", "OK");
                return;
            }

            if (!TryParsePreco(precoEntry.Text, out var preco))
            {
                await DisplayAlert("Validação", "Preço inválido.", "OK");
                return;
            }

            if (!int.TryParse(quantidadeEntry.Text, out var quantidade) || quantidade < 0)
            {
                await DisplayAlert("Validação", "Quantidade inválida.", "OK");
                return;
            }

            if (productId > 0)
            {
                var p = ProdutoRepository.GetById(productId);
                if (p != null)
                {
                    p.Nome = nome;
                    p.Preco = preco;
                    p.Descricao = descricao;
                    p.Quantidade = quantidade;
                }
            }
            else
            {
                var novo = new Produto
                {
                    Id = ProdutoRepository.Produtos.Any() ? ProdutoRepository.Produtos.Max(x => x.Id) + 1 : 1,
                    Nome = nome,
                    Preco = preco,
                    Descricao = descricao,
                    Quantidade = quantidade
                };
                ProdutoRepository.Add(novo);
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void Cancelar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        static bool TryParsePreco(string? texto, out decimal preco)
        {
            if (decimal.TryParse(texto, NumberStyles.Number, new CultureInfo("pt-BR"), out preco))
            {
                return true;
            }

            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out preco);
        }

        void PopulateForm(Produto p)
        {
            nomeEntry.Text = p.Nome;
            precoEntry.Text = p.Preco.ToString("F2", new CultureInfo("pt-BR"));
            descricaoEditor.Text = p.Descricao;
            quantidadeEntry.Text = p.Quantidade.ToString();
        }

        void ClearForm()
        {
            nomeEntry.Text = string.Empty;
            precoEntry.Text = string.Empty;
            descricaoEditor.Text = string.Empty;
            quantidadeEntry.Text = string.Empty;
        }
    }
}

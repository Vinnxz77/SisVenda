using Microsoft.Maui.Controls;
using SisVenda.Services;

namespace SisVenda
{
    public partial class ProdutoPage : ContentPage
    {
        public ProdutoPage()
        {
            InitializeComponent();
            produtosCollectionView.ItemsSource = ProdutoRepository.Produtos;
        }

        private async void ToolbarItem_Novo_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProdutoFormPage));
        }

        private async void Editar_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                await Shell.Current.GoToAsync($"{nameof(ProdutoFormPage)}?id={id}");
            }
        }

        private async void Excluir_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                var produto = ProdutoRepository.GetById(id);
                if (produto is null) return;
                var ok = await DisplayAlert("Confirmar", $"Excluir {produto.Nome}?", "Sim", "Não");
                if (!ok) return;
                ProdutoRepository.Remove(produto);
            }
        }
    }
}

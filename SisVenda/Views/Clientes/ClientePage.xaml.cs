using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda
{
    public partial class ClientePage : ContentPage
    {
        public ClientePage()
        {
            InitializeComponent();
            clientesCollectionView.ItemsSource = ClienteRepository.Clientes;
        }

        private async void ToolbarItem_Novo_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClienteFormPage));
        }

        private async void Editar_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                await Shell.Current.GoToAsync($"{nameof(ClienteFormPage)}?id={id}");
            }
        }

        private async void Excluir_Clicked(object sender, EventArgs e)
        {
            if (sender is Button b && b.CommandParameter is int id)
            {
                var cliente = ClienteRepository.GetById(id);
                if (cliente is null) return;
                var ok = await DisplayAlert("Confirmar", $"Excluir {cliente.Nome}?", "Sim", "Não");
                if (!ok) return;
                ClienteRepository.Remove(cliente);
            }
        }
    }
}

using SisVenda.Services;

namespace SisVenda;

public partial class PedidoPage : ContentPage
{
    public PedidoPage()
    {
        InitializeComponent();
        pedidosCollectionView.ItemsSource = PedidoRepository.Pedidos;
    }

    private async void Novo_Clicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PedidoFormPage));
    }

    private async void Editar_Clicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int id)
        {
            await Shell.Current.GoToAsync($"{nameof(PedidoFormPage)}?id={id}");
        }
    }

    private async void Excluir_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int id)
            return;

        var pedido = PedidoRepository.GetById(id);
        if (pedido is null)
            return;

        if (await DisplayAlertAsync("Confirmar", $"Excluir o pedido de {pedido.ClienteNome}?", "Sim", "Não"))
            PedidoRepository.Remove(pedido);
    }
}

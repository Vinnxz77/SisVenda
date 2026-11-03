using System.Collections.ObjectModel;
using System.Globalization;
using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda;

[QueryProperty(nameof(OrderId), "id")]
public partial class PedidoFormPage : ContentPage
{
    private readonly ObservableCollection<ItemPedido> itens = new();
    private int orderId;

    public string OrderId
    {
        get => orderId.ToString(CultureInfo.InvariantCulture);
        set
        {
            if (int.TryParse(value, out var id))
                orderId = id;
        }
    }

    public PedidoFormPage()
    {
        InitializeComponent();
        clientePicker.ItemsSource = ClienteRepository.Clientes;
        produtoPicker.ItemsSource = ProdutoRepository.Produtos;
        itensCollectionView.ItemsSource = itens;
        dataPicker.Date = DateTime.Today;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (orderId > 0)
        {
            var pedido = PedidoRepository.GetById(orderId);
            if (pedido is not null && itens.Count == 0)
            {
                clientePicker.SelectedItem = pedido.Cliente;
                dataPicker.Date = pedido.Data;
                observacaoEditor.Text = pedido.Observacao;
                foreach (var item in pedido.Itens)
                {
                    itens.Add(new ItemPedido
                    {
                        Id = item.Id,
                        Produto = item.Produto,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario
                    });
                }
                AtualizarTotal();
            }
        }
    }

    private void ProdutoPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (produtoPicker.SelectedItem is Produto produto)
            precoEntry.Text = produto.Preco.ToString("F2", new CultureInfo("pt-BR"));
    }

    private async void AdicionarItem_Clicked(object? sender, EventArgs e)
    {
        if (produtoPicker.SelectedItem is not Produto produto)
        {
            await DisplayAlertAsync("Validação", "Selecione um produto.", "OK");
            return;
        }

        if (!int.TryParse(quantidadeEntry.Text, out var quantidade) || quantidade <= 0)
        {
            await DisplayAlertAsync("Validação", "A quantidade deve ser maior que zero.", "OK");
            return;
        }

        if (!TryParseDecimal(precoEntry.Text, out var preco) || preco <= 0)
        {
            await DisplayAlertAsync("Validação", "O valor unitário deve ser maior que zero.", "OK");
            return;
        }

        var quantidadeNoPedido = itens
            .Where(item => item.Produto?.Id == produto.Id)
            .Sum(item => item.Quantidade);
        if (quantidadeNoPedido + quantidade > produto.Quantidade)
        {
            await DisplayAlertAsync(
                "Estoque insuficiente",
                $"A quantidade solicitada ({quantidadeNoPedido + quantidade}) é maior que o estoque disponível de {produto.Quantidade} unidade(s).",
                "OK");
            return;
        }

        itens.Add(new ItemPedido
        {
            Id = itens.Count == 0 ? 1 : itens.Max(item => item.Id) + 1,
            Produto = produto,
            Quantidade = quantidade,
            PrecoUnitario = preco
        });
        quantidadeEntry.Text = string.Empty;
        produtoPicker.SelectedIndex = -1;
        precoEntry.Text = string.Empty;
        AtualizarTotal();
    }

    private void RemoverItem_Clicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ItemPedido item)
        {
            itens.Remove(item);
            AtualizarTotal();
        }
    }

    private async void Salvar_Clicked(object? sender, EventArgs e)
    {
        var cliente = clientePicker.SelectedItem as Cliente;

        if (itens.Count == 0)
        {
            await DisplayAlertAsync("Validação", "Inclua pelo menos um item no pedido.", "OK");
            return;
        }

        foreach (var grupo in itens.Where(item => item.Produto is not null).GroupBy(item => item.Produto!.Id))
        {
            var produto = grupo.First().Produto!;
            var quantidadeSolicitada = grupo.Sum(item => item.Quantidade);
            if (quantidadeSolicitada > produto.Quantidade)
            {
                await DisplayAlertAsync(
                    "Estoque insuficiente",
                    $"A quantidade solicitada de {produto.Nome} ({quantidadeSolicitada}) é maior que o estoque disponível de {produto.Quantidade} unidade(s).",
                    "OK");
                return;
            }
        }

        if (orderId > 0)
        {
            var pedido = PedidoRepository.GetById(orderId);
            if (pedido is not null)
            {
                pedido.Cliente = cliente;
                pedido.Data = dataPicker.Date ?? DateTime.Today;
                pedido.Observacao = observacaoEditor.Text?.Trim();
                pedido.Itens = itens.ToList();
            }
        }
        else
        {
            PedidoRepository.Add(new Pedidos
            {
                Id = PedidoRepository.Pedidos.Count == 0 ? 1 : PedidoRepository.Pedidos.Max(pedido => pedido.Id) + 1,
                Cliente = cliente,
                Data = dataPicker.Date ?? DateTime.Today,
                Observacao = observacaoEditor.Text?.Trim(),
                Itens = itens.ToList()
            });
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void Cancelar_Clicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void AtualizarTotal()
    {
        totalLabel.Text = itens.Sum(item => item.Total).ToString("C2", CultureInfo.CurrentCulture);
    }

    private static bool TryParseDecimal(string? text, out decimal value)
    {
        return decimal.TryParse(text, NumberStyles.Number, new CultureInfo("pt-BR"), out value)
            || decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
    }
}

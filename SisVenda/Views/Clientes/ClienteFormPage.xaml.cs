using System;
using System.Text.Json;
using Microsoft.Maui.Controls;
using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda
{
    [QueryProperty(nameof(ClientId), "id")]
    public partial class ClienteFormPage : ContentPage
    {
        int clientId = 0;
        readonly List<EstadoBrasil> estados = new();
        bool localidadesCarregadas;

        public string ClientId
        {
            get => clientId.ToString();
            set
            {
                if (int.TryParse(value, out var id))
                {
                    clientId = id;
                }
            }
        }

        public ClienteFormPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarLocalidadesAsync();
            if (clientId > 0)
            {
                var c = ClienteRepository.GetById(clientId);
                if (c != null)
                {
                    PopulateForm(c);
                }
            }
            else
            {
                ClearForm();
            }
        }

        private void EstadoPicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (estadoPicker.SelectedIndex < 0) return;
            if (estadoPicker.SelectedItem is EstadoBrasil estado)
                AtualizarCidades(estado);
            else
                cidadePicker.ItemsSource = null;
        }

        private async void Salvar_Clicked(object? sender, EventArgs e)
        {
            var nome = nomeEntry.Text?.Trim();
            var email = emailEntry.Text?.Trim();
            var cpf = cpfEntry.Text?.Trim();
            var cnpj = cnpjEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                await DisplayAlertAsync("Validação", "Nome é obrigatório.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(cpf) && string.IsNullOrWhiteSpace(cnpj))
            {
                await DisplayAlertAsync("Validação", "Informe o CPF ou o CNPJ do cliente.", "OK");
                return;
            }

            if (clientId > 0)
            {
                var c = ClienteRepository.GetById(clientId);
                if (c != null)
                {
                    c.Nome = nome;
                    c.CNPJ = cnpj;
                    c.CPF = cpf;
                    c.RG = rgEntry.Text;
                    c.Telefone = telefoneEntry.Text;
                    c.Celular = celularEntry.Text;
                    c.Email = email;
                    c.Endereco = enderecoEntry.Text;
                    c.Numero = numeroEntry.Text;
                    c.Complemento = complementoEntry.Text;
                    c.Bairro = bairroEntry.Text;
                    c.Estado = (estadoPicker.SelectedItem as EstadoBrasil)?.Exibicao;
                    c.Cidade = cidadePicker.SelectedItem as string;
                    c.CEP = cepEntry.Text;
                }
            }
            else
            {
                var novo = new Cliente
                {
                    Id = ClienteRepository.Clientes.Any() ? ClienteRepository.Clientes.Max(x => x.Id) + 1 : 1,
                    Nome = nome,
                    CNPJ = cnpjEntry.Text,
                    CPF = cpfEntry.Text,
                    RG = rgEntry.Text,
                    Telefone = telefoneEntry.Text,
                    Celular = celularEntry.Text,
                    Email = email,
                    Endereco = enderecoEntry.Text,
                    Numero = numeroEntry.Text,
                    Complemento = complementoEntry.Text,
                    Bairro = bairroEntry.Text,
                    Estado = (estadoPicker.SelectedItem as EstadoBrasil)?.Exibicao,
                    Cidade = cidadePicker.SelectedItem as string,
                    CEP = cepEntry.Text
                };
                ClienteRepository.Add(novo);
            }

            // go back to list
            await Shell.Current.GoToAsync("..");
        }

        private async void Cancelar_Clicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        void PopulateForm(Cliente c)
        {
            nomeEntry.Text = c.Nome;
            cnpjEntry.Text = c.CNPJ;
            cpfEntry.Text = c.CPF;
            rgEntry.Text = c.RG;
            telefoneEntry.Text = c.Telefone;
            celularEntry.Text = c.Celular;
            emailEntry.Text = c.Email;
            enderecoEntry.Text = c.Endereco;
            numeroEntry.Text = c.Numero;
            complementoEntry.Text = c.Complemento;
            bairroEntry.Text = c.Bairro;
            var estado = estados.FirstOrDefault(item =>
                string.Equals(item.Exibicao, c.Estado, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(item.Nome, c.Estado, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(item.Sigla, c.Estado, StringComparison.OrdinalIgnoreCase));
            if (estado is not null)
            {
                estadoPicker.SelectedItem = estado;
                AtualizarCidades(estado, c.Cidade);
                cidadePicker.SelectedItem = c.Cidade;
            }
            else
            {
                estadoPicker.SelectedIndex = -1;
                cidadePicker.ItemsSource = null;
            }
            cepEntry.Text = c.CEP;
        }

        void ClearForm()
        {
            nomeEntry.Text = string.Empty;
            cnpjEntry.Text = string.Empty;
            cpfEntry.Text = string.Empty;
            rgEntry.Text = string.Empty;
            telefoneEntry.Text = string.Empty;
            celularEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            enderecoEntry.Text = string.Empty;
            numeroEntry.Text = string.Empty;
            complementoEntry.Text = string.Empty;
            bairroEntry.Text = string.Empty;
            estadoPicker.SelectedIndex = -1;
            cidadePicker.ItemsSource = null;
            cepEntry.Text = string.Empty;
        }

        private async Task CarregarLocalidadesAsync()
        {
            if (localidadesCarregadas)
                return;

            try
            {
                await using var stream = await FileSystem.OpenAppPackageFileAsync("estados-cidades.json");
                var localidades = await JsonSerializer.DeserializeAsync<List<EstadoBrasil>>(
                    stream,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                estados.Clear();
                if (localidades is not null)
                    estados.AddRange(localidades.OrderBy(item => item.Nome));

                estadoPicker.ItemsSource = estados;
                localidadesCarregadas = estados.Count > 0;
            }
            catch (Exception)
            {
                await DisplayAlertAsync("Erro", "Não foi possível carregar os estados e cidades.", "OK");
            }
        }

        private void AtualizarCidades(EstadoBrasil estado, string? cidadeSelecionada = null)
        {
            cidadePicker.ItemsSource = estado.Cidades;
            cidadePicker.SelectedItem = cidadeSelecionada;
        }
    }
}

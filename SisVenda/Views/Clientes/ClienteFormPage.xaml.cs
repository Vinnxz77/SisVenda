using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using SisVenda.Models;
using SisVenda.Services;

namespace SisVenda
{
    [QueryProperty(nameof(ClientId), "id")]
    public partial class ClienteFormPage : ContentPage
    {
        int clientId = 0;
        readonly Dictionary<string, List<string>> stateCities = new()
        {
            { "Estado Alpha", new List<string>{ "Alpha City 1","Alpha City 2","Alpha City 3", "Paracatu" } },
            { "Estado Beta", new List<string>{ "Beta City 1","Beta City 2","Beta City 3" } },
            { "Estado Gama", new List<string>{ "Gama City 1","Gama City 2","Gama City 3" } }
        };

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
            estadoPicker.ItemsSource = stateCities.Keys.ToList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

        private void EstadoPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (estadoPicker.SelectedIndex < 0) return;
            var estado = estadoPicker.SelectedItem as string;
            if (estado is null) return;
            cidadePicker.ItemsSource = stateCities[estado];
            cidadePicker.SelectedIndex = -1;
        }

        private async void Salvar_Clicked(object sender, EventArgs e)
        {
            var nome = nomeEntry.Text?.Trim();
            var email = emailEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                await DisplayAlert("Validação", "Nome é obrigatório.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Validação", "Email é obrigatório.", "OK");
                return;
            }

            if (clientId > 0)
            {
                var c = ClienteRepository.GetById(clientId);
                if (c != null)
                {
                    c.Nome = nome;
                    c.CNPJ = cnpjEntry.Text;
                    c.CPF = cpfEntry.Text;
                    c.RG = rgEntry.Text;
                    c.Telefone = telefoneEntry.Text;
                    c.Celular = celularEntry.Text;
                    c.Email = email;
                    c.Endereco = enderecoEntry.Text;
                    c.Numero = numeroEntry.Text;
                    c.Complemento = complementoEntry.Text;
                    c.Bairro = bairroEntry.Text;
                    c.Estado = estadoPicker.SelectedItem as string;
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
                    Estado = estadoPicker.SelectedItem as string,
                    Cidade = cidadePicker.SelectedItem as string,
                    CEP = cepEntry.Text
                };
                ClienteRepository.Add(novo);
            }

            // go back to list
            await Shell.Current.GoToAsync("..");
        }

        private async void Cancelar_Clicked(object sender, EventArgs e)
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
            if (c.Estado != null && stateCities.ContainsKey(c.Estado))
            {
                estadoPicker.SelectedItem = c.Estado;
                cidadePicker.ItemsSource = stateCities[c.Estado];
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
    }
}

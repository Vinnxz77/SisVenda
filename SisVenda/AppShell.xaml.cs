using Microsoft.Maui.Controls;

namespace SisVenda
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // register routes
            Routing.RegisterRoute(nameof(ClienteFormPage), typeof(ClienteFormPage));
            Routing.RegisterRoute(nameof(ProdutoFormPage), typeof(ProdutoFormPage));
        }
    }
}

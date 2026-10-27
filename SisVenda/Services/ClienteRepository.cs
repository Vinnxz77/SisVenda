using System.Collections.ObjectModel;
using System.Linq;
using SisVenda.Models;

namespace SisVenda.Services
{
    public static class ClienteRepository
    {
        public static ObservableCollection<Cliente> Clientes { get; } = new ObservableCollection<Cliente>();

        public static Cliente? GetById(int id) => Clientes.FirstOrDefault(c => c.Id == id);

        public static void Add(Cliente c)
        {
            Clientes.Add(c);
        }

        public static void Remove(Cliente c)
        {
            Clientes.Remove(c);
        }
    }
}

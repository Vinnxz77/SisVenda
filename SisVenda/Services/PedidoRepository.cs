using System.Collections.ObjectModel;
using System.Linq;
using SisVenda.Models;

namespace SisVenda.Services
{
    public static class PedidoRepository
    {
        public static ObservableCollection<Pedidos> Pedidos { get; } = new ObservableCollection<Pedidos>();

        public static Pedidos? GetById(int id) => Pedidos.FirstOrDefault(p => p.Id == id);

        public static void Add(Pedidos p)
        {
            Pedidos.Add(p);
        }

        public static void Remove(Pedidos p)
        {
            Pedidos.Remove(p);
        }
    }
}

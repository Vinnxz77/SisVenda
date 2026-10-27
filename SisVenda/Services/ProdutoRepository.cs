using System.Collections.ObjectModel;
using System.Linq;
using SisVenda.Models;

namespace SisVenda.Services
{
    public static class ProdutoRepository
    {
        public static ObservableCollection<Produto> Produtos { get; } = new ObservableCollection<Produto>();

        public static Produto? GetById(int id) => Produtos.FirstOrDefault(p => p.Id == id);

        public static void Add(Produto p)
        {
            Produtos.Add(p);
        }

        public static void Remove(Produto p)
        {
            Produtos.Remove(p);
        }
    }
}

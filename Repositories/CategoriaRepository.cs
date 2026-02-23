using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly VH_BurguerContext _context;

        public CategoriaRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Categoria> Listar()
        {
            return _context.Categoria.ToList();
        }

        public Categoria ObterPorId(int id)
        {
            Categoria categoria = _context.Categoria.FirstOrDefault(c => c.CategoriaID == id);

            return categoria;
        }

        public bool NomeExiste(string nome, int? categoriaIdAtual = null)
        {
            var consulta = _context.Categoria.AsQueryable();

            if (categoriaIdAtual.HasValue)
            {
                consulta = consulta.Where(categoria => categoria.CategoriaID != categoriaIdAtual.Value);
            }
            
            return consulta.Any(c => c.Nome == nome);

        }

        public void Adicionar(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();
        }

        public void Atualizar(Categoria categoria)
        {
            Categoria categoriaBanco = _context.Categoria.FirstOrDefault(c => c.CategoriaID == categoria.CategoriaID);

            if (categoriaBanco == null)
            {
                return;
            }

            categoriaBanco.Nome = categoria.Nome;

            _context.SaveChanges();

        }

            public void Remover (int id)
        {
            Categoria categoriaBanco = _context.Categoria.FirstOrDefault(c => c.CategoriaID == id);

            if (categoriaBanco == null)
            {
                return;
            }

            _context.Categoria.Remove(categoriaBanco);

            _context.SaveChanges();
        }
    }
}

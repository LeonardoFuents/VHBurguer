using VHBurguer.Domains;

namespace VHBurguer.Interfaces
{
    public interface ICategoriaRepository
    {
        List<Categoria> Listar();
        Categoria ObterPorId(int id);

        bool NomeExiste(string nome, int? categoriaIdAtual = null);

        void adicionar (Categoria categoria);
        void atualizar (Categoria categoria);

        void Remover(int id);
    }
}

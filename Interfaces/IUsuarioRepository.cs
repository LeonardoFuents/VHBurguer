using VHBurguer.Domains;

namespace VHBurguer.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();

        // Pode ser que não vena usuário na busca,
        // Entao colocamos "?"

        Usuario? ObterPorId(int id);

        Usuario? ObterPorEmail( string email);

        bool EmailExiste ( string email );

        void Adicionar(Usuario usuario);

        void Atualizar(Usuario usuario);

        void Remover(int id);


    }
}

using System.Security.Cryptography;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class UsuarioService
    {
        // Repository é o canal para acessar os dados.
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository) 
        
        {
            _repository = repository;
        }

        private static LerUsuarioDto LerDto(Usuario usuario)
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true
            };

            return lerUsuario;
        }

        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<LerUsuarioDto> usuariosDto = usuarios
               .Select(usuarioBanco => LerDto(usuarioBanco)) //SELECT QUE PERCORRE CADA USUÁRIO E LER DTO USUARIO
               .ToList(); // Devolve uma lista de dtos depois que foi criada
            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new DomainException("Email Inválido.");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if(string.IsNullOrEmpty(senha))// Garante que a senha nao está vazia
            {
                throw new DomainException("Senha é obrigatória.");
            }

            using var sha256 = SHA256.Create(); // Gera um hash SHA256 e devolve em byte[]
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario? usuario = _repository.ObterPorId(id);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe.");

            }

            return LerDto(usuario); // Se existe usuário , converte para DTO e devolve o usuário.
        }

        public LerUsuarioDto ObterPorEmail(string email)
        {
            Usuario? usuario = _repository.ObterPorEmail(email);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe.");

            }

            return LerDto(usuario); // Se existe usuário , converte para DTO e devolve o usuário.
        }
        


    }
}

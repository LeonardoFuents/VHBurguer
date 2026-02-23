using VHBurguer.Domains;
using VHBurguer.DTOs.CategoriaDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class CategoriaService 
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<LerCategoriaDto> Listar()
        {
            List<Categoria> categorias = _repository.Listar();

            List<LerCategoriaDto> categoriaDto = categorias.Select(categoria => new LerCategoriaDto { CategoriaId = categoria.CategoriaID, Nome = categoria.Nome }).ToList();
            
            return categoriaDto;

        }

        public LerCategoriaDto ObterPorId(int id)
        {
            Categoria categoria = _repository.ObterPorId(id);

            if(categoria == null)
            {
                throw new DomainException("Categoria não encontrada.");
            }

            LerCategoriaDto categoriaDto = new LerCategoriaDto
            {
                CategoriaId = categoria.CategoriaID,
                Nome = categoria.Nome,
            };

            return categoriaDto;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome é obrigatorio");
            }
        }

        public void Adicionar(CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Categoria ja existente.");

            }

            Categoria categoria = new Categoria
            {
                Nome = criarDto.Nome,

            };

            _repository.adicionar(categoria);

        }

        public void Atualizar(int id, CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco != null)
            {
                throw new DomainException("Categoria nao foi encontrada.");

            }

            if(_repository.NomeExiste(criarDto.Nome, categoriaIdAtual: id))
            {
                throw new DomainException("Ja existe outra categoria com este nome.");
            }

            categoriaBanco.Nome = criarDto.Nome;
            _repository.atualizar(categoriaBanco);
        }

        public void Remover(int id)
        {
            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null)
            {
                throw new DomainException("Categoria não encontrada.");

            }

            _repository.Remover(id);
        }


    }
}

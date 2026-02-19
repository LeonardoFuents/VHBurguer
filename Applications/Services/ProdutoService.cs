using VHBurguer.Applications.Conversoes;
using VHBurguer.Applications.Regras;
using VHBurguer.Domains;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }
    

    public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.Listar();

            List<LerProdutoDto> produtosDto = produtos.Select(ProdutoParaDto.ConverterParaDto).ToList();

            return produtosDto;
        }

        public LerProdutoDto ObterPorId(int id)
        {
            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        private static void ValidarCadastro(CriarProdutoDto produtoDto)
        {
            if (string.IsNullOrWhiteSpace(produtoDto.Nome))
            {
                throw new DomainException("Nome é obrigatório.");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que 0.");
            }

            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
            {
                throw new DomainException("Descrição é obrigatória.");
            } }


            public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if (imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada.");
            }
            return imagem;

        }

        public LerProdutoDto Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto);

            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto ja existente.");

            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = produtoDto.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioID = usuarioId
            };

            _repository.Adicionar(produto, produtoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        public LerProdutoDto Atualizar(int id, AtualizarProdutoDto produtoDto)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produtoBanco = _repository.ObterPorId(id);

            if (produtoBanco == null)
            {
                throw new DomainException("Produto nao encontrado.");

            }

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
            {
                throw new DomainException("Imagem é obrigatória.");
            }

            if (produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Produto deve ter ao menos uma categoria.");
            }

            produtoBanco.Nome = produtoDto.Nome;
            produtoBanco.Preco = produtoDto.Preco;
            produtoBanco.Descricao = produtoDto.Descricao;

            if (produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem);
            }

            if (produtoDto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produtoDto.StatusProduto.Value;
            }

            _repository.Atualizar(produtoBanco, produtoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produtoBanco);
        }

        public void Remover(int id)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(id);

            if (produto != null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            _repository.Remover(id);
        }
    }
}

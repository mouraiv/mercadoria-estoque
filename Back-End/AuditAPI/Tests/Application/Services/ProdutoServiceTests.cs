using Moq;
using AuditAPI.Application.Services;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;
using Xunit;

namespace AuditAPI.Tests{

    public class ProdutoServiceTests{
        private readonly Mock<IProdutoRepository> _mockProdutoRepository;
        private readonly Mock<IFileService> _mockFileService;
        private readonly ProdutoService _produtoService;
        private readonly Produto _produto;

        public ProdutoServiceTests(){

            _mockProdutoRepository = new Mock<IProdutoRepository>();
            _mockFileService = new Mock<IFileService>();

            _produtoService = new ProdutoService(_mockProdutoRepository.Object, _mockFileService.Object);

            _produto = new Produto{
                Id = 1,
                Nome = "Mochila Escolar",
                Preco = 89.90m,
                QuantidadeEstoque = 10,
                Imagem = null
            };
        }

        [Theory(DisplayName = "Atualizar estoque corretamente")]
        [InlineData(1, 10)]
        public async Task AtualizarEstoque_DeveAtualizarEstoqueCorretamente(int id_Produto, int quantidadeEstoque){

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(id_Produto)).ReturnsAsync(_produto);

            await _produtoService.AtualizarEstoque(id_Produto, quantidadeEstoque);

            Assert.Equal(20, _produto.QuantidadeEstoque);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(_produto), Times.Once);
        }

        [Theory(DisplayName = "Lançar exceção quando produto não existir")]
        [InlineData(1)]
        public async Task AtualizarEstoque_DeveLancarExcecaoQuandoProdutoNaoExistir(int id_Produto){

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(id_Produto)).ReturnsAsync((Produto)null!);
          
            var resultado = await _produtoService.ObterPorId(id_Produto);

            Assert.Null(resultado);
        }

        [Theory(DisplayName = "Lançar exceção quando a quantidade em estoque for negativa")]
        [InlineData(1, -10)]
        public async Task AtualizarEstoque_DeveLancarExcecaoQuandoQuantidadeForNegativa(int id_Produto, int quantidadeEstoque){

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(id_Produto)).ReturnsAsync(_produto);

            var excepion = await Assert.ThrowsAsync<ArgumentException>(() => 
            _produtoService.AtualizarEstoque(id_Produto, quantidadeEstoque));

            Assert.Equal("A quantidade não pode ser negativa", excepion.Message);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(It.IsAny<Produto>()), Times.Never);
        }
    }
}
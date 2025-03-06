using Moq;
using AuditAPI.Application.Services;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;
using Xunit;

namespace AuditAPI.Tests{

    public class ProdutoServiceTests{
        private readonly Mock<IProdutoRepository> _mockProdutoRepository;
        private readonly ProdutoService _produtoService;

        public ProdutoServiceTests(){
            _mockProdutoRepository = new Mock<IProdutoRepository>();
            _produtoService = new ProdutoService(_mockProdutoRepository.Object);
        }

        [Fact]
        public void AtualizarEstoque_DeveAtualizarEstoqueCorretamente(){
            // Arrange
            var produto = new Produto{
                Id = 1,
                Nome = "Mochila Escolar",
                Preco = 89.90m,
                QuantidadeEstoque = 10
            };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(1)).Returns(produto);

            // Act
            _produtoService.AtualizarEstoque(1, 10);

            // Assert
            Assert.Equal(20, produto.QuantidadeEstoque);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(produto), Times.Once);
        }

        [Fact]
        public void AtualizarEstoque_DeveLancarExcecaoQuandoProdutoNaoExistir(){
            // Arrange
            _mockProdutoRepository.Setup(repo => repo.ObterPorId(1)).Returns((Produto)null!);

            // Act
             var excepion = Assert.Throws<ArgumentException>(() => _produtoService.AtualizarEstoque(1, 10));

            // Assert
            Assert.Equal("Produto não encontrado", excepion.Message);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(It.IsAny<Produto>()), Times.Never);
        }

        [Fact]
        public void AtualizarEstoque_DeveLancarExcecaoQuandoQuantidadeForNegativa(){
            // Arrange
            var produto = new Produto{
                Id = 1,
                Nome = "Mochila Escolar",
                Preco = 89.90m,
                QuantidadeEstoque = 10
            };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(1)).Returns(produto);

            // Act
           var excepion = Assert.Throws<ArgumentException>(() => _produtoService.AtualizarEstoque(1, -10));

            // Assert
            Assert.Equal("A quantidade não pode ser negativa", excepion.Message);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(It.IsAny<Produto>()), Times.Never);
        }
    }
}
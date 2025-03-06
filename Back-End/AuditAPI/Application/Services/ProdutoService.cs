using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;

namespace AuditAPI.Application.Services{

    public class ProdutoService{

        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository){
            _produtoRepository = produtoRepository;

        }

        public Produto ObterPorId(int id){
            return _produtoRepository.ObterPorId(id);
        }

        public IEnumerable<Produto> ObterTodos(){
            return _produtoRepository.ObterTodos();
        }

        public void Adicionar(Produto produto){
            _produtoRepository.Adicionar(produto);
        }

        public void Atualizar(Produto produto){
            _produtoRepository.Atualizar(produto);
        }

        public void Remover(int id){
            _produtoRepository.Remover(id);
        }

        public void AtualizarEstoque(int id, int quantidade){
            
            if (quantidade < 0){ throw new ArgumentException("A quantidade não pode ser negativa"); }

            var produto = _produtoRepository.ObterPorId(id);
            
            if(produto == null){ throw new ArgumentException("Produto não encontrado"); }

            produto.AtualizarEstoque(quantidade);
            _produtoRepository.Atualizar(produto);
        }
    }
}

using System.Linq;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Exceptions;
using AuditAPI.Domain.Interfaces;

namespace AuditAPI.Application.Services{

    public class ProdutoService{

        private readonly IProdutoRepository _produtoRepository;
        private readonly IFileService _fileService;
        public ProdutoService(IProdutoRepository produtoRepository, IFileService fileService){
            _produtoRepository = produtoRepository;
            _fileService = fileService;
        }

        public async Task<Produto?> ObterPorId(int id){

            var resultado = await _produtoRepository.ObterPorId(id);

            if(resultado == null) throw new DomainException("Produto não encontrado.");

            return resultado;
        }

        public async Task<IEnumerable<Produto?>> ObterTodos(){
            
            var resultado = await _produtoRepository.ObterTodos();

            if(resultado == null || !resultado.Any()) throw new DomainException("Nenhuma resultado.");

            return resultado;
        }

        public async Task<string?> ExistsByName(string name){
            
            return await _produtoRepository.ExistsByName(name);
        }

        private async Task ValidarCampos(Produto produto){

            if(String.IsNullOrEmpty(produto.Nome)){

                throw new DomainException("O Campo Nome é obrigatório.");

            }else if(await ExistsByName(produto.Nome) != null){

                throw new DomainException($" O nome '{produto.Nome}' já existe.");

            }else if(produto.Preco < 0m){

                throw new DomainException("O Preço do produto não pode ser negativo");

            }else if (produto.QuantidadeEstoque < 0){

                throw new DomainException("A quantidade em estoque não pode ser negativo.");

            }
        }

        public async Task Adicionar(Produto produto, Stream streamFile, string fileName ){

            //Validar e salvar imagem
            _fileService.validateFile(streamFile, fileName);
            string pathFile = await _fileService.SaveFileAsync(streamFile, fileName);

            //Atribuir o path da imagem ao produto
            produto.Imagem = pathFile;

            //Validar campos
            await ValidarCampos(produto);

            //Adicionar produto ao repositorio
            await _produtoRepository.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto){
            
            //Validar campos
            await ValidarCampos(produto);

            //Atualizar produto
            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(int id){            
            //Excluir produto
            await _produtoRepository.Remover(id);
        }

        public async Task AtualizarEstoque(int id, int quantidade){
            
            if (quantidade < 0){ throw new DomainException("A quantidade não pode ser negativa"); }

            var produto = await _produtoRepository.ObterPorId(id);
            
            if(produto == null){ throw new DomainException("Produto não encontrado"); }

            produto.AtualizarEstoque(quantidade);
            await _produtoRepository.Atualizar(produto);
        }
    }
}

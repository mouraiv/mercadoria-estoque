using System.Threading.Tasks;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;

namespace AuditAPI.Application.Services{

    public class ProdutoService{

        private readonly IProdutoRepository _produtoRepository;
        private readonly IFileService _fileService;
        public ProdutoService(IProdutoRepository produtoRepository, IFileService fileService){
            _produtoRepository = produtoRepository;
            _fileService = fileService;
        }

        public async Task<Produto> ObterPorId(int id){
            return await _produtoRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos(){
            return await _produtoRepository.ObterTodos();
        }

        public async Task Adicionar(Produto produto, Stream streamFile, string fileName ){
            //Validar e salvar imagem
            _fileService.validateFile(streamFile, fileName);
            string pathFile = await _fileService.SaveFileAsync(streamFile, fileName);

            //Atribuir o path da imagem ao produto
            produto.Imagem = pathFile;

            //Adicionar produto ao repositorio
            await _produtoRepository.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto){
            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(int id){
            await _produtoRepository.Remover(id);
        }

        public async Task AtualizarEstoque(int id, int quantidade){
            
            if (quantidade < 0){ throw new ArgumentException("A quantidade não pode ser negativa"); }

            var produto = await _produtoRepository.ObterPorId(id);
            
            if(produto == null){ throw new ArgumentException("Produto não encontrado"); }

            produto.AtualizarEstoque(quantidade);
            await _produtoRepository.Atualizar(produto);
        }
    }
}

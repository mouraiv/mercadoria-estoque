using Microsoft.AspNetCore.Mvc;
using AuditAPI.API.DTOs;
using AuditAPI.Application.Services;
using AuditAPI.Domain.Entities;

namespace AuditAPI.API.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase {

        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService){
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> ObterTodos(){
            try{
                var produtos = await _produtoService.ObterTodos();

                var response = produtos.Select(produto => new ProdutoResponseDTO{
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    Imagem = produto.Imagem
                });
                return Ok(response);

            }catch(Exception ex){
                return BadRequest("Erro ao listar produtos :" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> ObterPorId(int id){
            try{
                var produto = await _produtoService.ObterPorId(id);
                if(produto == null){
                    return NotFound();
                }
                var response = new ProdutoResponseDTO{
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    Imagem = produto.Imagem
                };
                return Ok(response);

            }catch(Exception ex){
                return BadRequest("Erro ao carregar produtos :" + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar(IFormFile file, [FromQuery] ProdutoDTO produtoDTO){
            try{
                using(var stream = file.OpenReadStream()){

                    var produto = new Produto{
                        Nome = produtoDTO.Nome,
                        Preco = produtoDTO.Preco,
                        QuantidadeEstoque = produtoDTO.QuantidadeEstoque,
                    };

                    await _produtoService.Adicionar(produto, stream, file.FileName);
                    return CreatedAtAction(nameof(ObterPorId), new {id = produto.Id}, produto);

                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, [FromQuery] ProdutoDTO produtoDTO){
            try{

                var produto = new Produto{
                    Id = id,
                    Nome = produtoDTO.Nome,
                    Preco = produtoDTO.Preco,
                    QuantidadeEstoque = produtoDTO.QuantidadeEstoque,
                };

                await _produtoService.Atualizar(produto);
                return NoContent();

            }catch(Exception ex){
                return BadRequest("Erro ao atualizar produtos :" + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id){
            try{
                await _produtoService.Remover(id);
                return NoContent();
            }catch(KeyNotFoundException ex){
                return NotFound("Erro na chave do produto: " + ex.Message);
            }catch(ArgumentException ex){
                return BadRequest("Erro ao deletar produto: " + ex.Message);
            }
        }

    }
}
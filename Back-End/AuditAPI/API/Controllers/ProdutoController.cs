using Microsoft.AspNetCore.Mvc;
using AuditAPI.API.DTOs;
using AuditAPI.Application.Services;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Exceptions;
using AuditAPI.Infrastructure.Exceptions;

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

                var response = produtos.Select(produto => produto != null ? new ProdutoResponseDTO{
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    Imagem = produto.Imagem
                } : null).Where(dto => dto != null);

                return Ok(response);

            }catch(DomainException ex){
                return BadRequest(ex.Message);

            }catch(RepositoryException ex){
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> ObterPorId(int id){
            try{
                var produto = await _produtoService.ObterPorId(id);
                
                var response = produto != null ? new ProdutoResponseDTO{
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    Imagem = produto.Imagem
                } : null;
                return Ok(response);

            }catch(DomainException ex){
                return BadRequest(ex.Message);

            }catch(RepositoryException ex){
                return StatusCode(500, ex.Message);
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
            catch(DomainException ex){
                return BadRequest(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(RepositoryException ex){
                return StatusCode(500, ex.Message);
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

            }catch(DomainException ex){
                return BadRequest(ex.Message);

            }catch(RepositoryException ex){
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id){
            try{
                await _produtoService.Remover(id);
                return NoContent();

            }catch(ArgumentException ex){
                return BadRequest($"Erro ao excluir o produto: {ex.Message}");

            }catch(RepositoryException ex){
                return StatusCode(500, ex.Message);
            }
        }

    }
}
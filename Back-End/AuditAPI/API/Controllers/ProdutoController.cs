using Microsoft.AspNetCore.Mvc;
using AuditAPI.API.DTOs;
using AuditAPI.Application.Services;
using AuditAPI.Domain.Entities;
using System.Linq;

namespace AuditAPI.API.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase {

        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService){
            _produtoService = produtoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoResponseDTO>> ObterTodos(){
            var produtos = _produtoService.ObterTodos();
            var response = produtos.Select(p => new ProdutoResponseDTO{
                Id = p.Id,
                Nome = p.Nome,
                Preco = p.Preco
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<ProdutoResponseDTO> ObterPorId(int id){
            var produto = _produtoService.ObterPorId(id);
            if(produto == null){
                return NotFound();
            }
            var response = new ProdutoResponseDTO{
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco
            };
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Adicionar([FromBody] ProdutoDTO produtoDTO){
            var produto = new Produto{
                Nome = produtoDTO.Nome,
                Preco = produtoDTO.Preco,
                QuantidadeEstoque = produtoDTO.QuantidadeEstoque
            };

            _produtoService.Adicionar(produto);
            return CreatedAtAction(nameof(ObterPorId), new {id = produto.Id}, produto);

        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(int id, [FromBody] ProdutoDTO produtoDTO){
            var produto = new Produto{
                Id = id,
                Nome = produtoDTO.Nome,
                Preco = produtoDTO.Preco,
                QuantidadeEstoque = produtoDTO.QuantidadeEstoque
            };

            _produtoService.Atualizar(produto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Remover(int id){
            try{
                _produtoService.Remover(id);
                return NoContent();
            }catch(KeyNotFoundException ex){
                return NotFound(ex.Message);
            }catch(ArgumentException ex){
                return BadRequest(ex.Message);
            }
        }

    }
}
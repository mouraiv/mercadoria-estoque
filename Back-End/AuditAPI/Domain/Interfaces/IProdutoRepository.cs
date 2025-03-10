using System.Collections.Generic;
using AuditAPI.Domain.Entities;

namespace AuditAPI.Domain.Interfaces{

    public interface IProdutoRepository{
        Task<Produto?> ObterPorId(int id);
        Task<string?> ExistsByName(string nome);
        Task<IEnumerable<Produto?>> ObterTodos();
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(int id);
    }
}
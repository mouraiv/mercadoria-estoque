using System.Collections.Generic;
using AuditAPI.Domain.Entities;

namespace AuditAPI.Domain.Interfaces{

    public interface IProdutoRepository{
        Produto ObterPorId(int id);
        IEnumerable<Produto> ObterTodos();
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        void Remover(int id);
    }
}
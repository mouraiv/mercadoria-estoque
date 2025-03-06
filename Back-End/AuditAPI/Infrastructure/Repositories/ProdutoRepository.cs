using Dapper;
using MySql.Data.MySqlClient;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;

namespace AuditAPI.Infrastructure.Repositories{

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _connectionString;
        public ProdutoRepository(string connectionString){
            _connectionString = connectionString;
        }

        private MySqlConnection GetConnection(){
            return new MySqlConnection(_connectionString);
        }

        public void Adicionar(Produto produto)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute("INSERT INTO Produto (Nome, Preco, QuantidadeEstoque) VALUES (@Nome, @Preco, @QuantidadeEstoque)", produto);
        }

        public void Atualizar(Produto produto)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute("UPDATE Produto SET Nome = @Nome, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id", produto);
        }

        public Produto ObterPorId(int id)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.QueryFirstOrDefault<Produto>("SELECT * FROM Produto WHERE Id = @Id", new {Id = id}) ?? new Produto();
        }

        public IEnumerable<Produto> ObterTodos()
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<Produto>("SELECT * FROM Produto");
        }

        public void Remover(int id)
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute("DELETE FROM Produto WHERE Id = @Id", new {Id = id});
        }
    }
}
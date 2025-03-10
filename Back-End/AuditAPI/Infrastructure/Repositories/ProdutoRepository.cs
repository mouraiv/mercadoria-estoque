using Dapper;
using MySql.Data.MySqlClient;
using AuditAPI.Domain.Entities;
using AuditAPI.Domain.Interfaces;
using AuditAPI.Infrastructure.Exceptions;

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

        public async Task Adicionar(Produto produto)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("INSERT INTO Produto (Nome, Preco, QuantidadeEstoque) VALUES (@Nome, @Preco, @QuantidadeEstoque)", produto);

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao adicionar o produto", ex);
            }
            
        }

        public async Task Atualizar(Produto produto)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("UPDATE Produto SET Nome = @Nome, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id", produto);

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao atualizar o produto", ex);
            }
        }

        public async Task<Produto?> ObterPorId(int id)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                var produto = await connection.QueryFirstOrDefaultAsync<Produto>("SELECT * FROM Produto WHERE Id = @Id", new {Id = id});

                return produto;

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao carregar o produto",ex);
            }
            
        }

         public async Task<string?> ExistsByName(string nome)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                var produto = await connection.QueryFirstOrDefaultAsync<Produto>("SELECT Nome FROM Produto WHERE Nome = @nome", new {Nome = nome});

                return produto?.Nome;

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao verificar o nome do produto",ex);
            }
            
        }

        public async Task<IEnumerable<Produto?>> ObterTodos()
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                var produto =  await connection.QueryAsync<Produto>("SELECT * FROM Produto");

                return produto;

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao listar produto", ex);
            }
        }

        public async Task Remover(int id)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("DELETE FROM Produto WHERE Id = @Id", new {Id = id});

            }catch(MySqlException ex){
                throw new RepositoryException("Erro ao deletar produto", ex);
            }
            
        }
    }
}
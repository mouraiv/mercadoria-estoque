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

        public async Task Adicionar(Produto produto)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("INSERT INTO Produto (Nome, Preco, QuantidadeEstoque) VALUES (@Nome, @Preco, @QuantidadeEstoque)", produto);

            }catch(Exception ex){
                throw new ApplicationException("Erro ao adicionar o produto", ex);
            }
            
        }

        public async Task Atualizar(Produto produto)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("UPDATE Produto SET Nome = @Nome, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id", produto);

            }catch(Exception ex){
                throw new ApplicationException("Erro ao atualizar o produto", ex);
            }
        }

        public async Task<Produto> ObterPorId(int id)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<Produto>("SELECT * FROM Produto WHERE Id = @Id", new {Id = id}) ?? new Produto();

            }catch(Exception ex){
                throw new ApplicationException("Erro ao carregar produto", ex);
            }
            
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                return await connection.QueryAsync<Produto>("SELECT * FROM Produto");

            }catch(Exception ex){
                throw new ApplicationException("Erro ao listar produto", ex);
            }
        }

        public async Task Remover(int id)
        {
            try{
                using var connection = GetConnection();
                await connection.OpenAsync();
                await connection.ExecuteAsync("DELETE FROM Produto WHERE Id = @Id", new {Id = id});

            }catch(Exception ex){
                throw new ApplicationException("Erro ao deletar produto", ex);
            }
            
        }
    }
}
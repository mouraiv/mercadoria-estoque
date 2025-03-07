namespace AuditAPI.API.DTOs{

    public class ProdutoResponseDTO{
        public int Id {get; set;}
        public string? Nome {get; set;}
        public decimal Preco {get; set;}
        public int QuantidadeEstoque {get; set;}
        public string? Imagem {get; set;}
    }
}

namespace AuditAPI.Domain.Entities{

    public class Produto{
        public int Id {get; set;}
        public string? Nome {get; set;}
        public decimal Preco {get; set;}
        public int QuantidadeEstoque {get; set;}
        public string? Imagem {get; set;}

        public void AtualizarEstoque(int quantidade){
            if(quantidade < 0){ throw new ArgumentException("Quantidade não pode ser negativa."); }
            QuantidadeEstoque += quantidade;
        }    
    }
}
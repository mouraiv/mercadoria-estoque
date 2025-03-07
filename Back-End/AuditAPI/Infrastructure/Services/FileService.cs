
using AuditAPI.Domain.Interfaces;

namespace AuditAPI.Infrastructure.Services{

    public class FileService : IFileService
    {
        public async Task<string> SaveFileAsync(Stream file, string fileName)
        {
           //Caminho onde o arquivo será salvo 
           string folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

           //Verificar se o caminho (pasta) existe, se não existir sera criado
           if(!Directory.Exists(folder)){
                Directory.CreateDirectory(folder);
           }

           //Atribuir um nome unico ao arquivo
           string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

           //Criar caminnho absoluto do arquivo
           string filePath = Path.Combine(folder, uniqueFileName);

           //Salvar arquivo
           using(var stream = new FileStream(filePath, FileMode.Create)){
                await file.CopyToAsync(stream);
           }

           //Retornar o caminho do arquivo
           return Path.GetFullPath(filePath);
        }

        public void validateFile(Stream file, string fileName)
        {
            //Verificar se o arquivo foi recebido
            if(file == null || file.Length == 0){
                throw new ArgumentException("Nenhuma imagem foi enviada.");
            }

            //Validar o tamanho maximo do arquivo 5MB
            if(file.Length > 5 * 1024 * 1024){
                throw new ArgumentException("A imagem deve ter no máximo 5MB");
            }

            //Lista de extensões permitidas
            var extensoesPermitidas = new[] {".jpg", ".jpeg", ".png", ".jfif"};

            //Capturar extensão do arquivo
            var extensao = Path.GetExtension(fileName).ToLower();
            
            //validar extensão permitida
            if(!extensoesPermitidas.Contains(extensao)){
                throw new ArgumentException("A imagem deve ser no formato JPG, JPEG ou PNG.");
            }
        }
    }
}
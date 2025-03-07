namespace AuditAPI.Domain.Interfaces{

    public interface IFileService{
        Task<string> SaveFileAsync(Stream file, string fileName);
        void validateFile(Stream file, string fileName);
    }
}
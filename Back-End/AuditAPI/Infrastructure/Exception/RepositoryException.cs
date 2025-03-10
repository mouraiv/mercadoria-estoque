using System;

namespace AuditAPI.Infrastructure.Exceptions{

    public class RepositoryException : Exception{

        public RepositoryException(string message) : base(message) {}
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException) {}
    }
}
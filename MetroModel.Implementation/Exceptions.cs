using System;

namespace MetroModel
{
    internal sealed class UnexpectedFileFormatException : MetroException
    {
        public UnexpectedFileFormatException(string message, Exception innerException = null) : base(message, innerException) { }
    }

    internal sealed class FileLoadingException : MetroException
    {
        public FileLoadingException(string message, Exception innerException) : base(message, innerException) { }
    }

    internal sealed class FileSavingException : MetroException
    {
        public FileSavingException(string message, Exception innerException) : base(message, innerException) { }
    }

    internal sealed class GraphServiceErrorWrapperException : MetroException
    {
        public GraphServiceErrorWrapperException(string message) : base(message, null) { }
    }
}

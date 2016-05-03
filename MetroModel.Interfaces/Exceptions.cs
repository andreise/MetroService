using System;

namespace MetroModel
{
    public abstract class MetroException : Exception
    {
        public MetroException(string message, Exception innerException) : base(message, innerException) { }
    }
}

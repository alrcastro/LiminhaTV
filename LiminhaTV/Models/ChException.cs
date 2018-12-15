using System;

namespace LiminhaTV.Models
{
    internal class ChException : Exception
    {
        public ChException()
        {
        }

        public ChException(string message) : base(message)
        {
        }

        public ChException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
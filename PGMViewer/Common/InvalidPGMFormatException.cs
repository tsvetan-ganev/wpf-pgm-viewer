using System;

namespace PGMViewer.Common
{
    public class InvalidPGMFormatException : Exception
    {
        public InvalidPGMFormatException(string message)
            : base(message){}
    }
}

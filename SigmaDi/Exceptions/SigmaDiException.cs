using System;

namespace SigmaDi.Exceptions
{
    public class SigmaDiException : Exception
    {
        public SigmaDiException(string message) : base(message)
        {
        }
    }
}

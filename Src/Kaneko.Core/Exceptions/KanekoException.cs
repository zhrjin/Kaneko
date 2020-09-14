using System;
namespace Kaneko.Core.Exceptions
{
    public class KanekoException : Exception
    {
        public KanekoException(string message) : base(message) { }
    }
}

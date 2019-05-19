using System;

namespace DAL
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        { }
        public NotFoundException(string msg) : base(msg)
        { }
        public NotFoundException(string msg, Exception innerException)
            : base(msg, innerException)
        { }
    }
}

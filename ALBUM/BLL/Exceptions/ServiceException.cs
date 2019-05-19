using System;

namespace BLL
{
    public class ServiceException: Exception
    {
        public ExceptionType Type { get; set; }
        public string ExceptionValue { get; set; }
        
        public ServiceException(ExceptionType type = ExceptionType.UndefinedException)
            => Type = type;

        public ServiceException(ExceptionType type, string msg)
            : base(msg)
            => Type = type;

        public ServiceException(string msg)
            : base(msg)
            => Type = ExceptionType.UndefinedException;

        public ServiceException(ExceptionType type, string msg, Exception innerException)
            : base(msg, innerException)
        => Type = type;

        public ServiceException(string msg, Exception innerException)
            : base(msg, innerException)
        => Type = ExceptionType.UndefinedException;
    }
}

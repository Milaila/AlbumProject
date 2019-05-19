using System;

namespace BLL
{
    public enum ExceptionType
    {
        UndefinedException,
        NullException,
        NotFoundException,
        InvalidFieldException,
        InvalidDate,
        UniqueException,
        SameException,
        FileExtensionException,
        EmptyStringException,
        ForeignKeyException
    }
}

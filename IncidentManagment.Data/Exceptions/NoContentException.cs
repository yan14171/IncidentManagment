using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Data.Exceptions;
public class NoContentException : ArgumentException
{
    public NoContentException()
    {
    }

    public NoContentException(string? message) : base(message)
    {
    }

    public NoContentException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public NoContentException(string? message, string? paramName) : base(message, paramName)
    {
    }

    public NoContentException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
    {
    }

    protected NoContentException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

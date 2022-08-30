using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Data.Exceptions;
public class ValueNotFoundException : ArgumentException
{
    public ValueNotFoundException()
    {
    }

    public ValueNotFoundException(string? message) : base(message)
    {
    }

    public ValueNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ValueNotFoundException(string? message, string? paramName) : base(message, paramName)
    {
    }

    public ValueNotFoundException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
    {
    }
}

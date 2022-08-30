using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagment.Data.Exceptions;
public class UnprocessableException : InvalidOperationException
{
    public UnprocessableException()
    {
    }

    public UnprocessableException(string? message) : base(message)
    {
    }

    public UnprocessableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

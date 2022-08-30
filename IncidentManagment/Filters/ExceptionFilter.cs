using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IncidentManagment.Data.Exceptions;

namespace IncidentManagment.Filters;

public class ExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exceptionMessages = new List<string>();
        var resultBody = new ObjectResult("")
        {
            ContentTypes = { "application/json" },
        };

        try { context?.ExceptionDispatchInfo?.Throw(); }
        catch (ValueNotFoundException)
        {
            resultBody.StatusCode = StatusCodes.Status404NotFound;
            exceptionMessages.Add("Value couldn't have been found");
        }
        catch (NoContentException)
        {
            exceptionMessages.Add("No content with given identification could be found");
            context.Result = new NoContentResult();
            return;
        }
        catch (Exception)
        {
            resultBody.StatusCode = StatusCodes.Status403Forbidden;
            exceptionMessages.Add("Unknown error");
        }

        exceptionMessages.AddRange(context.Exception.GetExceptionMessages());
        resultBody.Value = new { IsSuccess = false, Messages = exceptionMessages, Token = "" };
        context.Result = resultBody;
    }
}

static class ExceptionExtensions
{
    public static List<string> GetExceptionMessages(this Exception e)
    {
        if (e == null) return new List<string> { string.Empty };

        List<string> msgs = new List<string> { e.Message };
        if (e.InnerException != null)
            msgs.AddRange(GetExceptionMessages(e.InnerException));
        return msgs;
    }
}


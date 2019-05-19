using BLL;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebAPI
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ServiceException ex = context.Exception as ServiceException;
            if (ex == null)
                return;
            switch (ex.Type)
            {
                case ExceptionType.NotFoundException:
                    context.Response = CreateResponse(ex.Message, HttpStatusCode.NotFound);
                    break;
                case ExceptionType.FileExtensionException:
                    context.Response = CreateResponse(ex.Message, HttpStatusCode.UnsupportedMediaType,
                        ex.ExceptionValue);
                    break;
                case ExceptionType.ForeignKeyException:
                    context.Response = CreateResponse(ex.Message, HttpStatusCode.NotFound);
                    break;
                case ExceptionType.InvalidDate:
                    context.Response = CreateResponse($"Date must be later than 1753 year",
                        HttpStatusCode.BadRequest, ex.ExceptionValue);
                    break;
                default:
                    context.Response = CreateResponse(ex.Message, HttpStatusCode.BadRequest);
                    break;
            }
        }

        private HttpResponseMessage CreateResponse
            (string message, HttpStatusCode statusCode, string value = null)
        {
            message = "Exception: " + message;
            if (value != null)
                message += $" [value: {value}]";
            HttpResponseMessage response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(message);
            return response;
        }
    }
}
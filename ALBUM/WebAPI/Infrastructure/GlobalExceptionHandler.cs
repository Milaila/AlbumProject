using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebAPI
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Oops... We have minor problems. Try again later."),
                ReasonPhrase = "Exception"
            };
            context.Result = new ErrorMessageResult(result);
        }
    }

    public class ErrorMessageResult : IHttpActionResult
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public ErrorMessageResult(HttpResponseMessage httpResponseMessage)
            => _httpResponseMessage = httpResponseMessage;

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            => Task.FromResult(_httpResponseMessage);
    }
}
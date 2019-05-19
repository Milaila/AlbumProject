using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Threading;

namespace WebAPI
{
    public class HttpActionResult: IHttpActionResult
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _message;
        private readonly HttpRequestMessage _request;

        public HttpActionResult(HttpStatusCode statusCode, string message = "", 
            HttpRequestMessage request = null)
        {
            _statusCode = statusCode;
            _message = message;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (_request != null)
            {
                response = _request.CreateResponse(_statusCode, _message);
                response.Headers.Date = DateTime.Now;
            }
            else
            {
                response = new HttpResponseMessage(_statusCode)
                {
                    Content = new StringContent(_message)
                };
            }
            return Task.FromResult(response);
        }
    }
}
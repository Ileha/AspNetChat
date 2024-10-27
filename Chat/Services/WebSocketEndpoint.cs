using System.Net;
using Microsoft.AspNetCore.Http;

namespace Chat.Services
{
	public class WebSocketEndpoint
    {
        private readonly RequestDelegate _next;

        public WebSocketEndpoint(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) 
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

                return;
            }

            await _next.Invoke(context);
        }
    }
}

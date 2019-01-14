using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace AtmosphereCommon.Middleware.SecurityHeaders.Hsts
{
    public class HstsMiddleware
    {
        //The HSTS Policy is communicated by the server to the user agent via an HTTPS response header field named "Strict-Transport-Security"
        private const string HeaderName = "Strict-Transport-Security";

        private readonly RequestDelegate _next;
        private readonly string _headerValue;

        public HstsMiddleware(RequestDelegate next /*,IOptions<HstsOptions> options ... for future use if needed to extend functionality*/)
        {
            _next = next;
            _headerValue = options.Value.BuildHeaderValue();
        }

        public async Task Invoke(HttpContext context)
        {
            //HSTS can only be applied to secure requests according to spec
            //there really is no point adding it to insecure ones since MiTM can just strip the header

            //Checks to see if header already exists, if not, it adds it.
            if (context.Request.IsHttps && !ContainsHstsHeader(context.Response))
            {
                context.Response.Headers.Add(HeaderName, _headerValue);
            }
            await _next(context);
        }

        private bool ContainsHstsHeader(HttpResponse response)
        {
            //Ordinal looks at raw bytes that represent each character.
            return response.Headers.Any(h => h.Key.Equals(HeaderName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AtmosphereCommon.Middleware.SecurityHeaders.Hsts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace Class_and_Field
{
    public class HstsMiddlewareTests
    {
        //Adds hsts header if not included. Doesn't add if hsts header is included
        public async Task hstsScreening()
        {
            //Mock request junk....
            RequestDelegate mockNext = (HttpContext ctx) =>
            {
                return Task.CompletedTask;
            };
            var options = Options.Create(new HstsOptions()
            {
                Duration = TimeSpan.FromHours(1)
            });
            var sut = new HstsMiddleware(mockNext, options); //He was using unit testing. SUT - System Under Test
            var mockContext = new DefaultHttpContext();
            mockContext.Request.Scheme = "https";
            mockContext.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");

            //Represents middleware execution
            await sut.Invoke(mockContext);

            //Invoke throws System.ArgumentException if it tries to add the header again
        }
    }
}

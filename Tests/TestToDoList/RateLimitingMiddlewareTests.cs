using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using WebAPITodoList.Middlewares;
using WebAPITodoList.Settings;
using Xunit;

namespace TestToDoList;

public class RateLimitingMiddlewareTests
{
    private TestServer CreateServer(int limit, int windowSeconds)
    {
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                // Injection della cache in memoria
                services.AddMemoryCache();

                // Injection dei parametri di test di rate limiting
                services.Configure<RateLimitSettings>(opts =>
                {
                    opts.LIMIT = limit;
                    opts.PERIOD = windowSeconds;
                });
            })
            .Configure(app =>
            {
                app.UseMiddleware<RateLimitingMiddleware>();

                // Endpoint simplice per il test
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("OK");
                });
            });

        return new TestServer(builder);
    }
    [Fact]
    public async Task AllowsRequestsWithinLimit()
    {
        // Arrange
        var server = CreateServer(limit: 5, windowSeconds: 60);
        var client = server.CreateClient();

        // Act + Assert
        for (int i = 0; i < 5; i++)
        {
            var response = await client.GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    [Fact]
    public async Task RejectsRequestsExceedingLimit()
    {
        // Arrange
        var server = CreateServer(limit: 3, windowSeconds: 60);
        var client = server.CreateClient();

        // Act
        for (int i = 0; i < 3; i++)
        {
            var response = await client.GetAsync("/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        var exceeded = await client.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.TooManyRequests, exceeded.StatusCode);
    }
}

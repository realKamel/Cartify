namespace Cartify.Web.WebAppHelpers;

public class UseEndpointChecker : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
        await CheckEndpointAsync(context);
    }

    private static async Task CheckEndpointAsync(HttpContext context)
    {
        if (context.Response.StatusCode == 404)
        {
            var response = new
            {
                status = "404",
                title = $"{context.Request.Method} {context.Request.Path} is not Found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
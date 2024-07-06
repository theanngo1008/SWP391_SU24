using JewelryProductionOrder.API.Middlewares;

namespace JewelryProductionOrder.API.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

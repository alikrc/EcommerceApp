﻿using Microsoft.AspNetCore.Builder;
using Services.Basket.API.Infrastructure.Middlewares;
using System;

namespace Services.Basket.API.Infrastructure.Exceptions
{
    public static class FailingMiddlewareAppBuilderExtensions
    {
        public static IApplicationBuilder UseFailingMiddleware(this IApplicationBuilder builder)
        {
            return UseFailingMiddleware(builder, null);
        }
        public static IApplicationBuilder UseFailingMiddleware(this IApplicationBuilder builder, Action<FailingOptions> action)
        {
            var options = new FailingOptions();
            action?.Invoke(options);
            builder.UseMiddleware<FailingMiddleware>(options);
            return builder;
        }
    }
}

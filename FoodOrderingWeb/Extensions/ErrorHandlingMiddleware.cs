﻿using System.IO;

namespace FoodOrderingWeb.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FoodNotFoundException ex)
            {
                // Log the exception if necessary
                // Redirect to the NotFound view with the error message
                context.Response.Redirect("/Home/NotFound?errorMessage=" + ex.Message);
            }
        }
    }
}

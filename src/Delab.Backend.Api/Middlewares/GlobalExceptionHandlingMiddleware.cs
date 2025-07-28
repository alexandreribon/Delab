using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;

namespace Delab.Backend.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var stackTrace = exception.StackTrace ?? "";

        var errors = new List<string>();

        switch (exception)
        {
            case SqlException:
                errors.Add("Erro ao acessar/processar dados no banco de dados");
                break;
            default:
                errors.Add("Ocorreu um erro inesperado");
                break;
        }

        var errorDictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);
        errorDictionary.Add("Erros", errors.ToArray());        

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(errorDictionary);
    }
}

using SrvIdo;
using System;
using System.Reflection.Metadata.Ecma335;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GetErro _getErro;

    public ExceptionMiddleware(RequestDelegate next, GetErro getErro)
    {
        _next = next;
        _getErro = getErro;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _getErro.LogErro(ex);
            throw;
        }
    }
}
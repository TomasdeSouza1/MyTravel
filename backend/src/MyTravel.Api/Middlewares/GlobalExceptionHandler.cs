using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MyTravel.Api.Middlewares;

 public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler                  
    {                                                                                                                
        public async ValueTask<bool> TryHandleAsync(                                                                 
            HttpContext httpContext,                                                                                 
            Exception exception,                                                                                     
            CancellationToken cancellationToken)                                                                     
        {                                                                                                            
            logger.LogError(exception, "Excepción no controlada capturada: {Message}", exception.Message);           
                                                                                                                     
            var problemDetails = new ProblemDetails                                                                  
            {                                                                                                        
                Status = StatusCodes.Status500InternalServerError,                                                   
                Title = "Error interno del servidor",                                                                
                Detail = "Ha ocurrido un error inesperado al procesar la solicitud.",                                
                Type = "https://tools.ietf.org/html/rfc7807",                                                        
                Instance = httpContext.Request.Path                                                                  
            };                                                                                                       
                                                                                                                     
            httpContext.Response.StatusCode = problemDetails.Status.Value;                                           
            httpContext.Response.ContentType = "application/problem+json";                                           
                                                                                                                     
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);                          
            return true;                                                
        }                                                                                                            
    }  

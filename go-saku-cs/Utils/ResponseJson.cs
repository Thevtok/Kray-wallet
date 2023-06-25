using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Go_Saku.Net.Utils
{
    public static class ResponseUtils
    {
        public static void JSONSuccess(HttpContext context, bool status, int statusCode, object result)
        {
            var response = new
            {
                Message = "Request success",
                Status = status,
                StatusCode = statusCode,
                Result = result,
               
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            context.Response.WriteAsync(jsonResponse);
        }
    }
}

using Go_Saku.Net.Repositories;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

namespace go_saku_cs.Middleware
{
    public class AuthMiddlewareUsername
    {
        private readonly RequestDelegate _next;
        private readonly SymmetricSecurityKey _jwtKey;

        public AuthMiddlewareUsername(RequestDelegate next, SymmetricSecurityKey jwtKey)
        {
            _next = next;
            _jwtKey = jwtKey;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            // Add log statement here
            Console.WriteLine("Authenticating user...");

            string tokenString = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(tokenString))
            {
                Console.WriteLine("Unauthorized");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            string token = tokenString.Trim();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken parsedToken = null;

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _jwtKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                parsedToken = (JwtSecurityToken)validatedToken;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to validate token");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var email = parsedToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var password = parsedToken.Claims.FirstOrDefault(c => c.Type == "password")?.Value;
            var username = parsedToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Invalid token claims");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            string requestedId = context.Request.RouteValues["username"]?.ToString();
            Console.WriteLine("Requested ID: " + requestedId);
            Console.WriteLine("Username: " + username);



            if (username != requestedId)
           

            {
                Console.WriteLine("Access forbidden");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access forbidden: You are not authorized to access this resource");
                return;
            }

            context.Items["email"] = email;
            context.Items["password"] = password;
            context.Items["username"] = username;

            await _next(context);

            Console.WriteLine("Success parsing middleware");
        }
    }
}

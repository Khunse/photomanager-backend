using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace imageuploadandmanagementsystem.Common
{
    public class JWTMiddleware
    {
     public JWTMiddleware()
     {
        
     }   

     public async Task InvokeAsync(HttpContext context)
     {
        var token = context.Request.Headers["Authorization"].ToString();
        try
        {
            if( token is null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            if( !token.StartsWith("Bearer"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var jwtToken = token.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
            
            if(tokenS is null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"ERROR :: {ex.Message}");
            await context.Response.WriteAsync("Internal Server Error");
        }
     }

    }
}
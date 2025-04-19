using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using imageuploadandmanagementsystem.Common;
using imageuploadandmanagementsystem.Data.Repository;
using imageuploadandmanagementsystem.Model;
using imageuploadandmanagementsystem.Service.UserService;
using Microsoft.IdentityModel.Tokens;

namespace imageuploadandmanagementsystem.Service.Authentication
{
    public class JWTAuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userrepsiory;
        private readonly string Issuer = "";
        private readonly string Audience = "";
        private readonly string Key = "";
        public JWTAuthenticationService(IUserRepository userRepository,IConfiguration configuration)
        {
            var jwtParameters = configuration.GetSection("JWTParameters");
            Issuer = jwtParameters["Issuer"] ?? throw new ArgumentNullException("JWT_Issuer");  
            Audience = jwtParameters["Audience"] ?? throw new ArgumentNullException("JWT_Audience");
            Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentNullException("JWT_KEY");
            _userrepsiory = userRepository;
        }
        public Task<ResponseModel<AuthenticationResponse>> LoginAsync(LoginRequest request)
        {
            var response = new ResponseModel<AuthenticationResponse>();
           try
           {

             var user =_userrepsiory.GetUserByEmail(request.Email).Result;
            if(user is null)
            {
                response.Message = "User not found";
                return Task.FromResult(response);
            }

            if(!Encryption.ValidatePassword(request.Password, user.Password, user.SaltKey))
            {
                response.Message = "Invalid password";
                return Task.FromResult(response);
            }

            string token = GenerateJWTToken(request.Email);

            response.IsSuccess = true;
            response.Message = "Login successful";
            response.Resp = new AuthenticationResponse(token, string.Empty);
            
            return Task.FromResult(response);

           }
           catch(Exception ex)
           {
                Console.WriteLine("Error :::" + ex.Message);
                response.Message = "Failed to login";
                return Task.FromResult(response);
           }
        }

        private string GenerateJWTToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                   new Claim(ClaimTypes.Name, email),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                ]),
                Audience = Audience,
                Issuer = Issuer,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
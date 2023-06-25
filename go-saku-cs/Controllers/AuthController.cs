using Go_Saku.Net.Utils;
using Go_Saku.Net.Usecase;

using Microsoft.IdentityModel.Tokens;


using go_saku_cs.Models;
using Microsoft.AspNetCore.Mvc;
using dotenv.net;

using System.Net;


namespace Go_Saku.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserUsecase _authUsecase;
        private readonly TokenService _tokenService;

        public AuthController(IUserUsecase authUsecase)
        {
            _authUsecase = authUsecase;
            _tokenService = _tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var user = await _authUsecase.GetByEmailAndPassword(loginRequest.Email, loginRequest.Password, "");

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                bool isPasswordValid = PasswordUtils.VerifyPassword(loginRequest.Password, user.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }


                DotEnv.Load();

                string jwtKey = Environment.GetEnvironmentVariable("JWTKEY");
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey);
                byte[] validKeyBytes = new byte[32]; 
                Array.Copy(keyBytes, validKeyBytes, Math.Min(keyBytes.Length, validKeyBytes.Length));

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(validKeyBytes);

                // Gunakan securityKey untuk menginisialisasi TokenService
                var tokenService = new TokenService(securityKey);


                var token = tokenService.GenerateToken(user);




                await _authUsecase.SaveDeviceToken(user.ID, loginRequest.Token);

                var response = new { token = token};

                ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, response);

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to login", error = ex.Message });
            }
        }
    }
}
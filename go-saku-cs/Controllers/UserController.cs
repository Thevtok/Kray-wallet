using Go_Saku.Net.Models;
using Go_Saku.Net.Usecase;
using Go_Saku.Net.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Go_Saku.Net.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserUsecase _userUsecase;

        public UserController(IUserUsecase userUsecase)
        {
            _userUsecase = userUsecase;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _userUsecase.FindAllUsers();
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, users);

            return new EmptyResult();
        }

        [HttpGet("name/{username}")]


        public ActionResult<User> GetByUsername(string username)
        {
            var user = _userUsecase.FindByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, user);

            return new EmptyResult();

        }
        [HttpGet("phone/{phone_number}")]
       

        public ActionResult<User> GetByPhone(string phone_number)
        {
            var user = _userUsecase.FindyPhonBe(phone_number);
            if (user == null)
            {
                return NotFound();
            }
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, user);

            return new EmptyResult();

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(User userDto)
        {
            // Validasi input
            if (string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.PhoneNumber) || string.IsNullOrEmpty(userDto.Address))
            {
                return BadRequest("Invalid Input: Required fields are empty");
            }

            if (!userDto.Email.EndsWith("@gmail.com"))
            {
                return BadRequest("Email must be a Gmail address");
            }

            if (userDto.Password.Length < 8)
            {
                return BadRequest("Invalid Input: Password must have at least 8 characters");
            }

            if (!IsValidPassword(userDto.Password))
            {
                return BadRequest("Password must contain at least one uppercase letter and one number");
            }

            if (userDto.PhoneNumber.Length < 11 || userDto.PhoneNumber.Length > 13)
            {
                return BadRequest("Phone number must be 11 - 13 digits");
            }

            // Panggil use case untuk membuat pengguna
            string result = await _userUsecase.CreateUser(userDto);

            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.Created, result);

            return new EmptyResult();
        }

        private bool IsValidPassword(string password)
        {
            bool hasNumber = false;
            bool hasUpper = false;

            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
            }

            return hasNumber && hasUpper;
        }
    }


}


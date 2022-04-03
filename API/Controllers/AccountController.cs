using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private readonly ITokenService _tokeService;
        public AccountController(ILogger<AccountController> logger, DataContext context, ITokenService tokeService)
        {
            _tokeService = tokeService;
            _logger = logger;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto)
        {
            if (await UserExists(dto.username))
            {
                return BadRequest("Username is taken.");
            }

            AppUser user = null;

            try
            {
                using var hmac = new HMACSHA512();

                user = new AppUser {
                    UserName = dto.username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
                    PasswordSalt =  hmac.Key
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();                
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error Registering. {ex.Message}");
            }

            return new UserDTO {
                Username = user.UserName,
                Token =  _tokeService.CreateToken(user)
            };

            //return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
            AppUser user = null;
            string strErrMessage = "Login Error! ";

            try
            {
                 user = await _context.Users
                        .SingleOrDefaultAsync(x => x.UserName == dto.username.ToLower());
                
                if (null != user)
                {
                    using var hmac = new HMACSHA512(user.PasswordSalt);

                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password));

                    if (computedHash.Length == user.PasswordHash.Length)
                    {
                        for (int i = 0; i < user.PasswordHash.Length; i++)
                        {
                            if (computedHash[i] != user.PasswordHash[i])
                            {
                                strErrMessage += "Invalid Password";
                                user = null;
                                break;
                            }    
                        }

                    }
                    else
                    {
                        strErrMessage += "Invalid Password";
                        user = null;                        
                    }
                }
                else
                {
                    strErrMessage += "User Not Found";
                }
            }
            catch (System.Exception ex)
            {
                strErrMessage += ex.Message;
            }

            if (null == user)
            {
                return Unauthorized(strErrMessage);
            }

            return new UserDTO {
                Username = user.UserName,
                Token =  _tokeService.CreateToken(user)
            };
            
        }
        
        private async Task<bool> UserExists(String username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}
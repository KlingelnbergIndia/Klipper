// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Models.Core.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using KlipperAuthorization;
using KlipperApi.Controllers.Employees;

namespace KlipperApi.Controllers.Auth
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeesAccessor _employeesAccessor;

        public AuthController(IUserRepository userRepository, IEmployeesAccessor employeesAccessor)
        {
            _userRepository = userRepository;
            _employeesAccessor = employeesAccessor;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if(_userRepository.GetByUserName(user.UserName) == null)
            {
                Serilog.Log.Logger.Error("Username is not found.");
                return NotFound();
            }

            if (_userRepository.ValidateCredentials(user.UserName, user.PasswordHash))
            {
                var returnedUser = _userRepository.GetByUserName(user.UserName).Result;

                // only set explicit expiration here if persistent. 
                // otherwise we reply upon expiration configured in cookie middleware.
                var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(3650)
                };

                var employee = await _employeesAccessor.GetEmployeeByIdAsync(returnedUser.ID);
                var roles = employee.Roles;
                SessionCache.Employees.Add(user.UserName, employee);

                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Sid, employee.ID.ToString(), ClaimValueTypes.UInteger32));
                claims.Add(new Claim(ClaimTypes.Email, employee.Email, ClaimValueTypes.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                foreach (var r in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, r, ClaimValueTypes.String));
                }

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KlipperSigningKey"));

                var token = new JwtSecurityToken(
                    issuer: "http://www.Klingelnberg.com",
                    audience: "http://www.Klingelnberg.com",
                    expires: DateTime.UtcNow.AddDays(5),
                    claims: claims.ToArray(),
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    Username = user.UserName
                });
            }
            ModelState.AddModelError("", "Error in user authentication");
            Serilog.Log.Logger.Error("Error in user authentication");

            return Unauthorized();
        }

        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    return Ok();
        //}
    }
}
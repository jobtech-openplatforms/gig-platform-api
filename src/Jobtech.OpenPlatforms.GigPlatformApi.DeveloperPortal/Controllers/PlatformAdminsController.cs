using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.AdminEngine.Managers;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Helpers;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Models.App.Responses;
using Jobtech.OpenPlatforms.GigPlatformApi.PlatformEngine.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class PlatformAdminsController : ControllerBase
    {
        private IPlatformAdminUserManager _platformAdminUserManager;
        private IPlatformManager _platformManager;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PlatformAdminsController(
            IPlatformAdminUserManager platformAdminUserManager,
            IPlatformManager platformManager,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _platformAdminUserManager = platformAdminUserManager;
            _platformManager = platformManager;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]PlatformAdminUserModel userDto)
        {
            var user = await _platformAdminUserManager.Authenticate(userDto.Email, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = ((PlatformAdminUserId)user.Id).Short(),
                user.Email,
                user.Name,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]PlatformAdminUserModel userModel)
        {
            // map dto to entity
            var user = _mapper.Map<PlatformAdminUser>(userModel);

            try
            {
                // save
                var admin = await _platformAdminUserManager.CreateAsync(user, userModel.Password);
                //var platform = await _platformManager.RegisterPlatformAsync(new PlatformRequest
                //{
                //    Description = "",
                //    ExportDataUri = "",
                //    MyGigDataToken = Guid.NewGuid().ToString(),
                //    PlatformToken = Guid.NewGuid().ToString(),
                //    Name = "",
                //    PushNotificationUri = ""
                //});
                //await _platformManager.AddAdminAsync(platform, admin);
                return Ok();
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<IActionResult> ResetLogin([FromBody]string  email)
        {
            //Random random = new Random();
            //var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //var password = new string(Enumerable.Repeat(chars, 20)
            //  .Select(s => s[random.Next(s.Length)]).ToArray());

            var user = await _platformAdminUserManager.GetUserAsync(email);

            try
            {
                //await _platformAdminUserManager.ResetLoginAsync(email, password);
                var resetCode = await _platformAdminUserManager.PasswordResetCodeAsync(user.Id);
                return Ok(new { ResetCode = resetCode });
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("platforms")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _platformAdminUserManager.GetAllAsync();
            var userDtos = _mapper.Map<IList<PlatformAdminUserModel>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            var user = await _platformAdminUserManager.GetUserAsync((PlatformAdminUserId)id);
            var userDto = _mapper.Map<PlatformAdminUserModel>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]string id, [FromBody]PlatformAdminUserModel adminUser)
        {
            // map dto to entity and set id
            var user = _mapper.Map<PlatformAdminUser>(adminUser);
            user.Id = id;

            try
            {
                // save
                await _platformAdminUserManager.UpdateAsync(user, adminUser.Password);
                return Ok();
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            await _platformAdminUserManager.DeleteAsync((PlatformAdminUserId)id);
            return Ok();
        }

        /// <summary>
        /// Updates the platform information for the admin.
        /// TODO: Add a check that this admin is authorized to update this platform
        /// </summary>
        /// <param name="id"></param>
        /// <param name="platformUpdate"></param>
        /// <returns></returns>
        [HttpPut("platform/{id}")]
        public async Task<IActionResult> UpdatePlatform([FromRoute]string id, [FromBody]PlatformUpdateRequestModel platformUpdate)
        {
            var platformRequest = _mapper.Map<PlatformRequest>(platformUpdate);
            var response = await _platformManager.UpdatePlatformAsync(id, platformRequest);

            return Ok(response.ForEditing());
        }

        /// <summary>
        /// Updates the contact information for the admin.
        /// TODO: Add a check that this admin is authorized to update this account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactUpdate"></param>
        /// <returns></returns>
        [HttpPut("contact/{id}")]
        public async Task<IActionResult> UpdateContact([FromRoute]string id, [FromBody]PlatformUpdateContactRequestModel contactUpdate)
        {
            var contact = await _platformAdminUserManager.UpdateContactAsync((PlatformAdminUserId)id, contactUpdate);
            var response = new
            {
                contact.Name,
                Id = ((PlatformAdminUserId)contact.Id).Short(),
                contact.Email
            };
            return Ok(response);
        }

        /// <summary>
        /// Get the platform information for the admin.
        /// TODO: Add a check that this admin is authorized to view this platform
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("platform/{id}")]
        public async Task<IActionResult> GetPlatform([FromRoute]PlatformId id)
        {
            var platform = await _platformManager.GetPlatformAsync(id);
            return Ok(platform.ForEditing());
        }

        /// <summary>
        /// Get the platform information for the admin.
        /// TODO: Add a check that this admin is authorized to view this platform
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/platforms")]
        public async Task<IActionResult> GetPlatformsForAdmin([FromRoute]string id)
        {
            var platforms = await _platformAdminUserManager.GetPlatformsAsync((PlatformAdminUserId)id);
            return Ok(platforms.ForEditing());
        }
    }
}
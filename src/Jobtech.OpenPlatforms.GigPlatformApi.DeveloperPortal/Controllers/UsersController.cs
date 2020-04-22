using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions;
using Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobtech.OpenPlatforms.GigPlatformApi.DeveloperPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public UsersController(
            IUserManager userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.GetAllAsync();
            var userDtos = _mapper.Map<IList<PlatformAdminUserModel>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.GetUserAsync(id);
            var userDto = _mapper.Map<PlatformAdminUserModel>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]PlatformAdminUserModel userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save
                await _userManager.UpdateAsync(user, userDto.Password);
                return Ok();
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userManager.DeleteAsync(id);
            return Ok();
        }
    }
}
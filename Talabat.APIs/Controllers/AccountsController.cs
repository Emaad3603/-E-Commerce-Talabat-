using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.InterFaces;

namespace Talabat.APIs.Controllers
{ 
    public class AccountsController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountsController
            (UserManager<AppUser>  userManager,
            SignInManager<AppUser> signManager ,
            ITokenServices tokenServices,
            IMapper mapper)
        {
           _userManager = userManager;
           _signManager = signManager;
           _tokenServices = tokenServices;
           _mapper = mapper;
        }


        [HttpPost("Register")]

       public async Task<ActionResult<UserDto>> Register(RegisterDto model)
       {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email is already exists"));
            }
            else
            {

                var user = new AppUser()
                {
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    UserName = model.Email.Split("@")[0],
                    PhoneNumber = model.PhoneNumber,

                };
                var Result = await _userManager.CreateAsync(user, model.Password);
                if (!Result.Succeeded)
                {
                    return BadRequest(new ApiResponse(400));
                }
                else
                {
                    var returnedUser = new UserDto()
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email,
                        Token = await _tokenServices.CreateTokenAsync(user, _userManager)
                    };
                    return Ok(returnedUser);
                }
            }
       }


        [HttpPost("Login")]
       public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user =  await  _userManager.FindByEmailAsync(model.Email);
            if(user is null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            else
            {
              var result = await  _signManager.CheckPasswordSignInAsync(user, model.Password, false);
              if(!result.Succeeded)
              {
                    return Unauthorized(new ApiResponse(401));
              }
                else
                {
                    return Ok(new UserDto()
                    {
                        DisplayName=user.DisplayName, 
                        Email=user.Email,
                        Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                    });
                }
            }
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
       public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var userEmail =  User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(userEmail);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager),
            });
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {


            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedAddress = _mapper.Map<Address, AddressDto>(user.Address);

            return Ok(mappedAddress);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress (AddressDto model)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var address =  _mapper.Map<AddressDto, Address>(model);

            user.Address = address;

            var result = await  _userManager.UpdateAsync(user);

            if(!result.Succeeded)
            {
                return BadRequest(400);
            }
            else
            {
                return Ok(model);
            }

           
        }


        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists (string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}

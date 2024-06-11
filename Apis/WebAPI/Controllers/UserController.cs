//using Application.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Application.ViewModels.UserViewModels;
//using AutoMapper.Configuration.Conventions;
//using System.Net;
//using Application.Commons;
//using System.ComponentModel;
//using Application.ViewModels.RequestModels;

//namespace WebAPI.Controllers
//{
//    public class UserController : BaseController
//    {
//        private readonly IUserService _userService;

//        public UserController(IUserService userService)
//        {
//            _userService = userService;
//        }

//        [HttpGet]
//        public async Task<Pagination<UserListDTO>> GetUserList([FromQuery][DefaultValue(0)] int pageIndex,
//                                                               [FromQuery][DefaultValue(10)] int pageSize) 
//                                                    => await _userService.GetListUsers(pageIndex, pageSize);

//        [HttpPost]
//        public async Task<HttpStatusCode> Login(LoginRequestModel loginRequestModel) => await _userService.Login(loginRequestModel);

//        [HttpPost]
//        public async Task<HttpStatusCode> Register(RegisterRequestModel registerRequestModel) => await _userService.Register(registerRequestModel);


//        [HttpPost("{email}")]
//        public async Task<HttpStatusCode> CheckUserExisted(string email) => await _userService.UserExists(email);

//    }
//}
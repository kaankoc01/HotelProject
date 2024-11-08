﻿using HotelProject.EntityLayer.Concrete;
using HotelProject.WebUI.Dtos.RegisterDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelProject.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<Appuser> _userManager;

        public RegisterController(UserManager<Appuser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateNewUserDto createNewUserDto)
        {
            // şifre 123456aA*
            if (!ModelState.IsValid) 
            {
                return View();
            }
            var appUser = new Appuser()
            {
                Name = createNewUserDto.Name,
                Surname = createNewUserDto.Surname,
                Email = createNewUserDto.Mail,
                UserName = createNewUserDto.Username
            };
            
            var result = await _userManager.CreateAsync(appUser,createNewUserDto.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Login");
            }
            
            return View();

        }
    }
}
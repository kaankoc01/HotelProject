using HotelProject.EntityLayer.Concrete;
using HotelProject.WebUI.Models.Setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelProject.WebUI.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<Appuser> _userManager;

        public SettingsController(UserManager<Appuser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        // Bilgileri getirdik 
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            userEditViewModel.Name = user.Name;
            userEditViewModel.Surname = user.Surname;
            userEditViewModel.Email = user.Email;
            userEditViewModel.Username = user.UserName;
            return View(userEditViewModel);
        }
        [HttpPost]
        // Bilgileri Güncellicez.
        public async Task<IActionResult> Index(UserEditViewModel userEditViewModel)
        {
            if(userEditViewModel.Password == userEditViewModel.ConfirmPassword)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.Name = userEditViewModel.Name;
                user.Surname = userEditViewModel.Surname;
                user.Email = userEditViewModel.Email;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userEditViewModel.Password);
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}

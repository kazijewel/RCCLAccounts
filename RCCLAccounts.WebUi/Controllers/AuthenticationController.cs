using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Services;
using ProvidentFund.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ProvidentFund.WebUi.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private IHttpContextAccessor _httpAccessor;
        private ISession _session => _httpAccessor.HttpContext.Session;
        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _httpAccessor = httpAccessor;
        }
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromForm] LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = ModelState.Values
				 .SelectMany(x => x.Errors)
				 .Select(x => x.ErrorMessage)
				 .ToList();

				return View(model);
				
			}
			//sign in the user
			var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).Result;
			if (result.Succeeded)
			{
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    setCompanyInfo(user.FullName, user.UserImage);
                   //setUserImage(user.Email);

                    user.LastLoginDateTime = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                           
			}
			else
			{
				ModelState.AddModelError("", "Invalid login attempt");
			}

			return View(model);
		}

        public async Task setUserImage(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                _session.SetString("profilePic", user.UserImage == null ? "" : user.UserImage);
            }
        }

        public async Task setCompanyInfo(string userName, string userImage)
        {
            //var company = _unitOfWork.Company.GetFirstOrDefault();
            _session.SetString("userName", userName);
            _session.SetString("userIp", getIp());
            _session.SetString("developer", "Developed By : JR Software");
            _session.SetString("phone", "Email: rupaliho@gmail.com");
            _session.SetString("companyName", "Rupali Credit Co-operative Ltd.");
            _session.SetString("companyAddress", "BM Heights,2nd Floor,318 Shekh Mujib Road,Badamtooli Moor,Agrabad,Chittagong, Bangladesh, 4100");

            _session.SetString("profilePic", userImage == null ? "" : userImage);

        }

        public static string getIp()
        {
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            return GetIPV4;
        }

        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register([FromForm] RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Error = ModelState.Values
					 .SelectMany(x => x.Errors)
					 .Select(x => x.ErrorMessage)
					 .ToList();

				return View(model);
			}

			ApplicationUser applicationUser = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				FullName = model.FullName,
				PhoneNumber = model.PhoneNumber,
			};
			IdentityResult result = await _userManager.CreateAsync(applicationUser, model.Password);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(applicationUser, isPersistent: true);
				return RedirectToAction("Index", "Home");
			}

			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Authentication");
		}


		[AllowAnonymous]
        public async Task<IActionResult> UserProfile()
		{
			var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var model = new UserProfileModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
					PhoneNumber=user.PhoneNumber,
					UserImage= user.UserImage

                    // Load o=ther profile fields
                };

                return View(model);
            }
            else
            {
                // User not found
                return NotFound();
            }          
		}


        [HttpPost]
        public async Task<IActionResult> UserProfile( UserProfileModel model)
        {

            string pictureFolder = "images/userPicture/";
            string picturepath = "";
            string rootpath = Directory.GetCurrentDirectory() + "/wwwroot";
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                   // user.FullName = model.FullName;
                   // user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    if (model.ImageUp != null)
                    {
                        if (model.ImageUp.Length > 0)
                        {

                            picturepath = pictureFolder + Guid.NewGuid() +
                               model.ImageUp.FileName;

                            await model.ImageUp.CopyToAsync
                             (new FileStream(Path.Combine(rootpath, picturepath),
                             FileMode.Create));
							user.UserImage = picturepath;
                        }
                    }


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _session.SetString("profilePic", picturepath);
                        // Profile updated successfully
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Handle errors
                        ModelState.AddModelError(string.Empty, "Error updating profile.");
                    }
                }
                else
                {
                    // User not found
                    return NotFound();
                }
          
             }


            return View(model);

        }
        //Pasword Change
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange([FromForm] PasswordChangeModel model)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound("User not found");
            }
           
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Optionally sign in the user again with the new password
               //await _signInManager.RefreshSignInAsync(user);

               await _signInManager.SignOutAsync();
               return RedirectToAction("Login", "Authentication");

                }
            else
            {
              
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

          }
            return View(model);
        }

    }
 
}

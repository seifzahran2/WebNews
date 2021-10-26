using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebNews.Models;

namespace WebNews.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Appuser> _userManager;
        private readonly SignInManager<Appuser> _signInManager;

        public IndexModel(
            UserManager<Appuser> userManager,
            SignInManager<Appuser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Profile Picture")]
            public byte[] ProfilePic { get; set; }
            [Required]
            [Display(Name = "User name")]
            public string Username { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            [MaxLength(11)]
            [MinLength(11)]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Appuser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = user.FirstName,
                LastName=user.LastName,
                ProfilePic = user.ProfilePic
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var FName = user.FirstName;
            if (Input.FirstName != FName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            var LName = user.LastName;
            if (Input.LastName != FName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
            var UName = user.UserName;
            if (Input.Username != UName)
            {
                user.UserName = Input.Username;
                await _userManager.UpdateAsync(user);
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if(Request.Form.Files.Count>0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                using(var datastream = new MemoryStream())
                {
                    await file.CopyToAsync(datastream);
                    user.ProfilePic = datastream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

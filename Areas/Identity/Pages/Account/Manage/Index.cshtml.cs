// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork;

namespace StudentClub.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            AppDbContext db,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _unitOfWork=unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }


        public Student student { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public Image image { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            string userId = _userManager.GetUserId(User);
            var stud = await _db.Students.FirstOrDefaultAsync(x => x.UserId == userId);
            var studId = stud != null ? stud.Id : (int?)null;
            var image = await _db.Images.FirstOrDefaultAsync(x => x.studentId == studId);

            if (stud == null)
            {
                student = new Student() { UserId = userId };
            }
            else
            {
                student = stud;
            }
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                image = image,
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
            try
            {
                string userId = _userManager.GetUserId(User);
            var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == userId);
            var imageExist = _db.Images.Any(x => x.studentId == student.Id);

                if (student != null   && Input.image != null)
                {

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await Input.image.clientFile.CopyToAsync(stream);
                        if (imageExist == false)
                        {
                            var image = new Image
                            {
                                imagePath = stream.ToArray(),
                                studentProfile = student,
                                studentId = student.Id
                            };
                            _unitOfWork.images.AddOne(image);
                        }
                        else
                        {
                            Console.Write("valid to Update *****************************************************************!");
                            var imageModif = _db.Images.FirstOrDefault(x => x.studentId == student.Id);
                            imageModif.imagePath = stream.ToArray();
                            _unitOfWork.images.UpdateOne(imageModif);
                        }
                    }

                }


            }
            catch (Exception)
            {

                Console.Write("***************************error*****************************");
            }




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

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage("Index");

        }



    }
}

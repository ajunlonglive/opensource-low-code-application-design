#region Copyright Syncfusion Inc. 2001-2018.

// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.

#endregion Copyright Syncfusion Inc. 2001-2018.

using Domain.Entities;
using ISDAOC_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ISDAOC_web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<DCAppUser> _userManager;

        public HomeController(UserManager<DCAppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = _userManager.GetUserName(User);
            if (user == "johnmangam@gmail.com" || user == "emmyjstephen@gmail.com" || user.EndsWith("iemoutreach.org"))
                return View();
            else
                return View("AccessDenied");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
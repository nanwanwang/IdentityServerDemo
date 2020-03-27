using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public async Task<IActionResult> AuthenticateAsync()
        {
            var demonClaims = new List<Claim>()
            {
              new Claim(ClaimTypes.Name,"Bob"),
              new Claim(ClaimTypes.Email,"123@hotmail.com"),
              new Claim("Demon.Says","very nice boy")
            };
            var licenseClaims = new List<Claim>
            {
            new Claim(ClaimTypes.Name,"ddddd"),
            new Claim("DrivingLicense","A+")
            };
            var demonIdentity = new ClaimsIdentity(demonClaims, "Demon Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");
            var userPrincipal = new ClaimsPrincipal(new[] { demonIdentity, licenseIdentity });

            await HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
    }
}

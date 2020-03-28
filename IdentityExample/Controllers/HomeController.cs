using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IEmailService  emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                //sign in 
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(string username, string password)
        {

            var user = new IdentityUser
            {
                UserName = username,
                Email = "nanwanwangjian@163.com"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //sign in by username add password
                //var signiResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                //if (signiResult.Succeeded)
                //{
                //    RedirectToAction("Index");
                //}


                //sigin in by email confirm
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());




                var messageToSend = new MimeMessage
                {
                    Sender = new MailboxAddress("demon", "766066375@qq.com"),

                    Subject = "Verfiy Email",
                    Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"<a href=\"{link}\">emial verify</a>" }
                };

                messageToSend.To.Add(new MailboxAddress(user.Email));


                using (SmtpClient client = new SmtpClient())
                {
                    client.MessageSent += (sender, args) => { Console.WriteLine(args.Response); };


                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("smtp.qq.com", 25, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync("766066375@qq.com", "");

                    await client.SendAsync(messageToSend);

                    await client.DisconnectAsync(true);
                };
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }


        public IActionResult Register()
        {

            return View();
        }


        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}

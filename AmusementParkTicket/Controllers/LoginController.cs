using System.Numerics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
namespace AmusementPark.Controllers
{
    public class LoginController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IOtpService _otpService;

        public LoginController(
            IPlayerService playerService,
            IOtpService otpService)
        {
            _playerService = playerService;
            _otpService = otpService;
        }


        // =====================================================
        // Login GET
        // =====================================================

        [HttpGet]
        public IActionResult Login()
        {
            // اگر کاربر قبلاً Login کرده،
            // دوباره صفحه Login را نشان نده.

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(
                    "Home",
                    "Home");
            }

            return View();
        }


        // =====================================================
        // Login POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(
            string? username,
            string? mobile)
        {
            username =
                username?.Trim() ?? string.Empty;

            mobile =
                NormalizeDigits(mobile);

            mobile =
                mobile.Trim();


            // =================================================
            // Validation نام کاربری
            // =================================================

            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["LoginError"] =
                    "نام کاربری را وارد کنید.";

                return View();
            }


            // =================================================
            // Validation موبایل
            // =================================================

            if (!Regex.IsMatch(
                    mobile,
                    @"^09\d{9}$"))
            {
                TempData["LoginError"] =
                    "شماره موبایل باید دقیقاً ۱۱ رقم باشد و با ۰۹ شروع شود.";

                return View();
            }


            // =================================================
            // پیدا کردن کاربر
            // =================================================

            Player? playerByMobile =
                _playerService.GetByMobile(mobile);

            Player? playerByName =
                _playerService.GetByName(username);


            // =================================================
            // کاربر جدید
            // =================================================

            if (playerByMobile == null &&
                playerByName == null)
            {
                Player newPlayer =
                    new Player
                    {
                        PlayerName = username,
                        PlayerMobile = mobile
                    };


                // ثبت در دیتابیس
                _playerService.Add(newPlayer);


                // دریافت دوباره برای گرفتن PlayerID
                playerByMobile =
                    _playerService.GetByMobile(mobile);
            }


            // =================================================
            // موبایل قبلاً ثبت شده
            // =================================================

            else if (playerByMobile != null)
            {
                if (playerByMobile.PlayerName != username)
                {
                    TempData["LoginError"] =
                        "این شماره موبایل قبلاً با نام کاربری دیگری ثبت شده است.";

                    return View();
                }
            }


            // =================================================
            // نام کاربری قبلاً ثبت شده
            // =================================================

            else if (playerByName != null)
            {
                if (playerByName.PlayerMobile != mobile)
                {
                    TempData["LoginError"] =
                        "این نام کاربری قبلاً با شماره موبایل دیگری ثبت شده است.";

                    return View();
                }
            }


            // =================================================
            // بررسی نهایی
            // =================================================

            if (playerByMobile == null)
            {
                TempData["LoginError"] =
                    "ثبت کاربر انجام نشد. دوباره تلاش کنید.";

                return View();
            }


            // =================================================
            // تولید OTP
            // =================================================

            string otp =
                _otpService.GenerateCode();


            // =================================================
            // Session موقت
            // =================================================

            HttpContext.Session.SetString(
                "LoginPlayerId",
                playerByMobile.PlayerID.ToString()!);

            HttpContext.Session.SetString(
                "LoginMobile",
                mobile);

            HttpContext.Session.SetString(
                "LoginOtp",
                otp);


            // فقط برای تست
            HttpContext.Session.SetString(
                "TestOtp",
                otp);


            return RedirectToAction(
                nameof(Otp));
        }


        // =====================================================
        // OTP GET
        // =====================================================

        [HttpGet]
        public IActionResult Otp()
        {
            string? playerId =
                HttpContext.Session.GetString(
                    "LoginPlayerId");


            if (string.IsNullOrWhiteSpace(playerId))
            {
                return RedirectToAction(
                    nameof(Login));
            }


            ViewBag.Mobile =
                HttpContext.Session.GetString(
                    "LoginMobile");


            // فقط برای تست
            ViewBag.TestOtp =
                HttpContext.Session.GetString(
                    "TestOtp");


            return View();
        }


        // =====================================================
        // Verify OTP
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(
            string? otp)
        {
            string? generatedOtp =
                HttpContext.Session.GetString(
                    "LoginOtp");

            string? playerId =
                HttpContext.Session.GetString(
                    "LoginPlayerId");


            if (string.IsNullOrWhiteSpace(
                    generatedOtp) ||
                string.IsNullOrWhiteSpace(
                    playerId))
            {
                return RedirectToAction(
                    nameof(Login));
            }


            otp =
                NormalizeDigits(otp);


            bool isValid =
                _otpService.ValidateCode(
                    otp,
                    generatedOtp);


            if (!isValid)
            {
                TempData["OtpError"] =
                    "کد وارد شده اشتباه است.";

                return RedirectToAction(
                    nameof(Otp));
            }
            int playerIdValue;

            if (!int.TryParse(playerId, out playerIdValue))
            {
                return RedirectToAction(
                    nameof(Login));
            }

            Player? player =
                _playerService.GetById(playerIdValue);

            if (player == null)
            {
                TempData["LoginError"] =
                    "اطلاعات کاربر پیدا نشد. دوباره وارد شوید.";

                return RedirectToAction(
                    nameof(Login));
            }

            // =================================================
            // Login موفق
            // =================================================
            var claims = new List<Claim>
{
    new Claim(
        ClaimTypes.NameIdentifier,
        player.PlayerID.ToString()),

    new Claim(
        ClaimTypes.Name,
        player.PlayerName ?? string.Empty)
};

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddYears(10)
                });

            // =================================================
            // Cookie دائمی
            // =================================================

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,

                    ExpiresUtc =
                        DateTimeOffset.UtcNow
                            .AddYears(10),

                    AllowRefresh = true
                });


            // =================================================
            // Session
            // =================================================

            HttpContext.Session.SetString(
                "IsLoggedIn",
                "true");

            HttpContext.Session.SetString(
                "PlayerId",
                playerId);


            // =================================================
            // پاک کردن اطلاعات موقت OTP
            // =================================================

            HttpContext.Session.Remove(
                "LoginOtp");

            HttpContext.Session.Remove(
                "LoginPlayerId");

            HttpContext.Session.Remove(
                "LoginMobile");

            HttpContext.Session.Remove(
                "TestOtp");


            // =================================================
            // برگشت به Home
            // =================================================

            return RedirectToAction(
                "Home",
                "Home");
        }


        // =====================================================
        // Logout
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // حذف Cookie احراز هویت
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme);


            // حذف Session
            HttpContext.Session.Clear();


            // برگشت به Home
            return RedirectToAction(
                "Home",
                "Home");
        }


        // =====================================================
        // تبدیل اعداد فارسی به انگلیسی
        // =====================================================

        private static string NormalizeDigits(
            string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;


            return value
                .Replace('۰', '0')
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9');
           
        }
    }
}
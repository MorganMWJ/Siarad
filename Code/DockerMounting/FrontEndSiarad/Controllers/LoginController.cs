using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using FrontEndSiarad.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrontEndSiarad.Controllers
{
    public class LoginController : Controller
    {
        public static Boolean userLoggedIn = false;
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([Bind("Username,Password")] Login loginInput)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"loginInfo.txt");
            StreamReader sr = new StreamReader(path);
            Dictionary<string, string> loginDetails = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                loginDetails.Add(sr.ReadLine(), sr.ReadLine());
            }
            sr.Close();
            try
            {
                if (loginDetails[loginInput.Username] == loginInput.Password)
                {
                    //Some kind of identity filter needed to allow modification of navigation setup
                    userLoggedIn = true;
                    return RedirectToAction("Index", "Home");
                }
            }catch(ArgumentNullException e)
            {
                return View();
            }
            if (loginDetails[loginInput.Username] != loginInput.Password)
            {
                ViewData["Message"] = "Username or Password is incorrect";
            }
            return View(); //Need some kind of notification to user
        }
    }
}
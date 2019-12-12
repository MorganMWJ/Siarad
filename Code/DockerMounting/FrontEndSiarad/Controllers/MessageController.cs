using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FrontEndSiarad.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Message()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Message(String messageRecipient, String message)
        {
            if(messageRecipient == null)
            {
                ViewData["RecipientExist"] = "Please enter a Recipient.";
                return View();
            }
            if (message == null)
            {
                var url = "http://m56-docker1.dcs.aber.ac.uk:8100/students";
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                StringBuilder sb = new StringBuilder(sr.ReadToEnd());
                sr.Close();
                sb.Remove(0, 1);
                sb.Remove(sb.Length - 1, 1);

                String[] responses = sb.ToString().Replace("\"", "").Split(",");
                foreach (String s in responses)
                {
                    if (messageRecipient.ToUpperInvariant().Equals(s))
                    {
                        ViewData["RecipientExist"] = "Recipient Found!";
                        return View();
                    }
                }
                ViewData["RecipientExist"] = "Recipient not found.";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
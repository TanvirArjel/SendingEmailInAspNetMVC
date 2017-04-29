using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SendingEmailInAspNetMVC.Models;
using System.Net.Mail;
using System.Net;

namespace SendingEmailInAspNetMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                var mailFrom = contactForm.Email;
                var mailTo = "tanvirarjel@hotmail.com"; // To which you want to send the mail
                using (MailMessage mail = new MailMessage(mailFrom,mailTo))
                {
                    mail.From = new MailAddress(contactForm.Email);
                    mail.Subject = contactForm.Subject;
                    mail.Body = "Hello I am " + contactForm.Name+"," + "<br/><br/>" + contactForm.Message;
                    mail.ReplyToList.Add(new MailAddress(contactForm.Email));

                    if (contactForm.UploadFile != null && contactForm.UploadFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(contactForm.UploadFile.FileName);
                        mail.Attachments.Add(new Attachment(contactForm.UploadFile.InputStream, fileName));
                    }
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        //Smtp configuration.You can also do it in web.config file
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential networkCredential = new NetworkCredential("your gmail email", "password");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = networkCredential;
                        smtp.Port = 587; //This for gmail

                        smtp.Send(mail);

                        ModelState.Clear();
                        ViewBag.SendingStatus = "Sent";
                        return View();
                    }

                }
            }
            return View();
        }
    }
}
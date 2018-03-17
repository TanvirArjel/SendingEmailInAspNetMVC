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
using System.Threading.Tasks;
using System.Web.Hosting;

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

        

        public string GetEmailTemplate()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>()
            {
                new EmployeeViewModel() { Company = "Alfreds Futterkiste", Contact = "Maria Anders", Country = "Germany"},
                new EmployeeViewModel() { Company = "Centro comercial Moctezuma", Contact = "Francisco Chang", Country = "Mexico"}
            };

            ViewData.Model = employees;

            using (StringWriter stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "EmployeeListPartialViewModel");
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }
        public ActionResult Contact(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                var mailFrom = contactForm.Email;
                var mailTo = "tanvirarjel@hotmail.com"; // To which you want to send the mail
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(mailTo);
                    mail.From = new MailAddress(mailFrom,"Form Site Contact Form");
                    mail.Subject = contactForm.Subject;
                    mail.Body = "Hello I am " + contactForm.Name +"," + "<br/><br/>" + contactForm.Message;
                    mail.ReplyToList.Add(new MailAddress(contactForm.Email, contactForm.Name));

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
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential("Your email", "your email pass");
                        smtp.Port = 587; //This for gmail

                        smtp.Send(mail);

                        ModelState.Clear();
                        ViewBag.SendingStatus = "Sent";
                    }

                }
            }
            return View();
        }

        public ActionResult SendEmailWithHtmlTemplate()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>()
            {
                new EmployeeViewModel() { Company = "Alfreds Futterkiste", Contact = "Maria Anders", Country = "Germany"},
                new EmployeeViewModel() { Company = "Centro comercial Moctezuma", Contact = "Francisco Chang", Country = "Mexico"}
            };

            ViewBag.EmployeeList = employees;
            return View();
        }

        [HttpPost]
        public ActionResult SendEmailWithHtmlTemplate(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                var mailFrom = contactForm.Email;
                var mailTo = "tanvirarjel@hotmail.com"; // To which you want to send the mail
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(mailTo);
                    mail.From = new MailAddress(mailFrom, "Form Site Contact Form");
                    mail.Subject = contactForm.Subject;
                    mail.Body = GetEmailTemplate();
                    //mail.Body = "Hello I am " + contactForm.Name +"," + "<br/><br/>" + contactForm.Message;
                    mail.ReplyToList.Add(new MailAddress(contactForm.Email, contactForm.Name));

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
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential("Your email", "your email pass");
                        smtp.Port = 587; //This for gmail

                        smtp.Send(mail);

                        ModelState.Clear();
                        ViewBag.SendingStatus = "Sent";
                    }

                }
            }

            List<EmployeeViewModel> employees = new List<EmployeeViewModel>()
            {
                new EmployeeViewModel() { Company = "Alfreds Futterkiste", Contact = "Maria Anders", Country = "Germany"},
                new EmployeeViewModel() { Company = "Centro comercial Moctezuma", Contact = "Francisco Chang", Country = "Mexico"}
            };

            ViewBag.EmployeeList = employees;
            ContactForm form = new ContactForm();
            return View(form);
        }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SendingEmailInAspNetMVC.Models
{
    public class ContactForm
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please Enter a valid Email Addrees")]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        
        [Required]
        [Display(Name ="Attachment")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile  { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

    }
}
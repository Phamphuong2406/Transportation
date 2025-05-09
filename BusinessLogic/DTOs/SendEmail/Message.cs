﻿using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SendEmail
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject {  get; set; }
        public string Content {  get; set; }
        public IFormFileCollection Attachments { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)//IFormFileCollection Attachments)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("",x)));
         //   Attachments = Attachments;
            Subject = subject;
            Content = content;

        }
    }
}

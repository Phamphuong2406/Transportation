using BusinessLogic.DTOs.SendEmail;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        public  SendEmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpGet("SendTestEmail")]
        public IActionResult  SendTestEmail()
        {
            var message = new Message(new string[] { "phuonguria123@gmail.com" }, "Test email", "This iss content");
            _emailSender.SendEmail(message);
            return Ok();
        }
     /*   [HttpPost]
        public async  Task<IActionResult> SendTestEmailAsync()
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            var message = new MessageAsync(new string[] { "phuonguria123@gmail.com" }, "Test email with attachment", "This iss content", files);

            _emailSender.SendEmailAsync(message);
            return Ok();
        }*/
    }
}

  


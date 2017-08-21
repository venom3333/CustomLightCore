

namespace CustomLightCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.ViewModels.Mail;

    using MailKit.Net.Smtp;

    using Microsoft.AspNetCore.Mvc;

    using MimeKit;

    /// <summary>
    /// The mail controller.
    /// </summary>
    public class MailController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailController"/> class.
        /// </summary>
        public MailController()
        {
            this.FromAddress = "sanitarium667@gmail.com";
            this.FromAdressTitle = "����������� ���! (From Address)";
            this.ToAddress = "sanitarium667@gmail.com";
            this.ToAdressTitle = "����������� ���! (To Address)";
        }

        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        private string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the from adress title.
        /// </summary>
        private string FromAdressTitle { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        private string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the to adress title.
        /// </summary>
        private string ToAdressTitle { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        private string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body content.
        /// </summary>
        private string BodyContent { get; set; }

        /// <summary>
        /// The call back mail.
        /// </summary>
        /// <param name="callBackForm">
        /// The call Back Form.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        public IActionResult CallBackMail(CallBackMailViewModel callBackForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.Subject = "������� �����������!";

                    this.BodyContent = string.Format(
                        @"
                    ������� �����������!
                    ���: {0}
                    �������: {1}
                    ����������: {2}",
                        callBackForm.Name,
                        callBackForm.Phone,
                        callBackForm.Misc);

                    // Smtp Server 
                    const string SmtpServer = "smtp.gmail.com";

                    // Smtp Port Number 
                    const int SmtpPortNumber = 587;

                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                    mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                    mimeMessage.Subject = Subject;
                    mimeMessage.Body = new TextPart("plain") { Text = BodyContent };

                    using (var client = new SmtpClient())
                    {

                        client.Connect(SmtpServer, SmtpPortNumber, false);

                        // Note: only needed if the SMTP server requires authentication 
                        // Error 5.5.1 Authentication  
                        client.Authenticate(FromAddress, "aeseJ1zZ");
                        client.Send(mimeMessage);
                        client.Disconnect(true);
                    }

                    return this.PartialView("_CallBackMail");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return this.PartialView("_CallBackMail");
        }

        /// <summary>
        /// The order mail.
        /// </summary>
        /// <param name="orderForm">
        /// The order Form.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        /// TODO: �������� ���� �� Cart!
        [HttpPost]
        public IActionResult OrderMail(OrderMailViewModel orderForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.Subject = "�����!";

                    this.BodyContent = $@"
                    �����!
                    ���: {orderForm.Name}
                    �������: {orderForm.Phone}
                    Email: {orderForm.Email}
                    �����: {orderForm.Address}
                    ��� ��������: {orderForm.DeliveryType}
                    ����������: {orderForm.Misc}";

                    // Smtp Server 
                    const string SmtpServer = "smtp.gmail.com";

                    // Smtp Port Number 
                    const int SmtpPortNumber = 587;

                    var mimeMessage = new MimeMessage();

                    mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                    mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                    mimeMessage.Subject = Subject;
                    mimeMessage.Body = new TextPart("plain") { Text = BodyContent };

                    using (var client = new SmtpClient())
                    {

                        client.Connect(SmtpServer, SmtpPortNumber, false);

                        // Note: only needed if the SMTP server requires authentication 
                        // Error 5.5.1 Authentication  
                        client.Authenticate(FromAddress, "aeseJ1zZ");
                        client.Send(mimeMessage);
                        client.Disconnect(true);
                    }

                    return this.PartialView("~/Views/Cart/_OrderForm.cshtml");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return this.PartialView("~/Views/Cart/_OrderForm.cshtml");
        }
    }
}
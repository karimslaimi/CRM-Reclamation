using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;
using Nexmo.Api.Request;
using Nexmo.Api;

namespace PFE_reclamation.Services {
    public class ApiService {

        public String sendmail(string mails, string obj, string body) {
            try {
                string sendermail = System.Configuration.ConfigurationManager.AppSettings["SenderMail"].ToString();
                string senderpassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 1000000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                MailMessage mailMessage = new MailMessage();


                mailMessage.To.Add(mails);



                mailMessage.Body = body;

                client.Credentials = new NetworkCredential(sendermail, senderpassword);

                mailMessage.IsBodyHtml = true;

                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);

                } catch (Exception) {

                return "error occured";

                }
            return "success";
            }

        public void sendSMS(string body, string phone) {


            var NEXMO_API_KEY = "e5668206";
            var NEXMO_API_SECRET = "P4VhyGeOyadg8AVr";

            var credentials = Credentials.FromApiKeyAndSecret(
                NEXMO_API_KEY,
                NEXMO_API_SECRET
                );

            var nexmoClient = new NexmoClient(credentials);

            var response = nexmoClient.SmsClient.SendAnSms(new Nexmo.Api.Messaging.SendSmsRequest() {
                To = phone,
                From = "CRM RECLAM",
                Text = body
                });
            Console.WriteLine(response.Messages[0].To);



            }
        }
    }
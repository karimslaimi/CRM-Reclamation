
using System;

using System.Net.Mail;
using System.Net;
using System.Text;
using Nexmo.Api.Request;
using Nexmo.Api;
using System.Net.Http;
using System.Collections.Generic;
using RestSharp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Http.Headers;

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
                string from = sendermail;

                MailMessage mailMessage = new MailMessage(from, mails);



                mailMessage.Subject = obj;
                mailMessage.Body = body;

                client.Credentials = new NetworkCredential(sendermail, senderpassword);

                mailMessage.IsBodyHtml = true;

                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);

                } catch (Exception e) {

                return "error occured";

                }
            return "success";
            }

        public void sendSMS(string body, string phone) {


            try {




                var client = new RestClient("https://rest.nexmo.com/sms/json?api_key=e5668206&api_secret=P4VhyGeOyadg8AVr&from=Assurance&to=216"+phone+"&text="+Uri.EscapeUriString(body)+"");
                var request = new RestRequest();
             

                request.Method = Method.POST;
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response);
                } catch(Exception e) {
                Console.WriteLine(e);
                }





    }
        }
    }
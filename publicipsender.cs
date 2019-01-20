using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace publicipsender
{
    public class Program
    {
        public static var destinationAddress = string.empty; // define here the recipient's mail
        public static var senderAddress = string.empty; // define mail adress here
        public static var password = string.empty; // define mail password here
        public static var mailSubject = string.empty; // define here mail subject
        public static var mailBody = string.empty; // define here mail body


        private static void Main(string[] args)
        {
            var publicIpList = new List<string> {"0"};
            var timer = new Timer(e => { Externalip(publicIpList); }, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));
            Console.ReadLine();
        }

        private static void Externalip(IList<string> publicIpList)
        {
            var externalip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");
            var lastpublicip = publicIpList[publicIpList.Count - 1];
            publicIpList.Add(externalip);
            if (lastpublicip != externalip)
                Sendmail(externalip , senderAddress , destinationAddress );
        }

        private static void Sendmail(string newPublicIpAddress, string fromAddress, string toAdress)
        {
            var mail = new MailMessage(fromAddress, toAdress);
            var client = new SmtpClient
            {
                Port = 587, //port 465 is deprecated, required to use port 587
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = string.empty, // define here your smtp host address ex:"smtp.gmail.com"
                Credentials = new NetworkCredential(mailAddress, password),
                EnableSsl = true //necessary for STARTTLS command
            };
            mail.Subject = mailSubject;
            mail.Body = mailBody + " " + newPublicIpAddress;
            client.Send(mail);
        }
    }
}
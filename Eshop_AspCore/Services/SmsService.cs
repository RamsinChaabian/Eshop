using Eshop_AspCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Services
{
    public class SmsService
    {
        public static bool SendSms(string lineNumber, string MobileNo, string Message)
        {
            SmsIrRestful.Token tokenInstant = new SmsIrRestful.Token();

            ApplicationDbContext database = new ApplicationDbContext();

            var q = database.Tbl_SettingSite.FirstOrDefault();

            var token = tokenInstant.GetToken(q.SmsApiService, q.SmsSecretKey);

            SmsIrRestful.MessageSend messageInstant = new SmsIrRestful.MessageSend();

            var res = messageInstant.Send(token, new SmsIrRestful.MessageSendObject()
            {
                CanContinueInCaseOfError = false,
                LineNumber = lineNumber,
                Messages = new List<string> { Message }.ToArray(),
                MobileNumbers = new List<string> { MobileNo }.ToArray(),
                SendDateTime=DateTime.Now,
            });

            if (res.IsSuccessful == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SendSmsForNewsLetter(string lineNumber, string[] MobileNo, string Message)
        {
            SmsIrRestful.Token tokenInstant = new SmsIrRestful.Token();

            ApplicationDbContext database = new ApplicationDbContext();

            var q = database.Tbl_SettingSite.FirstOrDefault();

            var token = tokenInstant.GetToken(q.SmsApiService, q.SmsSecretKey);

            SmsIrRestful.MessageSend messageInstant = new SmsIrRestful.MessageSend();

            var res = messageInstant.Send(token, new SmsIrRestful.MessageSendObject()
            {
                CanContinueInCaseOfError = false,
                LineNumber = lineNumber,
                Messages = new List<string> { Message }.ToArray(),
                MobileNumbers = MobileNo,
                SendDateTime = DateTime.Now,
            });

            if (res.IsSuccessful == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


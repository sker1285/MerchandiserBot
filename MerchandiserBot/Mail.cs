using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace MerchandiserBot
{
    /// <summary>
    /// 發email物件
    /// </summary>
    public class Mail
    {

        public static string MsgCode { get; set; }
        public static string Message { get; set; }
        public static string strUserMail { get; set; }
        public static string otp { get; set; }

        /// <summary>
        /// 發送OTP email給客戶
        /// </summary>
        /// <param name="strUserMail">收件者</param>
        /// <param name="otp">動態密碼</param>
        /// <returns></returns>
        public static void SendMail()
        {
            try
            {
                PwdChange.wsPwdChangeSoapClient ws = new PwdChange.wsPwdChangeSoapClient();
                var result = ws.userPwdChangeFO("EK3730", otp, "修改密碼");
               





                MailMessage mailMsg = new MailMessage();
                mailMsg.IsBodyHtml = true;
                mailMsg.SubjectEncoding = Encoding.UTF8;
                mailMsg.BodyEncoding = Encoding.UTF8;
                mailMsg.Subject = "【新光人壽】您的動態密碼";
                mailMsg.To.Add(new MailAddress("sker1285@skl.com.tw"));


                mailMsg.Body = "【新光人壽】 OTP動態密碼為：" + otp + "，請於有效時間內輸入。如有任何疑問，請洽新光人壽客服免付費電話0800-031-115";

                mailMsg.From = new MailAddress("sklcustadmin@skl.com.tw");

                SmtpClient smtp = new SmtpClient();

                smtp.Send(mailMsg);
                smtp.Dispose();
            }
            catch (Exception ex)
            {

                Message = ex.ToString();
                MsgCode = "N";
                return;// false;
            }

            Message = "郵件寄送成功";
            MsgCode = "Y";
            return;// true;
        }

    }
   
}
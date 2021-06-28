using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.Utility
{
   public class Utilities
    {
        public void SendEmail(string from, string to, string cc, string bcc, string smtp, string password, int portNo, string subject, string bodyContent)
        {
            try
            {
                var mailMsg = new MailMessage();
                mailMsg.To.Add(to);
                if (!string.IsNullOrEmpty(cc))
                {
                    string[] ccIds = cc.Split(';');
                    foreach (string ccEmail in ccIds)
                    {
                        mailMsg.CC.Add(new MailAddress(ccEmail));
                    }
                }
                 
                if (!string.IsNullOrEmpty(bcc))
                {
                    string[] bccIds = bcc.Split(';');
                    foreach (string bm in bccIds)
                    {
                        mailMsg.Bcc.Add(new MailAddress(bm));
                    }
                }
                mailMsg.From = new MailAddress(from);
                mailMsg.Subject = subject;
                mailMsg.Body = bodyContent;
                mailMsg.IsBodyHtml = true;

                var smtpServer = new SmtpClient(smtp)
                {
                    Port = portNo,
                    Credentials = new NetworkCredential(from, password),
                    EnableSsl = false
                };
                smtpServer.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("BL:Send Mail Error: - " + ex.Message);
            }
        }

        /// <summary>
        /// This function will create datewise error logfile in application root.
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="sErrMsg"></param>
        /// <param name="preFixFileName"></param>
        public void CreateLogFiles(string logPath, string sErrMsg, string preFixFileName = "")
        {
            string sLogFormat;
            string sErrorTime;
            string logsDirectory = logPath;
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string sPathName = logsDirectory + "\\" + (string.IsNullOrEmpty(preFixFileName) ? "" : preFixFileName) + "log{0}.txt";
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
            StreamWriter sw = new StreamWriter(string.Format(sPathName, sErrorTime), true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
        /// <summary>
        ///     Methos for encrypting text
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public string Encrypt(string toEncrypt)
        {
            //var settingsReader = new AppSettingsReader();
            //var encryptionKey = (string)settingsReader.GetValue("SecurityKey", typeof(string));
            var encryptionKey = "Dynax@2019";
            var clearBytes = Encoding.Unicode.GetBytes(toEncrypt);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return toEncrypt;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    toEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }

            return toEncrypt;
        }

        /// <summary>
        ///     Decrypt encrypted string
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns>String</returns>
        public string Decrypt(string cipherText)
        {
            // var settingsReader = new AppSettingsReader();
            // var encryptionKey = (string)settingsReader.GetValue("SecurityKey", typeof(string));
            var encryptionKey = "Dynax@2019";
            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return cipherText;

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }



    }
}

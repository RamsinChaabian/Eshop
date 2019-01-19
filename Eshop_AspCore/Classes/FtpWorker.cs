using Eshop_AspCore.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eshop_AspCore.Classes
{
    public class FtpWorker
    {
        private FtpParametr GetFtp(int ServerID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var q = db.Tbl_Server.Where(a => a.ServerId == ServerID).Single();

            FtpParametr f = new FtpParametr()
            {
                FtpAddress = q.IP + q.Path,
                Password = q.FtpPassword,
                UserName = q.FtpUsername
            };

            return f;
        }
        private FtpParametr GetFtp(string TypeFtp)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var q = db.Tbl_Server.Where(a => a.Type == TypeFtp).ToList();
            int RndServer = new Random().Next(0, q.Count() - 1);

            FtpParametr f = new FtpParametr()
            {
                FtpAddress = q[RndServer].IP + q[RndServer].Path,
                Password = q[RndServer].FtpPassword,
                UserName = q[RndServer].FtpUsername,
                FtpID = q[RndServer].ServerId
            };

            return f;
        }

        public int Upload(string TypeFtp, string FileName, Stream MyFile)
        {
            var qP = GetFtp(TypeFtp);

            /* Create an FTP Request */
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(qP.FtpAddress + FileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(qP.UserName, qP.Password);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            /* Establish Return Communication with the FTP Server */
            Stream ftpStream = ftpRequest.GetRequestStream();
            /* Open a File Stream to Read the File for Upload */
            Stream localFileStream = MyFile;
            /* Buffer for the Downloaded Data */
            byte[] byteBuffer = new byte[2048];
            int bytesSent = localFileStream.Read(byteBuffer, 0, 2048);
            /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */

            while (bytesSent != 0)
            {
                ftpStream.Write(byteBuffer, 0, bytesSent);
                bytesSent = localFileStream.Read(byteBuffer, 0, 2048);
            }

            /* Resource Cleanup */
            localFileStream.Close();
            ftpStream.Close();
            ftpRequest = null;

            return qP.FtpID;
        }

        public bool Remove(int ServerID, string FileName)
        {
            try
            {
                var qP = GetFtp(ServerID);

                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(qP.FtpAddress + FileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(qP.UserName, qP.Password);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }


    public class FtpParametr
    {
        public int FtpID { get; set; }
        public string FtpAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}


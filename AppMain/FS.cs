using System;
using System.IO;
using System.Text;
using AppMain.Interface;
using Ionic.Zip;

namespace AppMain
{
    public class FS : IFS
    {
        public string GetDrive()
        {
            string drive = Environment.CurrentDirectory.Substring(0, 1);
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string message = @"Выберите диск для установки программы SmrPro. ";
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "Fixed")
                {
                    message += d.Name.Substring(0,1) + @" или ";
                }
            }
            message = message.Substring(0, message.Length - 5);
            Console.WriteLine(message);
            drive = Console.ReadLine();
            drive = drive.ToUpper().Substring(0, 1);
            if (!Directory.Exists(drive + ":" + Path.DirectorySeparatorChar))
            {
                Console.WriteLine("Диска не существует. Выберит другой диск");
                return GetDrive();
            }

            return drive;
        }
        public void UnZip(string file, string password, string dir)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding win1251 = Encoding.GetEncoding(866);
            ZipFile zf = new ZipFile(file, win1251);
            zf.Password = password;
            zf.ExtractAll(dir);
        }
        public void UnRar(string file, string password, string dir)
        {
            return;
        }
    }
}

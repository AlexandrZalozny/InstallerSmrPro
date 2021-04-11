using AppMain.Interface;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AppMain
{
    public class Installer : IInstaller
    {
        public string Drive { get; set; }
        public string Dir { get; set; }
        public string Ext { get; set; }
        public string Link { get; set; }
        public string Password { get; set; }
        public string FileZip { get; set; }
        public static WebClient WebClient { get; set; }
        public static EventWaitHandle handle = new AutoResetEvent(false);
        public static FS Fs { get; set; }

        public Installer(string link, string dir, string ext)
        {
            Fs = new FS();
            SetDrive();
            Password = SetPassword();
            WebClient = new WebClient();
            Link = link;
            Dir = dir;
            Ext = ext;
        }

        public void Install()
        {
            SetDir();
            SetFileZip();
            DownloadFile();
            UnZip();
            Console.WriteLine("Установка завершена");
            Console.ReadKey();

        }

        protected void SetDir()
        {
            Dir = Drive + @":\" + Dir;
        }

        protected void SetFileZip()
        {
            Random rnd = new Random();
            FileZip = Drive + @":\" + rnd.Next(0, 999999).ToString() + ".dat";
        }

        protected void SetDrive()
        {
            Drive = Fs.GetDrive();
            Console.WriteLine(@"Программа будет установлена на диск {0}", Drive);
        }

        protected string SetPassword()
        {
            Console.WriteLine(@"Введите пароль");

            string pass = String.Empty;
            ConsoleKey key;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }

        protected void UnZip()
        {
            if (!File.Exists(FileZip))
            {
                Console.WriteLine("Файл не существует. Установка отменена");
                return;
            }
            if (Directory.Exists(Dir))
            {
                Console.WriteLine("Каталог с програмой существут. Установка отменена");
                return;
            }
            //FileInfo fi = new FileInfo(FileZip);
            //string ext = fi.Extension.ToLower();
            
            try
            {
                if (Ext == "zip")
                {
                    Fs.UnZip(FileZip, Password, Drive + @":\");
                }
                else if(Ext == "rar")
                {
                    Fs.UnRar(FileZip, Password, Drive + @":\");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected void DownloadFile()
        {
            try
            {
                if (File.Exists(FileZip))
                {
                    throw new Exception("Загрузка не возможна. Файл существует. Нажмите Enter чтобы продолжить или удалите файл и нажмите любую другую клавишу");
                }
                Task.Factory.StartNew(() => { DownloadFileFromURL(Link, FileZip); });
                handle.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    DownloadFile();
                }
            }

            Console.WriteLine("Распаковка");
        }

        protected void DownloadFileFromURL(string link, string file)
        {
            Console.WriteLine("Загрузка начата");
            WebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
            WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            WebClient.DownloadFileAsync(new Uri(link), file);
        }

        protected static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Загрузка файла отменена");
            }

            if (e.Error != null)
            {
                Console.WriteLine(e.Error.ToString());
            }
            handle.Set();
        }

        protected static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write("\rЗагружено: {0} of {1} bytes. {2} % выполнено ...", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
        }




    }
}

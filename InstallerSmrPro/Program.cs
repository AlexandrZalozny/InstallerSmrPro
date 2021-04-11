using AppMain;
using System;

namespace InstallerSmrPro
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Установка SmrPro");
            Installer installerSmrPro = new Installer(@"http://smr-pro.by/support/smrpro.zip", @"SmrPro", "zip");
            installerSmrPro.Install();
            Console.WriteLine("Установка WSMR2016");
            Console.WriteLine("Установить WSMR2016 [Y/N]");
            string a = Console.ReadLine().Substring(0,1).ToLower();
            if (a == "y")
            {
                Installer installerWSMR = new Installer(@"http://smr-pro.by/support/Wsmr2016.rar", @"Wsmr2016", "rar");
                installerWSMR.Install();
            }
        }
    }
}

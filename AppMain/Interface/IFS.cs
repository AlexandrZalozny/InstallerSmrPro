using System;
using System.Collections.Generic;
using System.Text;

namespace AppMain.Interface
{
    interface IFS
    {
        string GetDrive();
        void UnRar(string file, string password, string dir);
        void UnZip(string file, string password, string dir);
    }
}
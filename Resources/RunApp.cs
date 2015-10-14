using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Web;
using System.Reflection;

namespace Play9GamePackBasic
{
    class RunApp
    {
        public Process content;
        private string contentPath;
        private string appID;
        private Thread launchThread;

        public event ExitHandler Exited;
        public delegate void ExitHandler(object sender, EventArgs e);

        public event LoadHandler Loaded;
        public delegate void LoadHandler();

        private void content_Exited(object sender, EventArgs e)
        {
            Exited(sender, e);
        }

        private void content_Loaded()
        {
            Loaded();
        }

        public RunApp()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Exception ex = ((Exception)eventArgs.ExceptionObject);
                Environment.Exit(1);
            };
        }

        public void setCurrentContent(string id, string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            stopMonitoring();
            appID = id;
            contentPath = path;
            startMonitoring();        
        }

        public void startMonitoring()
        {
            launchThread = new Thread(new ThreadStart(launch));
            launchThread.Start();
        }

        public void stopMonitoring()
        {
            if (isNotMonitoring())
                return;

            launchThread.Abort();
            launchThread.Join();

            //content.Kill();
            try
            {
                if (!content.CloseMainWindow())
                {
                    Console.WriteLine("Could not Close Main Window. Killing content instead");
                    content.Kill();
                }
                else
                {
                    if (!content.WaitForExit(10000))
                    {
                        Console.WriteLine("Content took too long to close. Killing content instead");
                        content.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The content process could not be terminated properly: {0}", e.Message);
            }

            content = null;
        }

        private bool isNotMonitoring()
        { 
            return (null == launchThread)
                || (null == content);
        }

        internal void launch()
        {
            ProcessStartInfo contentInfo = new ProcessStartInfo();

            contentInfo.FileName = contentPath;
            contentInfo.WorkingDirectory = Path.GetDirectoryName(contentPath);

            content = Process.Start(contentInfo);
            //content.WaitForExit();
            content.EnableRaisingEvents = true;
            
            content.Exited += content_Exited;
            //Splasher.Show();
            while (!content.Responding)
            {
                content_Loaded();
                //Splasher.Close();
            }
        }
    }
}

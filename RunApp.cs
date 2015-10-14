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

using firefighter_logger;
using firefighter_web;
using firefighter_licensing;

namespace watchdog
{
    class ScreenCapture
    {
        private static System.Drawing.Bitmap CaptureScreen()
        {
            var desktopSize = System.Windows.Forms.SystemInformation.VirtualScreen.Size;
            var bitmap = new Bitmap(desktopSize.Width, desktopSize.Height);

            Graphics.FromImage(bitmap).CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size, CopyPixelOperation.SourceCopy);

            return bitmap;
        }

        private static Bitmap CaptureScreen(float scale)
        {
            var bitmap = CaptureScreen();
            var bitmapSize = new Size((int)(bitmap.Width * scale), (int)(bitmap.Height * scale));
            return new Bitmap(bitmap, bitmapSize);
        }

        private static byte[] JPEGEncode(Bitmap bitmap, uint quality)
        {
            var encoder = System.Array.Find(ImageCodecInfo.GetImageEncoders(), element => element.MimeType == "image/jpeg");

            var encoderParams = new EncoderParameters
            {
                Param = new EncoderParameter[]
                {
                    new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Math.Min(quality, 100U)),
                }
            };

            var stream = new System.IO.MemoryStream();
            bitmap.Save(stream, encoder, encoderParams);
            return stream.ToArray();
        }

        public static string CaptureScreenToJPEGBase64(uint quality, float scale)
        {
            return System.Convert.ToBase64String(JPEGEncode(CaptureScreen(scale), quality));
        }
    }

    class watchdog
    {
        private Process content;
        private string appconfigHash;
        private string contentPath;
        private string appID;
        private Heartbeat heartbeat;
        private Thread launchThread;
        private Thread heartbeatThread;
        private NamedPipeServerStream schedulerPipe;
        private NamedPipeServerStream contentScreengrabPipe;
        private Timer screenGrabTimer;

        public watchdog()
        {
            Logger.Initialise("com.imtkm9.firefighter.watchdog", LoggerComponent.Watchdog);

            appconfigHash = String.Empty;

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Exception ex = ((Exception)eventArgs.ExceptionObject);
                Logger.TraceError(
                    "Unhandled exception: {0}\n{1}",
                    ex.Message,
                    ex.StackTrace);
                Logger.Close();
                Environment.Exit(1);
            };

            Logger.TraceInformation("Version " + getRunningVersion() + " started.");

            firefighter_environment.Environment.DesktopBackgroundColour = System.Drawing.Color.White;

            listenToScheduler();
        }

        public void listenToScheduler()
        {
            while (true)
            {
                schedulerPipe = new NamedPipeServerStream(Properties.Settings.Default.PipeName, PipeDirection.In);
                schedulerPipe.WaitForConnection();
                readFromSchedulerPipe();
                schedulerPipe.Close();
            }
        }

        private void readFromSchedulerPipe()
        {
            try
            {
                StreamReader pipeReader = new StreamReader(schedulerPipe);
                processXML(pipeReader.ReadToEnd());
                
            }
            catch(Exception ex)
            {
                Logger.TraceError(String.Format("Named pipe error. {0}", ex.Message));
            }        
        }

        private void processXML(string xml)
        {
            try
            {
                XElement root = XDocument.Parse(xml).Root;

                string newAppID = root.Element("appID").Value;
                string newExePath = root.Element("exePath").Value;
                string newAppconfigHash = String.Empty;
                //if current app has shared assets, update the app's config file
                if (null != root.Element("shared_xml") && null != root.Element("appconfigPath"))
                {
                    XElement newAppSharedXml = root.Element("shared_xml");
                    updateAppConfig(newAppSharedXml, root.Element("appconfigPath").Value);
                }
                
                if (null != root.Element("appconfigPath"))
                {
                    newAppconfigHash = hashAppConfig(root.Element("appconfigPath").Value);
                }

                try
                {
                    if (!Licensing.isValid())
                    {
                        Logger.TraceWarning("Expired license key. Content not playing.");
                        stopMonitoring();
                        return;
                    }
                }catch
                {
                    Logger.TraceError("Invalid license key, could not decrypt. Content not  playing");
                    stopMonitoring();
                    return;
                }

                if (!isCurrentContent(newAppID, newAppconfigHash) || isNotMonitoring())
                    setCurrentContent(newAppID, newExePath, newAppconfigHash);
            }
            catch (Exception ex)
            {
                Logger.TraceError(String.Format("Watchdog received malformed XML. {0}.xml:{1}", ex.Message,xml));
            }
        }

        private bool isCurrentContent(string id, string hash)
        {
            return appID == id && appconfigHash.Equals(hash);
        }

        private string hashAppConfig(string path)
        {
            try
            {
                var hash = new SHA1CryptoServiceProvider().ComputeHash(File.ReadAllBytes(path));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch (Exception e)
            {
                Logger.TraceWarning("Could not calculate hash of {0} : {1} ", path, e.Message);
                return String.Empty;
            }            
        }

        private void setCurrentContent(string id, string path, string hash)
        {
            if (!File.Exists(path))
            {
                Logger.TraceError("Content not found. {0}", path);
                return;
            }

            stopMonitoring();
            appID = id;
            contentPath = path;
            appconfigHash = hash;
            startMonitoring();        
        }
        //update the current app config with the new scheduled assets
        private void updateAppConfig(XElement sharedElements, string appConfigPath)
        {
            if (File.Exists(appConfigPath))
            {
                XElement root = XDocument.Load(appConfigPath).Root;

                var playlist = root.Element("playlist");
                playlist.Remove();

                XElement newPlaylist = new XElement("playlist");
                
                foreach (XElement sharedElement in sharedElements.Elements())
                {
                    string assetid = sharedElement.Element("assetID").Value;
                    string aws_key = sharedElement.Element("AWSKey").Value;
                    string path = sharedElement.Element("path").Value;
                    string fullpath = System.IO.Path.Combine("../shared/", assetid, path);
                    newPlaylist.Add(new XElement("item", fullpath));
                }

                root.Add(newPlaylist);
                root.Save(appConfigPath);
            }
            else
                Console.WriteLine("The file {0} could not be located while updating appconfig", appConfigPath);

        }

        private void startMonitoring()
        {
            heartbeat = new Heartbeat();

            launchThread = new Thread(new ThreadStart(launch));
            heartbeatThread = new Thread(new ThreadStart(heartbeat.run));
            listenToContentScreengrab();

            launchThread.Start();

            screenGrabTimer = new Timer(
                new TimerCallback((obj) => new Thread(new ThreadStart(this.sendScheduleEvent)).Start()), 
                null, 
                Properties.Settings.Default.ScreenCaptureDelay, 
                Timeout.Infinite);

            Thread.Sleep(Properties.Settings.Default.HeartbeatCheckDelay);
            heartbeatThread.Start();
        }

        private void stopMonitoring()
        {
            if (isNotMonitoring())
                return;

            launchThread.Abort();
            heartbeatThread.Abort();

            launchThread.Join();
            heartbeatThread.Join();

            //content.Kill();
            try
            {
                if (!content.CloseMainWindow())
                {
                    Logger.TraceWarning("Could not Close Main Window. Killing content instead");
                    content.Kill();
                }
                else
                {
                    if (!content.WaitForExit(10000))
                    {
                        Logger.TraceWarning("Content took too long to close. Killing content instead");
                        content.Kill();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.TraceError("The content process could not be terminated properly: {0}", e.Message);
            }

            content = null;
        }

        private bool isNotMonitoring()
        { 
            return (null == launchThread) 
                || (null == heartbeatThread) 
                || (null == content);
        }

        internal void launch()
        {
            ProcessStartInfo contentInfo = new ProcessStartInfo();

            contentInfo.FileName = contentPath;
            contentInfo.WorkingDirectory = Path.GetDirectoryName(contentPath);

            while (true)
            {
                content = Process.Start(contentInfo);
                heartbeat.contentToWatch = content;
                content.WaitForExit();
            }
        }

        void listenToContentScreengrab()
        {
            try
            {
                contentScreengrabPipe = new NamedPipeServerStream("ContentToWatchdog", PipeDirection.In,1,PipeTransmissionMode.Message,PipeOptions.Asynchronous);

                contentScreengrabPipe.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), contentScreengrabPipe);//non-blocking
            }
            catch(Exception e)
            {
                Logger.TraceError("Could not open screengrab pipe: {0}", e.Message);
            }
            
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
            screenGrabTimer.Change(Timeout.Infinite, Timeout.Infinite); //disable screencapture from IM
            try
            {
                pipeServer.EndWaitForConnection(iar);
                using (StreamReader sr = new StreamReader(pipeServer))
                {
                    string base64EncodedImage;
                    while ((base64EncodedImage = sr.ReadToEnd()) != null)
                    {
                        Web.schedule_event(appID, base64EncodedImage);    //send

                        break;
                    }
                }
                pipeServer.Close();

            }
            catch
            {
                //connection was closed by sendScheduleEvent()
            }
        }

        internal void sendScheduleEvent()
        {
            try
            {
                contentScreengrabPipe.Close();//will throw an exception (caught)
                Web.schedule_event(appID, ScreenCapture.CaptureScreenToJPEGBase64(10, 0.3f));
            }
            catch (Exception e)
            {
                Logger.TraceError("Schedule event web method failed: {0}", e.Message);
            }
        }

        private int getRunningVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.Major;
        }
    }
}

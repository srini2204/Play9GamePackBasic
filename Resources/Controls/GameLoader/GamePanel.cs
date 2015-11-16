using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Play9GamePackBasic.Resources.Controls.GamePanel
{
    class GamePanel : System.Windows.Forms.Panel
	{

        public event EventHandler ProcessLoaded;
        public event EventHandler ProcessDestroyed;

		/// <summary>
		/// Track if the application has been created
		/// </summary>
		public bool createdEXE = false;

		/// <summary>
		/// Handle to the application Window
		/// </summary>
		IntPtr appWin;

		/// <summary>
		/// The name of the exe to launch
		/// </summary>
		private string exeName = "";

		/// <summary>
		/// Get/Set if we draw the tick marks
		/// </summary>
		[Category("Custom")]
		[Description("Game Path")]
		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
		public string ExeName
		{
			get
			{
				return exeName;
			}
			set
			{
				exeName = value;				
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
        public GamePanel()
		{
			Visible = false;
		}
		
		[DllImport("user32.dll", EntryPoint="GetWindowThreadProcessId",  SetLastError=true,
			 CharSet=CharSet.Unicode, ExactSpelling=true,
			 CallingConvention=CallingConvention.StdCall)]
		private static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId); 
			
		[DllImport("user32.dll", SetLastError=true)]
		private static extern IntPtr FindWindow (string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError=true)]
		private static extern long SetParent (IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", EntryPoint="GetWindowLongA", SetLastError=true)]
		private static extern long GetWindowLong (IntPtr hwnd, int nIndex);

		[DllImport("user32.dll", EntryPoint="SetWindowLongA", SetLastError=true)]
		private static extern long SetWindowLong (IntPtr hwnd, int nIndex, long dwNewLong);

		[DllImport("user32.dll", SetLastError=true)]
		private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);
		
		[DllImport("user32.dll", SetLastError=true)]
		private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
		
		[DllImport("user32.dll", EntryPoint="PostMessageA", SetLastError=true)]		
		private static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);
		
		private const int SWP_NOOWNERZORDER = 0x200;
		private const int SWP_NOREDRAW = 0x8;
		private const int SWP_NOZORDER = 0x4;
		private const int SWP_SHOWWINDOW = 0x0040;
		private const int WS_EX_MDICHILD = 0x40;
		private const int SWP_FRAMECHANGED = 0x20;
		private const int SWP_NOACTIVATE = 0x10;
		private const int SWP_ASYNCWINDOWPOS = 0x4000;
		private const int SWP_NOMOVE = 0x2;
		private const int SWP_NOSIZE = 0x1;
		private const int GWL_STYLE = (-16);
		private const int WS_VISIBLE = 0x10000000;
		private const int WM_CLOSE = 0x10;
		private const int WS_CHILD = 0x40000000;

		private Process p;
		
		/// <summary>
		/// Force redraw of control when size changes
		/// </summary>
		/// <param name="e">Not used</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.Invalidate();
			base.OnSizeChanged (e);
		}


		/// <summary>
		/// Create control when visibility changes
		/// </summary>
		/// <param name="e">Not used</param>
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged (e);
		}

	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			// Stop the application
			if (appWin != IntPtr.Zero)
			{

				// Post a close message
				PostMessage(appWin, WM_CLOSE, 0, 0);

				// Delay for it to get the message
				System.Threading.Thread.Sleep(1000);

                try
                {
                    p.Dispose();
                    //p.Close();
                    p = null;
                }
                catch (Exception)
                {
                    
                }
				// Clear internal handle
				appWin = IntPtr.Zero;

				createdEXE = false;

			}

			base.OnHandleDestroyed (e);
		}


		/// <summary>
		/// Update display of the executable
		/// </summary>
		/// <param name="e">Not used</param>
		protected override void OnResize(EventArgs e)
		{
			if (this.appWin != IntPtr.Zero)
			{
				MoveWindow(appWin, 0, 0, this.Width, this.Height, true);
			}
			base.OnResize (e);
		}

		public void launchEXE()
		{
			if (!createdEXE)
			{
				// Initialize handle value to invalid
				appWin = IntPtr.Zero;

				// Start the remote application
                p = null;
				try
				{
                    ProcessStartInfo contentInfo = new ProcessStartInfo();

                    contentInfo.FileName = this.exeName;
                    contentInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(this.exeName);

                    contentInfo.UseShellExecute = true;
                    contentInfo.CreateNoWindow = true;
                    contentInfo.WindowStyle = ProcessWindowStyle.Normal;
                    contentInfo.RedirectStandardInput = false;
                    contentInfo.RedirectStandardOutput = false;
                    contentInfo.RedirectStandardError = false;

					// Start the process
                    p = System.Diagnostics.Process.Start(contentInfo);

					// Wait for process to be created and enter idle condition
					p.WaitForInputIdle();
                    p.Exited +=p_Exited;

					// Get the main handle
					appWin = p.MainWindowHandle;

					// Mark that control is created
					createdEXE = true;

					// Put it into this form
					SetParent(appWin, this.Handle);

					// Remove border and whatnot
					SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE);

					// Move the window to overlay it on this window
					MoveWindow(appWin, 0, 0, this.Width, this.Height, true);

                    while (!p.Responding)
                    {
                        
                    }
                    Process_Loaded();
				}
				catch (Exception ex)
				{
                    Console.WriteLine("Error : " + ex.Message);
					//MessageBox.Show(this, ex.Message, "Error Loading game");
				}
			}
		}

        private void p_Exited(object sender, EventArgs e)
        {
            Process_Exited();
        }

        private void Process_Loaded()
        {
            if (this.ProcessLoaded != null)
                this.ProcessLoaded(null, null);
        }

        private void Process_Exited()
        {
            if (this.ProcessDestroyed != null)
                this.ProcessDestroyed(null, null);
        }

        public void disposeEXE()
        {
            if (appWin != IntPtr.Zero)
            {

                // Post a close message
                PostMessage(appWin, WM_CLOSE, 0, 0);

                // Delay for it to get the message
                System.Threading.Thread.Sleep(1000);

                try
                {
                    p.CloseMainWindow();
                    p.WaitForExit(1000);
                    p.Dispose();
                    p = null;
                    Process_Exited();
                }
                catch (Exception)
                {

                }
                // Clear internal handle
                appWin = IntPtr.Zero;

                createdEXE = false;
            }
        }
	}
}

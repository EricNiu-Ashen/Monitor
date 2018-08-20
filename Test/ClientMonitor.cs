using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
   internal class ClientMonitor
    {
        public Process client;
        public Point pos;
        public Size size;

        #region GetWindowCapture

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );
        [DllImport("gdi32.dll")]
        private static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );
        #endregion

        public ClientMonitor(Process p)
        {

            client = p;

        }

        public int GetId()
        {
            return client.Id;
        }

        public Bitmap PrintWindow()
        {
            IntPtr hwnd = client.MainWindowHandle;
            Rectangle rc=new Rectangle();
            GetWindowRect(hwnd, ref rc);

            int width = Math.Abs(rc.X - rc.Width);
            int height = Math.Abs(rc.Y - rc.Height);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        public  Image GetWindowCapture()
        {

            IntPtr hWnd = client.MainWindowHandle;

            Rectangle windowRect = new Rectangle();
            GetWindowRect(hWnd, ref windowRect);

            int width = Math.Abs(windowRect.X - windowRect.Width);
            int height = Math.Abs(windowRect.Y - windowRect.Height);


            IntPtr hscrdc = GetWindowDC(hWnd);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);




            pos.X = windowRect.X;
            pos.Y = windowRect.Y;
            size.Width = width;
            size.Height = height;

            hbitmap = CreateCompatibleBitmap(hscrdc, width, height);

            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);

          
          Image bmp = Image.FromHbitmap(hbitmap);

        
            return bmp;
        }

        public void ChangeWindow(Point pos,Size size)
        {
            IntPtr hWnd = client.MainWindowHandle;
            MoveWindow(hWnd, pos.X, pos.Y, size.Width, size.Height, true);
        }
    }
}

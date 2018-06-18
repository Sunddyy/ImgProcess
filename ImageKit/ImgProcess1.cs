using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace ImgPocess
{
    public static class ImgeTool
    {

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string driver, string device, IntPtr res1, IntPtr res2);

        //public void Dispose()
        //{
        //    //调用带参数的Dispose方法，释放托管和非托管资源

        //    m_gScreen.Dispose();
        //    m_image.Dispose();
        //    m_gImage.Dispose();
        //    DeleteObject(m_dcImage);
        //    DeleteObject(m_dcScreen);
        //    //DeleteObject(m_hscrdc);
        //    //DeleteObject(m_hbitmap);
        //    //DeleteObject(m_hmemdc);
        //    m_subtitleBmp.Dispose();


        //    //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
        //    System.GC.SuppressFinalize(this);
        //}

        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, /* dest = source*/
            SRCPAINT = 0x00EE0086, /* dest = source OR dest*/
            SRCAND = 0x008800C6, /* dest = source AND dest*/
            SRCINVERT = 0x00660046, /* dest = source XOR dest*/
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
            PATCOPY = 0x00F00021, /* dest = pattern*/
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
            DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
            BLACKNESS = 0x00000042, /* dest = BLACK*/
            WHITENESS = 0x00FF0062, /* dest = WHITE*/
        };

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
            int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；
        /// </summary>
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。
        /// </returns>

        /// <summary>
        /// 窗口与要获得句柄的窗口之间的关系。
        /// </summary>
        /// 
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        public enum GetWindowCmd : uint
        {
            /// <summary>
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /*GetWindowCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上：
              GW_CHILD
              寻找源窗口的第一个子窗口
              GW_HWNDFIRST
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口
              GW_HWNDLAST
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口
              GW_HWNDNEXT
              为源窗口寻找下一个兄弟窗口
              GW_HWNDPREV
              为源窗口寻找前一个兄弟窗口
              GW_OWNER
              寻找窗口的所有者
         */



        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
         string lpszDriver,         // driver name驱动名
         string lpszDevice,         // device name设备名
         string lpszOutput,         // not used; should be NULL
         IntPtr lpInitData   // optional printer data
         );
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(
         IntPtr hdcDest, // handle to destination DC目标设备的句柄
         int nXDest,   // x-coord of destination upper-left corner目标对象的左上角的X坐标
         int nYDest,   // y-coord of destination upper-left corner目标对象的左上角的Y坐标
         int nWidth,   // width of destination rectangle目标对象的矩形宽度
         int nHeight, // height of destination rectangle目标对象的矩形长度
         IntPtr hdcSrc,   // handle to source DC源设备的句柄
         int nXSrc,    // x-coordinate of source upper-left corner源对象的左上角的X坐标
         int nYSrc,    // y-coordinate of source upper-left corner源对象的左上角的Y坐标
         UInt32 dwRop   // raster operation code光栅的操作值
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );


        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );


        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        static Rectangle m_rect;
        static IntPtr m_dcTmp;
        static Graphics m_gScreen;
        static Bitmap m_image;
        static Graphics m_gImage;
        static IntPtr m_dcImage;
        static IntPtr m_dcScreen;
        private static int m_Threshhold;
        //static IntPtr m_hscrdc;
        static Rectangle m_windowRect = new Rectangle();
        //static IntPtr m_hbitmap;
        //static IntPtr m_hmemdc;
        private static Bitmap m_subtitleBmp;



        [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换  
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标  
        public static extern int GetCursorPos(
            ref POINTAPI lpPoint
        );

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄  
        public static extern IntPtr WindowFromPoint(
            int xPoint,
            int yPoint
        );

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            int hWnd,
            StringBuilder lpString,
            int nMaxCount
        );

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            int hWnd,
            StringBuilder lpString,
            int nMaxCont
        );

        public delegate void delDisplayImg(Bitmap bmp);// 用于调用用户的显示程序
        public static void CaptureDesktop(string sPath)
        {
            Rectangle rect = new Rectangle();
            rect.Width = Screen.PrimaryScreen.Bounds.Width;
            rect.Height = Screen.PrimaryScreen.Bounds.Height;

            IntPtr dcTmp = CreateDC("DISPLAY", "DISPLAY", (IntPtr)null, (IntPtr)null);
            Graphics gScreen = Graphics.FromHdc(dcTmp);
            Bitmap image = new Bitmap((int)(rect.Width), (int)(rect.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gImage = Graphics.FromImage(image);
            IntPtr dcImage = gImage.GetHdc();
            IntPtr dcScreen = gScreen.GetHdc();
            BitBlt(dcImage, 0, 0, (int)(rect.Width), (int)(rect.Height), dcScreen, (int)(rect.Left), (int)(rect.Top), TernaryRasterOperations.SRCCOPY);
            gScreen.ReleaseHdc(dcScreen);
            gImage.ReleaseHdc(dcImage);

            image.Save(sPath);
        }

        public static Bitmap CaptureWindowPart(IntPtr hWind, int x, int y, int w, int h)
        {
            Bitmap winPic = GetWindowCapture(hWind);
            m_subtitleBmp = (Bitmap)getSubImage(winPic, x, y, w, h);
            return m_subtitleBmp;
        }
        #region 从大图中截取一部分图片, from https://www.cnblogs.com/kehaocheng/p/7126690.html
        /// <summary>
        /// 从大图中截取一部分图片
        /// modified by sund 
        /// date:2018-01-31
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static Bitmap getSubImage(Bitmap fromImage, int offsetX, int offsetY, int width, int height)
        {
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height, fromImage.PixelFormat);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, new Rectangle(0, 0, width, height), new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            IntPtr bitPtr;
            bitPtr = bitmap.GetHbitmap();
            Bitmap subImage = Image.FromHbitmap(bitPtr);
            //释放资源   
            ImgPocess.ImgeTool.DeleteObject(bitPtr);
            graphic.Dispose();
            bitmap.Dispose();
            return subImage;

        }
        #endregion


        /// <summary>
        /// 指定窗口截图
        /// </summary>
        /// <param name="handle">窗口句柄. (在windows应用程序中, 从Handle属性获得)</param>
        /// <returns></returns>
        public static Bitmap GetWindowCapture(IntPtr hWind)
        {

            IntPtr hScrdc = GetWindowDC(hWind);
            GetWindowRect(hWind, ref m_windowRect);
            int width = m_windowRect.Right - m_windowRect.Left;
            int height = m_windowRect.Bottom - m_windowRect.Top;

            IntPtr hmemdc = CreateCompatibleDC(hScrdc);
            IntPtr hbitmap = CreateCompatibleBitmap(hScrdc, width, height);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWind, hmemdc, 0);
            Bitmap bmp;
            bmp = Bitmap.FromHbitmap(hbitmap);

            //DeleteObject(hbitmap);// very important code!!! it not do this the memory will be used up soon
            //DeleteDC(hScrdc);//删除用过的对象
            //DeleteDC(hmemdc);//删除用过的对象
            GC.Collect();// important 

            return bmp;
        }
        /// <summary>
        /// 给图片加入文字水印,且设置水印透明度, copied from internet 
        /// </summary>
        /// <param name="destPath">保存地址</param>
        /// <param name="srcPath">源文件地址，如果想覆盖源图片，两个地址参数一定要一样</param>
        /// <param name="text">文字</param>
        /// <param name="font">字体，为空则使用默认，注意，在创建字体时 GraphicsUnit.Pixel </param>
        /// <param name="brush">刷子，为空则使用默认</param>
        /// <param name="pos">设置水印位置，1左上，2中上，3右上
        ///                                 4左中，5中，  6右中
        ///                                 7左下，8中下，9右下</param>
        /// <param name="padding">跟css里的padding一个意思</param>
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="opcity">不透明度  100 为完全不透明，0为完全透明</param>
        /// <param name="error"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static bool DrawWaterText(string destPath, string srcPath, string text, Font font, Brush brush, int pos, int padding, int quality, int opcity, out string error, string mimeType = "image/jpeg")
        {
            bool retVal = false;
            error = string.Empty;
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            if (font == null)
            {
                font = new Font("微软雅黑", 20, FontStyle.Bold, GraphicsUnit.Pixel);//统一尺寸
            }
            if (brush == null)
            {
                brush = new SolidBrush(Color.White);
            }
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                //定义画布，大小与源图像一样
                destImage = new Bitmap(srcImage);
                //获取高清Graphics
                graphics = Graphics.FromImage(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destImage.Width, destImage.Height), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果水印字不为空，且不透明度大于0，则画水印
                if (!string.IsNullOrEmpty(text) && opcity > 0)
                {
                    //获取可以用来绘制水印图片的有效区域
                    Rectangle validRect = new Rectangle(padding, padding, srcImage.Width - padding * 2, srcImage.Height - padding * 2);
                    //获取绘画水印文字的格式,即文字对齐方式
                    StringFormat format = GetStringFormat(pos);
                    //如果不透明度==100,那么直接将字画到当前画布上.
                    if (opcity == 100)
                    {
                        graphics.DrawString(text, font, brush, validRect, format);
                    }
                    else
                    {
                        //如果不透明度在0到100之间,就要实现透明效果，文字没法透明，图片才能透明
                        //则先将字画到一块临时画布，临时画布与destImage一样大，先将字写到这块画布上，再将临时画布画到主画布上，同时设置透明参数
                        Bitmap transImg = null;
                        Graphics gForTransImg = null;
                        try
                        {
                            //定义临时画布
                            transImg = new Bitmap(destImage);
                            //获取高清Graphics
                            gForTransImg = Graphics.FromImage(transImg);
                            //绘制文字
                            gForTransImg.DrawString(text, font, brush, validRect, format);
                            //**获取带有透明度的ImageAttributes
                            ImageAttributes imageAtt = GetAlphaImgAttr(opcity);
                            //将这块临时画布画在主画布上
                            graphics.DrawImage(transImg, new Rectangle(0, 0, destImage.Width, destImage.Height), 0, 0, transImg.Width, transImg.Height, GraphicsUnit.Pixel, imageAtt);
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            return retVal;
                        }
                        finally
                        {
                            if (transImg != null)
                                transImg.Dispose();
                            if (gForTransImg != null)
                                gForTransImg.Dispose();
                        }
                    }
                }
                //如果两个地址相同即覆盖，则提前Dispose源资源
                if (destPath.ToLower() == srcPath.ToLower())
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                //SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                if (destImage != null)
                    destImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
            return retVal;
        }
        public static Image DrawWaterText(Image srcImage, string text, Font font, Brush brush, int pos, int padding, int quality, int opcity, out string error, string mimeType = "image/jpeg")
        {

            error = string.Empty;

            Image destImage = null;
            Graphics graphics = null;
            if (font == null)
            {
                font = new Font("微软雅黑", 20, FontStyle.Bold, GraphicsUnit.Pixel);//统一尺寸
            }
            if (brush == null)
            {
                brush = new SolidBrush(Color.White);
            }
            try
            {
                //定义画布，大小与源图像一样
                destImage = new Bitmap(srcImage);
                //获取高清Graphics
                graphics = Graphics.FromImage(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destImage.Width, destImage.Height), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果水印字不为空，且不透明度大于0，则画水印
                if (!string.IsNullOrEmpty(text) && opcity > 0)
                {
                    //获取可以用来绘制水印图片的有效区域
                    Rectangle validRect = new Rectangle(padding, padding, srcImage.Width - padding * 2, srcImage.Height - padding * 2);
                    //获取绘画水印文字的格式,即文字对齐方式
                    StringFormat format = GetStringFormat(pos);
                    //如果不透明度==100,那么直接将字画到当前画布上.
                    if (opcity == 100)
                    {
                        graphics.DrawString(text, font, brush, validRect, format);
                    }
                    else
                    {
                        //如果不透明度在0到100之间,就要实现透明效果，文字没法透明，图片才能透明
                        //则先将字画到一块临时画布，临时画布与destImage一样大，先将字写到这块画布上，再将临时画布画到主画布上，同时设置透明参数
                        Bitmap transImg = null;
                        Graphics gForTransImg = null;
                        try
                        {
                            //定义临时画布
                            transImg = new Bitmap(destImage);
                            //获取高清Graphics
                            gForTransImg = Graphics.FromImage(transImg);
                            //绘制文字
                            gForTransImg.DrawString(text, font, brush, validRect, format);
                            //**获取带有透明度的ImageAttributes
                            ImageAttributes imageAtt = GetAlphaImgAttr(opcity);
                            //将这块临时画布画在主画布上
                            graphics.DrawImage(transImg, new Rectangle(0, 0, destImage.Width, destImage.Height), 0, 0, transImg.Width, transImg.Height, GraphicsUnit.Pixel, imageAtt);
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                            return null;
                        }
                        finally
                        {
                            if (transImg != null)
                                transImg.Dispose();
                            if (gForTransImg != null)
                                gForTransImg.Dispose();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                //if (destImage != null)
                //    destImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }

        /// <summary>
        /// 获取文字水印位置  
        /// </summary>
        /// <param name="pos">
        ///         1左上，2中上，3右上
        ///         4左中，5中，  6右中
        ///         7左下，8中下，9右下
        /// </param>
        /// <returns></returns>
        public static StringFormat GetStringFormat(int pos)
        {
            StringFormat format = new StringFormat();
            switch (pos)
            {
                case 1: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Near; break;
                case 2: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Near; break;
                case 3: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Near; break;
                case 4: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Center; break;
                case 6: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Center; break;
                case 7: format.Alignment = StringAlignment.Near; format.LineAlignment = StringAlignment.Far; break;
                case 8: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Far; break;
                case 9: format.Alignment = StringAlignment.Far; format.LineAlignment = StringAlignment.Far; break;
                default: format.Alignment = StringAlignment.Center; format.LineAlignment = StringAlignment.Center; break;
            }
            return format;

        }
        /// <summary>
        /// 获取一个带有透明度的ImageAttributes
        /// </summary>
        /// <param name="opcity"></param>
        /// <returns></returns>
        public static ImageAttributes GetAlphaImgAttr(int opcity)
        {
            if (opcity < 0 || opcity > 100)
            {
                throw new ArgumentOutOfRangeException("opcity 值为 0~100");
            }
            //颜色矩阵
            float[][] matrixItems =
            {
              new float[]{1,0,0,0,0},
              new float[]{0,1,0,0,0},
              new float[]{0,0,1,0,0},
              new float[]{0,0,0,(float)opcity / 100,0},
              new float[]{0,0,0,0,1}
            };
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAtt;
        }
        public static Bitmap RGB2Gray(Bitmap srcBitmap)

        {

            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData

            BitmapData srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            //创建Bitmap

            Bitmap dstBitmap = CreateGrayscaleImage(wide, height);//这个函数在后面有定义

            BitmapData dstBmData = dstBitmap.LockBits(rect,

                      ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;

            System.IntPtr dstPtr = dstBmData.Scan0;

            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];

            int dst_bytes = dstBmData.Stride * height;

            byte[] dstValues = new byte[dst_bytes];

            //复制GRB信息到byte数组

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);

            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);

            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            for (int i = 0; i < height; i++)

                for (int j = 0; j < wide; j++)

                {

                    //只处理每行中图像像素数据,舍弃未用空间

                    //注意位图结构中RGB按BGR的顺序存储

                    int k = 3 * j;

                    byte temp = (byte)(srcValues[i * srcBmData.Stride + k + 2] * .299

                         + srcValues[i * srcBmData.Stride + k + 1] * .587 + srcValues[i * srcBmData.Stride + k] * .114);

                    dstValues[i * dstBmData.Stride + j] = temp;

                }
            System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);

            //解锁位图

            srcBitmap.UnlockBits(srcBmData);

            dstBitmap.UnlockBits(dstBmData);

            return dstBitmap;

        }
        ///<summary>

        /// Create and initialize grayscale image

        ///</summary>

        public static Bitmap CreateGrayscaleImage(int width, int height)

        {

            // create new image

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // set palette to grayscale

            SetGrayscalePalette(bmp);

            // return new image

            return bmp;

        }//#
         ///<summary>

        /// Set pallete of the image to grayscale

        ///</summary>

        public static void SetGrayscalePalette(Bitmap srcImg)

        {

            // check pixel format

            if (srcImg.PixelFormat != PixelFormat.Format8bppIndexed)

                throw new ArgumentException();

            // get palette

            ColorPalette cp = srcImg.Palette;

            // init palette

            for (int i = 0; i < 256; i++)
            {

                cp.Entries[i] = Color.FromArgb(i, i, i);

            }

            srcImg.Palette = cp;

        }
        /// <summary>
        ///  通过内存访问的方法获得像素的颜色值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color myGetPixel(Bitmap srcBitmap, int x, int y)
        {
            Color retClr = new Color();
            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData;
            double clrLen;
            if (srcBitmap.PixelFormat == PixelFormat.Format32bppArgb ||
                srcBitmap.PixelFormat == PixelFormat.Format32bppPArgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                clrLen = 4;

            }
            else if (srcBitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                clrLen = 3;

            }
            //else if (srcBitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            //    clrLen = 1;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format4bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
            //    clrLen = 0.5;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format1bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            //    clrLen = 0.125;

            //}
            else
            {
                MessageBox.Show("图片格式不支持", "提示");
                return retClr;
            }

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;


            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];


            //复制GRB信息到byte数组

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            //只处理每行中图像像素数据,舍弃未用空间

            //注意位图结构中RGB按BGR的顺序存储

            int ptr = (int)clrLen * x;
            //int ptr;

            byte R, G, B, A = 0;
            if (clrLen == 4)
            {
                A = srcValues[y * srcBmData.Stride + ptr + 3];
            }
            R = srcValues[y * srcBmData.Stride + ptr + 2];
            G = srcValues[y * srcBmData.Stride + ptr + 1];
            B = srcValues[y * srcBmData.Stride + ptr + 0];

            //解锁位图

            srcBitmap.UnlockBits(srcBmData);

            if (clrLen == 4)
            {
                retClr = Color.FromArgb(A, R, G, B);
            }
            else
            {
                retClr = Color.FromArgb(R, G, B);

            }
            return retClr;
        }
        /// <summary>
        ///  通过内存设置的方法获得像素的颜色值,由于是单个像素操作和SetPixel速度相当
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Bitmap mySetPixel(Bitmap srcBitmap, Color clr, int x, int y)
        {

            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData;
            double clrLen;
            if (srcBitmap.PixelFormat == PixelFormat.Format32bppArgb ||
                srcBitmap.PixelFormat == PixelFormat.Format32bppPArgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                clrLen = 4;

            }
            else if (srcBitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                clrLen = 3;

            }
            //else if (srcBitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            //    clrLen = 1;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format4bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
            //    clrLen = 0.5;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format1bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            //    clrLen = 0.125;

            //}
            else
            {
                MessageBox.Show("图片格式不支持", "提示");
                return srcBitmap;
            }

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;


            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];




            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            //只处理每行中图像像素数据,舍弃未用空间

            //注意位图结构中RGB按BGR的顺序存储

            int ptr = (int)clrLen * x;
            //int ptr;

            if (clrLen == 4)
            {
                srcValues[y * srcBmData.Stride + ptr + 3] = clr.A;
            }
            srcValues[y * srcBmData.Stride + ptr + 2] = clr.R;
            srcValues[y * srcBmData.Stride + ptr + 1] = clr.G;
            srcValues[y * srcBmData.Stride + ptr + 0] = clr.B;

            //解锁位图
            System.Runtime.InteropServices.Marshal.Copy(srcValues, 0, srcPtr, src_bytes);
            srcBitmap.UnlockBits(srcBmData);


            return srcBitmap;
        }

        public static Bitmap WriteEmbossed(Bitmap srcBitmap, string text, Font font, int x, int y)
        {

            int width = srcBitmap.Width;

            int height = srcBitmap.Height;
            Bitmap dstBitmap = new Bitmap(srcBitmap.Width, srcBitmap.Height, srcBitmap.PixelFormat);


            Graphics dstGrph = Graphics.FromImage(dstBitmap);
            SolidBrush brush = new SolidBrush(Color.White);
            dstGrph.DrawString(text, font, brush, x, y);
            //The size of the string int the picture。
            Size sif = TextRenderer.MeasureText(dstGrph, text, font, new Size(0, 0), TextFormatFlags.NoPadding);
            //Define the locked area in the picture
            Rectangle rect = new Rectangle(0, 0, width, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcBmData;
            BitmapData dstBmData;

            int clrLen;
            if (srcBitmap.PixelFormat == PixelFormat.Format32bppArgb ||
                srcBitmap.PixelFormat == PixelFormat.Format32bppPArgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                dstBmData = dstBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                clrLen = 4;

            }
            else if (srcBitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                dstBmData = dstBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                clrLen = 3;

            }
            //else if (srcBitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            //    clrLen = 1;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format4bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
            //    clrLen = 0.5;

            //}
            //else if (srcBitmap.PixelFormat == PixelFormat.Format1bppIndexed)
            //{
            //    srcBmData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            //    clrLen = 0.125;

            //}
            else
            {
                MessageBox.Show("图片格式不支持", "提示");
                return srcBitmap;
            }

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;
            System.IntPtr dstPtr = dstBmData.Scan0;

            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];

            int dst_bytes = dstBmData.Stride * height;

            byte[] dstValues = new byte[dst_bytes];




            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);

            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            //只处理每行中图像像素数据,舍弃未用空间

            //注意位图结构中RGB按BGR的顺序存储

            for (int i = y; i < height; i++)

                for (int j = x; j < width; j++)

                {

                    //只处理每行中图像像素数据,舍弃未用空间

                    //注意位图结构中RGB按BGR的顺序存储

                    int dPtr = clrLen * j;
                    int sPtr = clrLen * j;

                    byte dstR, dstG, dstB;
                    byte sR, sG, sB;
                    dstR = dstValues[i * dstBmData.Stride + dPtr + 2];

                    sR = srcValues[i * srcBmData.Stride + sPtr + 2];
                    sG = srcValues[i * srcBmData.Stride + sPtr + 1];
                    sB = srcValues[i * srcBmData.Stride + sPtr + 0];

                    //if (i>0 && j > 0)
                    if (dstR == 255)
                    {
                        srcValues[i * srcBmData.Stride + sPtr + 2] = (byte)(sR / 2);
                        srcValues[i * srcBmData.Stride + sPtr + 1] = (byte)(sG / 2);
                        srcValues[i * srcBmData.Stride + sPtr + 0] = (byte)(sB / 2);

                        sPtr = clrLen * (j - 1);
                        int i_1 = i - 1;
                        int sr = sR + 24 > 255 ? 255 : sR + 24;
                        sR = (byte)sr;
                        int sg = sG + 24 > 255 ? 255 : sG + 24;
                        sG = (byte)sg;
                        int sb = sB + 24 > 255 ? 255 : sB + 24;
                        sB = (byte)sb;

                        srcValues[i_1 * srcBmData.Stride + sPtr + 2] = sR;
                        srcValues[i_1 * srcBmData.Stride + sPtr + 1] = sG;
                        srcValues[i_1 * srcBmData.Stride + sPtr + 0] = sB;
                        //srcValues[i_1 * srcBmData.Stride + sPtr + 2] = 255;
                        //srcValues[i_1 * srcBmData.Stride + sPtr + 1] = 255;
                        //srcValues[i_1 * srcBmData.Stride + sPtr + 0] = 255;
                    }


                }

            System.Runtime.InteropServices.Marshal.Copy(srcValues, 0, srcPtr, src_bytes);            //解锁位图
            dstBitmap.UnlockBits(dstBmData);
            srcBitmap.UnlockBits(srcBmData);


            return srcBitmap;
        }

        // blind image, angle: the leaf rotate angle, dir, H horizontal or V
        // vertical ,
        // leafNb, the number of the leaves
        public static Bitmap BlindWindow(Bitmap src, double angle, String dir, int leafNb)
        {
            int src_width = src.Width;
            int src_height = src.Height;
            // src.getCapabilities(null);
            // src.getGraphics().
            // calculate the new m_Image size
            Bitmap subImg = null;
            Bitmap leafImg = null;
            Bitmap mewImg = null;
            mewImg = new Bitmap(src_width, src_height, PixelFormat.Format24bppRgb);
            Graphics gNew = Graphics.FromImage(mewImg);
            Graphics gSrc = Graphics.FromImage(src);

            int x, y;
            double w, h;
            int leafW, leafH;
            x = 0;
            y = 0;

            if (dir == "H")
            {
                w = src_width;
                h = src_height / (double)leafNb;
                leafW = (int)w;
                leafH = (int)(h * Math.Sin(angle / 180 * Math.PI));
                if (leafH == 0)
                    leafH = 1;

            }
            else
            {
                w = src_width / (double)leafNb;
                h = src_height;
                leafH = (int)h;
                leafW = (int)(w * Math.Sin(angle / 180 * Math.PI));
                if (leafW == 0)
                    leafW = 1;
            }
            double dblX, dblY;
            dblX = x;
            dblY = y;
            if (dir == "H")
            {
                for (int i = 0; i < leafNb; i++)
                {
                    //subImg = src.getgetSubimage(x, y, (int)w, (int)h);
                    subImg = getSubImage(src, (int)x, (int)y, (int)w, (int)h);

                    //leafImg = subImg.getScaledInstance(leafW, leafH, BufferedImage.SCALE_SMOOTH);
                    leafImg = shrinkTo(subImg, new Rectangle(0, 0, leafW, leafH));

                    gNew.DrawImage(leafImg, new Point(x, (int)(dblY + (h - leafH) / 2)));
                    dblY += h;
                    y = (int)dblY;

                }
            }
            else
            {
                for (int i = 0; i < leafNb; i++)
                {
                    //subImg = src.getSubimage(x, y, (int)w, (int)h);
                    subImg = getSubImage(src, (int)x, (int)y, (int)w, (int)h);
                    //leafImg = subImg.getScaledInstance(leafW, leafH, BufferedImage.SCALE_SMOOTH);
                    shrinkTo(subImg, new Rectangle(0, 0, leafW, leafH));
                    leafImg = shrinkTo(subImg, new Rectangle(0, 0, leafW, leafH));
                    //gNewImg.drawImage(leafImg, (int)(dblX + (w - leafW) / 2), y, null);
                    gNew.DrawImage(leafImg, new Point((int)(dblX + (w - leafW) / 2), y));

                    dblX += w;
                    x = (int)dblX;

                }

            }
            gNew.Dispose();

            return mewImg;
        }

        public static void displayBlindWindow(Bitmap src, delDisplayImg delDippaly, String dir, int leafNb)
        {
            double angle;
            double dAngle = 90.0 / leafNb;
            for (angle = dAngle; angle <= 90; angle += dAngle)
            {
                delDippaly(BlindWindow(src, angle, dir, leafNb));
                Thread.Sleep(100);
                Application.DoEvents();
            }
            delDippaly(src);
        }


        /// <summary>
        /// 对图像pic进行缩放处理，缩放为Rect大小的新图像, from internet 
        /// </summary>
        public static Bitmap shrinkTo(Image pic, Rectangle Rect)
        {
            //创建图像
            Bitmap tmp = new Bitmap(Rect.Width, Rect.Height);                   //按指定大小创建位图
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Rectangle srcRect = new Rectangle(0, 0, pic.Width, pic.Height);     //pic的整个区域

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, srcRect, GraphicsUnit.Pixel);//从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }
        /// <summary>
        ///   对图像进行压缩变形
        ///   起点：图像的左边或者上边顶点。
        /// </summary>
        /// <param name="src">图像</param>
        /// <param name="edge">1:左边；2顶边；3右边；4底边</param>
        /// <param name="from">相对于起点的距离</param>
        /// <param name="to">相对于起点的距离， 此距离要大于 from</param>
        /// <returns></returns>
        public static Bitmap deform(Bitmap src, int edge, float from, float to)
        {
            //创建图像
            if (to < from) return src;

            Bitmap dest = new Bitmap(src.Width, src.Height);                   //按指定大小创建位图
            Bitmap subSrc;

            //一个像素宽度的子图片
            if (edge == 1 || edge == 3)
            {
                subSrc = new Bitmap(1, src.Height);
            }
            else if (edge == 2 || edge == 4)
            {
                subSrc = new Bitmap(src.Width, 1); //一个像素高度的子图片
            }
            else
            {
                return src;
            }
            float baseL = to - from + 1;
            //float ratio;
            Graphics destG = Graphics.FromImage(dest);
            if (edge == 1 || edge == 3)
            {
                int tempH;//一个像素宽的图像高度
                float subHeight;
                subHeight = src.Height - baseL;
                int step = 1;
                float w;
                w = edge == 1 ? 0 : src.Width;
                step = edge == 1 ? 1 : -1;
                for (float i = 0; i < src.Width; i++)
                {
                    //计算一个像宽的高度
                    tempH = (int)(baseL + (i / src.Width) * (src.Height - baseL) + 0.5);
                    //获取一个像素宽的图像
                    subSrc = getSubImage(src, (int)w, 0, 1, src.Height);
                    //获取该图像的压缩后的图像
                    Rectangle rec = new Rectangle(0, 0, 1, tempH);
                    Bitmap subDest = shrinkTo(subSrc, rec);
                    float subY;
                    subY = from - (from / src.Width * i);
                    //if((int)subY+ subDest.Height>src.Height)
                    //{
                    //    MessageBox.Show("error");
                    //}
                    destG.DrawImage(subDest, w, subY);
                    w = w + step;
                }


            }
            else if (edge == 2 || edge == 4)
            {
                int tempW;//一个像素宽的图像高度
                float subWidth;
                subWidth = src.Width - baseL;
                int step = 1;
                float h;
                h = edge == 2 ? 0 : src.Height;
                step = edge == 2 ? 1 : -1;
                for (float i = 0; i < src.Height; i++)
                {
                    //计算一个像宽的高度
                    tempW = (int)(baseL + (i / src.Height) * (src.Width - baseL) + 0.5);
                    //获取一个像素宽的图像
                    subSrc = getSubImage(src, 0, (int)h, src.Width, 1);
                    //获取该图像的压缩后的图像
                    Rectangle rec = new Rectangle(0, 0, tempW, 1);
                    Bitmap subDest = shrinkTo(subSrc, rec);
                    float subX;
                    subX = from - (from / src.Height * i);
                    //if((int)subY+ subDest.Height>src.Height)
                    //{
                    //    MessageBox.Show("error");
                    //}
                    destG.DrawImage(subDest, subX, h);
                    h = h + step;
                }


            }
            else
            {
                return src;
            }


            //绘制
            //从位图创建Graphics对象
            //destG.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            //destG.DrawImage(src, drawRect, srcRect, GraphicsUnit.Pixel);//从pic的给定区域进行绘制

            return dest;     //返回构建的新图像
        }
        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像, from internet
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image GetRotateImage(Image srcImage, int angle)
        {
            angle = angle % 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
            int rotateWidth = rotateRec.Width;
            int rotateHeight = rotateRec.Height;
            //目标位图
            Bitmap destImage = null;
            Graphics graphics = null;
            try
            {
                //定义画布，宽高为图像旋转后的宽高
                destImage = new Bitmap(rotateWidth, rotateHeight);
                //graphics根据destImage创建，因此其原点此时在destImage左上角
                graphics = Graphics.FromImage(destImage);
                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotateWidth / 2, rotateHeight / 2);
                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                Point Offset = new Point((rotateWidth - srcWidth) / 2, (rotateHeight - srcHeight) / 2);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }
        /// <summary>
        /// 计算矩形绕中心任意角度旋转后所占区域矩形宽高
        /// </summary>
        /// <param name="width">原矩形的宽</param>
        /// <param name="height">原矩形高</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public static Rectangle GetRotateRectangle(int width, int height, float angle)
        {
            double radian = angle * Math.PI / 180; ;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }
        public static void displayFade(Bitmap src, delDisplayImg delDippaly, bool bIn, int interval, int step)
        {
            int opacity = 0;
            step = Math.Abs(step);
            if (step > 100) return;
            if (!bIn)
            {
                opacity = 100;
                step = -step;
            }
            while (true)
            {

                Bitmap bmp = ImgeTool.FadeInOrOut(src, opacity);
                delDippaly(bmp);
                Thread.Sleep(interval);
                opacity += step;
                if (opacity > 100 || opacity < 0) break;
            }
        }

        public static Bitmap FadeInOrOut(Bitmap src, int opacity)
        {
            //if (img == null)
            //{
            //    throw new Exception("淡入内容不得为Null！");
            //    return;
            //}
            Bitmap fadeBmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

            Size size = new Size(src.Width, src.Height);
            //Image imgBack = (Image)src.Clone();
            Graphics g = Graphics.FromImage(fadeBmp);

            int width = src.Width;
            int height = src.Height;
            ImageAttributes attributes = new ImageAttributes();



            //创建淡入颜色矩阵
            float[][] colorMatrixElements = {
                                new float[] {0,  0,  0,  0, 0},        // 0,0 red
                                new float[] {0,  0,  0,  0, 0},        // 1,1 green 
                                new float[] {0,  0,  0,  0, 0},        // 2,2 blue
                                new float[] {0,  0,  0,  1, 0},        // 3,3 alpha
                                new float[] {0,  0,  0,  0, 1}};       // 4,4 W



            ColorMatrix matrix = new ColorMatrix(colorMatrixElements);
            try
            {
                if (opacity > 100) opacity = 100;
                if (opacity < 0) opacity = 0;

                float count = opacity / 100.0f;

                matrix.Matrix00 = count;
                matrix.Matrix11 = count;
                matrix.Matrix22 = count;
                matrix.Matrix33 = count;
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(src, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attributes);
                g.Save();
            }
            catch (Exception ex)
            {
                throw ex;           //错误处理
                //frm.BackgroundImage = bmp;
            }

            return (Bitmap)fadeBmp;
        }
        public static Bitmap ZoomInOrOut(Bitmap src, float scale)
        {
            scale = Math.Abs(scale);
            int w, h;
            w = (int)(src.Width * scale);
            h = (int)(src.Height * scale);
            if (w * h == 0) return src;
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g1 = Graphics.FromImage(bmp);
            g1.Clear(Color.White);
            g1.ScaleTransform(scale, scale);
            int left, top;
            left = (int)(src.Width - src.Width * scale) / 2;
            top = (int)(src.Height - src.Height * scale) / 2;
            g1.DrawImage(src, 0, 0, src.Width, src.Height);
            Bitmap dest = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gDest = Graphics.FromImage(dest);
            gDest.DrawImage(bmp, left, top);
            bmp.Dispose();
            return dest;
        }

        public static void displayZoom(Bitmap src, delDisplayImg delDippaly, bool bZoomIn, int interval, int times)
        {
            float ratio = 0f;
            if (times <= 0) return;
            float step = 1.0f / times;
            times = Math.Abs(times);
            ratio = step;
            if (!bZoomIn)
            {
                ratio = 1f;
                step = -step;

            }
            while (true)
            {
                if (ratio > 1 || ratio <= 0) break;
                Bitmap bmp = ImgeTool.ZoomInOrOut(src, ratio);
                delDippaly(bmp);
                Thread.Sleep(interval);
                ratio += step;

            }
        }
        /// <summary>
        ///  获取图像一图像中心为圆心，参数radius 为半径的圆形图像。
        /// </summary>
        /// <param name="SRC"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Bitmap circle(Bitmap src, int radius, bool bsameSize = true)
        {
            //生成一个以src最大边为宽和高的正方形图像，使src在中间，而其他部分为透明
            int maxR = src.Width;
            if (maxR < src.Height) maxR = src.Height;
            maxR = maxR / 2 * 2 + 1;//将边长处理成奇数
            Bitmap SRC = new Bitmap(maxR, maxR, PixelFormat.Format32bppArgb);
            Graphics SRCG = Graphics.FromImage(SRC);
            SRCG.Clear(Color.Transparent);
            //SRCG.FillRectangle()
            //Rectangle putRec=  new Rectangle((maxR - src.Width) / 2, (maxR - src.Height) / 2),src.Width,src.Height) ;
            Rectangle putRec = new Rectangle((maxR - src.Width) / 2, (maxR - src.Height) / 2, src.Width, src.Height);
            Rectangle getGec = new Rectangle(0, 0, src.Width, src.Height);
            SRCG.DrawImage(src, putRec, getGec, GraphicsUnit.Pixel);
            SRCG.Save();
            //return SRC;
            radius = Math.Abs(radius);

            //计算内接正方形的半边长
            int inner_side;
            inner_side = (int)(radius / Math.Sqrt(2.0));
            //处理成奇数
            inner_side = inner_side / 2 * 2 + 1;
            //去以原图像中心为中心的， 边长为radius*2 -1 的正方形图片
            Bitmap dest = new Bitmap((int)(radius * 2) - 1, (int)(radius * 2) - 1, PixelFormat.Format32bppArgb);
            Graphics destG = Graphics.FromImage(dest);
            Bitmap subBmp;
            subBmp = getSubImage(SRC, (int)(SRC.Width - dest.Width) / 2, (int)(SRC.Height - dest.Height) / 2, (int)(radius * 2), (int)(radius * 2));
            destG.DrawImage(subBmp, 0, 0);


            //将大于半径的元素的透明度变成全透明

            int wide = dest.Width;

            int height = dest.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData dstBmData;
            double clrLen;
            dstBmData = dest.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            clrLen = 4;
            int centerX, centerY;
            centerX = (dest.Width - 1) / 2;
            centerY = (dest.Height - 1) / 2;

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = dstBmData.Scan0;


            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = dstBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];


            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            //只处理每行中图像像素数据,舍弃未用空间

            //注意位图结构中RGB按BGR的顺序存储
            int X;
            int Y;
            int ptr;
            for (int x = 0; x < inner_side; x++)
            {


                for (double y = inner_side; y < radius; y++)
                {

                    if (x + inner_side < radius)
                    {
                        X = (int)(centerX + x + inner_side + 0.5);
                        Y = (int)(centerY - y + 0.5);
                        //第一象限的右上角的小方块
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;// 透明处理
                        //srcValues[Y * dstBmData.Stride + ptr + 2] = 0;
                        //srcValues[Y * dstBmData.Stride + ptr + 1] = 0;
                        //srcValues[Y * dstBmData.Stride + ptr + 0] = 0;
                        //第二象限
                        X = (int)(centerX - x - inner_side + 0.5);
                        Y = (int)(centerY - y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        //第三象限
                        X = (int)(centerX - x - inner_side + 0.5);
                        Y = (int)(centerY + y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;
                        //第四象限
                        X = (int)(centerX + x + inner_side + 0.5);
                        Y = (int)(centerY + y + 0.5);

                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;
                    }



                    if (x * x + y * y > radius * radius)
                    {
                        //第一象限， y=x 以上部分，世界坐标
                        X = (int)(centerX + x + 0.5);
                        Y = (int)(centerY - y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        //第一象限， y=x 以下部分，世界坐标

                        X = (int)(centerX + y + 0.5);
                        Y = (int)(centerY - x + 0.5);
                        ptr = (int)clrLen * X;
                        //int ptr;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;



                        //第二象限

                        X = (int)(centerX - x + 0.5);
                        Y = (int)(centerY - y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        X = (int)(centerX - y + 0.5);
                        Y = (int)(centerY - x + 0.5);
                        ptr = (int)clrLen * X;
                        //int ptr;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        //第三象限

                        X = (int)(centerX - x + 0.5);
                        Y = (int)(centerY + y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        X = (int)(centerX - y + 0.5);
                        Y = (int)(centerY + x + 0.5);
                        ptr = (int)clrLen * X;
                        //int ptr;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        //第四象限
                        X = (int)(centerX + x + 0.5);
                        Y = (int)(centerY + y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;

                        //第一象限， y=x 以下部分，世界坐标

                        X = (int)(centerX + y + 0.5);
                        Y = (int)(centerY + x + 0.5);
                        ptr = (int)clrLen * X;
                        //int ptr;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;



                    }
                }

            }
            //解锁位图
            System.Runtime.InteropServices.Marshal.Copy(srcValues, 0, srcPtr, src_bytes);
            dest.UnlockBits(dstBmData);
            if (bsameSize)
            {
                Bitmap tmpImg = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                Graphics tmpG = Graphics.FromImage(tmpImg);
                tmpG.DrawImage(dest, (src.Width - dest.Width) / 2, (src.Height - dest.Height) / 2);
                dest = tmpImg;
            }
            return dest;
        }
        /// <summary>
        ///  获得同心的等边三角形图像
        /// </summary>
        /// <param name="src"></param>
        /// <param name="sideL"></param>
        /// <param name="bsameSize"></param>
        /// <returns></returns>
        public static Bitmap triangle(Bitmap src, float sideL, bool bsameSize = true)
        {
            //生成辅助图像，使src在中间，周边透明
            int maxW, maxH;
            //计算上这个大矩形图案的边长

            double sqrt3 = Math.Sqrt(3.0);
            maxW = (int)(2.0 * src.Width + 2.0 / sqrt3 * src.Height + 0.5);
            maxH = (int)(sqrt3 * src.Width + src.Height + 0.5);
            maxW = maxW / 2 * 2 + 1;
            maxH = maxH / 2 * 2 + 1;

            sideL = (int)(sideL / 2) * 2 + 1;

            Bitmap SRC = new Bitmap(maxW, maxH, PixelFormat.Format32bppArgb);
            Graphics SRCG = Graphics.FromImage(SRC);
            SRCG.Clear(Color.Transparent);
            //SRCG.FillRectangle()
            //Rectangle putRec=  new Rectangle((maxR - src.Width) / 2, (maxR - src.Height) / 2),src.Width,src.Height) ;
            int srcX, srcY;
            srcX = (int)(src.Width / 2.0 + src.Height / sqrt3);
            srcY = (int)(sqrt3 / 2 * src.Width);
            Rectangle putRec = new Rectangle(srcX, srcY, src.Width, src.Height);
            Rectangle getGec = new Rectangle(0, 0, src.Width, src.Height);
            SRCG.DrawImage(src, putRec, getGec, GraphicsUnit.Pixel);
            //SRCG.Save();



            //sideL = Math.Abs(sideL);
            //计算外接矩形的边长
            int outW, outH;
            outW = (int)sideL;
            outH = (int)(sideL * sqrt3 / 2.0);
            outH = outH / 2 * 2 + 1;
            //取以原图像中心为中心的， 边长为outW和outH的子图片
            Bitmap dest = new Bitmap(outW, outH, PixelFormat.Format32bppArgb);
            Graphics destG = Graphics.FromImage(dest);
            Bitmap subBmp;
            subBmp = getSubImage(SRC, (SRC.Width - dest.Width) / 2, (SRC.Height - dest.Height) / 2, dest.Width, dest.Height);
            destG.DrawImage(subBmp, 0, 0);




            int wide = dest.Width;

            int height = dest.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData dstBmData;
            double clrLen;
            dstBmData = dest.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            clrLen = 4;
            float centerX, centerY;
            centerX = (dest.Width - 1.0f) / 2;
            centerY = (dest.Height - 1.0f) / 2;

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = dstBmData.Scan0;


            //将Bitmap对象的信息存放到byte数组中

            int src_bytes = dstBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];


            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);
            //根据Y=0.299*R+0.114*G+0.587B,Y为亮度

            //只处理每行中图像像素数据,舍弃未用空间

            //注意位图结构中RGB按BGR的顺序存储
            int X;
            int Y;
            int ptr;

            for (int x = 0; x < (int)(sideL / 2.0 + 0.5); x++)
            {


                for (double y = (-sqrt3 * sideL / 4.0); y < sqrt3 * sideL / 4.0 + 1; y++)
                {

                    if (y >= sqrt3 * sideL / 4f * (1.0 - x / (sideL / 4.0)))
                    {
                        //第一象限， y=x 以上部分，世界坐标
                        X = (int)(centerX + x + 0.5);
                        Y = (int)(centerY - y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;
                        X = (int)(centerX - x);
                        Y = (int)(centerY - y + 0.5);
                        ptr = (int)clrLen * X;
                        srcValues[Y * dstBmData.Stride + ptr + 3] = 0;
                    }
                }

            }
            //解锁位图
            System.Runtime.InteropServices.Marshal.Copy(srcValues, 0, srcPtr, src_bytes);
            dest.UnlockBits(dstBmData);
            //Bitmap dest = square(src, (int)sideL);
            if (bsameSize)
            {
                Bitmap tmpImg = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                Graphics tmpG = Graphics.FromImage(tmpImg);
                tmpG.DrawImage(dest, (src.Width - dest.Width) / 2, (src.Height - dest.Height) / 2);
                dest = tmpImg;
            }
            return dest;
        }

        /// <summary>
        /// 获取图像中心的正方形图像
        /// </summary>
        /// <param name="src"></param>
        /// <param name="sideL"></param>
        /// <param name="bsameSize"></param>
        /// <returns></returns>
        public static Bitmap square(Bitmap src, int sideL, bool bsameSize = true)
        {
            //生成一个以src最大边为宽和高的正方形图像，使src在中间，而其他部分为透明
            //            sideL = Math.Abs(sideL);
            int maxR = src.Width;
            if (maxR < src.Height) maxR = src.Height;
            //将边长调整为基数，这样中心就不是在两点之间了。
            maxR = maxR / 2 * 2 + 1;
            sideL = sideL / 2 * 2 + 1;
            Bitmap SRC = new Bitmap(maxR, maxR, PixelFormat.Format32bppArgb);
            Graphics SRCG = Graphics.FromImage(SRC);
            SRCG.Clear(Color.Transparent);
            //SRCG.FillRectangle()
            //Rectangle putRec=  new Rectangle((maxR - src.Width) / 2, (maxR - src.Height) / 2),src.Width,src.Height) ;
            Rectangle putRec = new Rectangle((maxR - src.Width) / 2, (maxR - src.Height) / 2, src.Width, src.Height);
            Rectangle getGec = new Rectangle(0, 0, src.Width, src.Height);
            SRCG.DrawImage(src, putRec, getGec, GraphicsUnit.Pixel);
            SRCG.Save();
            //return SRC;

            Bitmap dest;

            dest = getSubImage(SRC, (int)(SRC.Width - sideL) / 2, (int)(SRC.Height - sideL) / 2, sideL, sideL);

            if (bsameSize)
            {
                Bitmap tmpImg = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                Graphics tmpG = Graphics.FromImage(tmpImg);
                tmpG.DrawImage(dest, (src.Width - dest.Width) / 2, (src.Height - dest.Height) / 2);
                dest = tmpImg;
            }
            return dest;
        }
        /// <summary>
        ///  以扇形的方式显示图形
        /// </summary>
        /// <param name="src"></param>
        /// <param name="delDippaly"></param>
        /// <param name="radious"></param>
        /// <param name="startAngle">起始角度（dgree）</param>
        /// <param name="endAngle">结束角度（dgree）</param>
        /// <returns></returns>
        public static Bitmap sector(Bitmap src, int radious, int startAngle, int endAngle,bool bsameSize=true)
        {
            //获得源图的圆形图
            Bitmap srcCircle = circle(src, radious, false);
            Bitmap dest = new Bitmap(srcCircle);
            //return dest;
            Graphics destG = Graphics.FromImage(dest);
            destG.Clear(Color.Transparent);
            double ang1, ang2;
            //转换为弧度
            ang1 = startAngle / 180.0 * Math.PI;
            ang2 = endAngle / 180.0 * Math.PI;
          
            //确定起始坐标范围，分成四个象限单独考虑
            //第一象限
            double A1, A2, Aborder;
            Aborder = Math.PI / 2;
            int x1, x2, y1, y2;// x y 的范围
            x1 = x2 = y1 = y2 = 0;
            int r2 = radious * radious;
            bool bEnd = false;


            int wide = dest.Width;

            int height = dest.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData srcCBmData, dstBmData;
            double clrLen;
            srcCBmData = srcCircle.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            dstBmData = dest.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            clrLen = 4;
            float centerX, centerY;
            centerX = (dest.Width - 1.0f) / 2;
            centerY = (dest.Height - 1.0f) / 2;

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr dstPtr = dstBmData.Scan0;
            System.IntPtr srcCPtr = srcCBmData.Scan0;


            //将Bitmap对象的信息存放到byte数组中

            int dst_bytes = dstBmData.Stride * height;

            byte[] dstValues = new byte[dst_bytes];

            int srcC_bytes = srcCBmData.Stride * height;

            byte[] srcCValues = new byte[srcC_bytes];

            System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);
            System.Runtime.InteropServices.Marshal.Copy(srcCPtr, srcCValues, 0, srcC_bytes);

            int X;
            int Y;
            int ptr;
            double tmpA;
            for (int i = 0; i < 5; i++)
            {
                if (ang1 >= i * Aborder && ang1 < Aborder * (i + 1))
                {
                    A1 = ang1;

                    if (ang2 > Aborder * (i + 1))
                    {
                        A2 = Aborder * (i + 1);
                        ang1 = A2; ///!!!!!!!!!!!，代换进入下一象限判断
                    }
                    else
                    {
                        A2 = ang2;//结束
                        bEnd = true;
                    }
                    //直线方程是： y=tan A *x
                    //确定想x y 的范围
                    if (i == 0)
                    {
                        x1 = 0;
                        y1 = 0;
                        x2 = (int)(radious * Math.Cos(A1) + 0.5);
                        y2 = (int)(radious * Math.Sin(A2) + 0.5);
                    }
                    else if (i == 1)
                    {
                        tmpA = A1;
                        A1 = A2;
                        A2 = tmpA;

                        x2 = 0;
                        y1 = 0;
                        x1 = (int)(radious * Math.Cos(A1) - 0.5);
                        y2 = (int)(radious * Math.Sin(A2) + 0.5);


                    }
                    else if (i == 2)
                    {
                        //tmpA = A1;
                        //A1 = A2;
                        //A2 = tmpA;
                        x2 = 0;
                        y2 = 0;
                        x1 = (int)(radious * Math.Cos(A1) - 0.5);
                        y1 = (int)(radious * Math.Sin(A2) - 0.5);

                    }
                    else if (i == 3)
                    {
                        tmpA = A1;
                        A1 = A2;
                        A2 = tmpA;
                        x1 = 0;
                        y2 = 0;
                        x2 = (int)(radious * Math.Cos(A1) + 0.5);
                        y1 = (int)(radious * Math.Sin(A2) - 0.5);

                    }
                    else if (i == 4)
                    {
                        x1 = 0;
                        y1 = 0;
                        x2 = (int)(radious * Math.Cos(A1) + 0.5);
                        y2 = (int)(radious * Math.Sin(A2) + 0.5);

                    }
                    for (int x = x1; x <= x2; x++)
                    {
                        for (int y = y1; y < y2; y++)
                        {
                            if ((Math.Abs(y) >= Math.Abs(x * Math.Tan(A1)) || (y == 0 && A1 % (Math.PI / 2) == 0)) && (Math.Abs(y) <= Math.Abs(x * Math.Tan(A2)) || (x == 0 && A2 % (Math.PI / 2) == 0)))
                            {
                                if (x * x + y * y < r2)
                                {
                                    X = (int)(centerX + x);
                                    Y = (int)(centerY - y);
                                    ptr = (int)clrLen * X;
                                    dstValues[Y * dstBmData.Stride + ptr + 3] = srcCValues[Y * srcCBmData.Stride + ptr + 3];
                                    dstValues[Y * dstBmData.Stride + ptr + 2] = srcCValues[Y * srcCBmData.Stride + ptr + 2];
                                    dstValues[Y * dstBmData.Stride + ptr + 1] = srcCValues[Y * srcCBmData.Stride + ptr + 1];
                                    dstValues[Y * dstBmData.Stride + ptr + 0] = srcCValues[Y * srcCBmData.Stride + ptr + 0];
                                }
                            }
                        }
                    }
                    if (bEnd) break;

                }

            }

            //解锁位图
            System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);
            dest.UnlockBits(dstBmData);
           // System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);
            srcCircle.UnlockBits(srcCBmData);

            if (bsameSize)
            {
                Bitmap tmpImg = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                Graphics tmpG = Graphics.FromImage(tmpImg);
                tmpG.DrawImage(dest, (src.Width - dest.Width) / 2, (src.Height - dest.Height) / 2);
                dest = tmpImg;
            }
            return dest;

        }
        public static void displayRadar(Bitmap src, delDisplayImg delDippaly, bool bZoomIn, int interval, int times)
        {
            int radius = 0;
            if (times <= 0) return;
            int maxR;
         
            maxR = (int)(Math.Sqrt(src.Width * src.Width + src.Height * src.Height) / 2 + 0.5);
            int step = maxR / times;
            times = Math.Abs(times);
            if (step < 1)
            {
                step = 1;
            }
            radius = step;
            if (!bZoomIn)
            {
                radius = maxR;
                step = -step;

            }
            Bitmap bmp;
            bmp = src;
            while (true)
            {
                if (radius > maxR || radius <= 0) break;
                for (int A = 0; A <= 360; A = A + 10)
                {
                    bmp = sector(src, radius, 0,A);


                    delDippaly(bmp);
                    Thread.Sleep(interval);
                }
                radius += step;

            }

        }
        public static void displaySquare(Bitmap src, delDisplayImg delDippaly, bool bZoomIn, int interval, int times)
        {
            int sideL = 0;
            if (times <= 0) return;
            int maxR;
            maxR = src.Width;
            if (maxR < src.Height) maxR = src.Height;
            float step = maxR / times;
            times = Math.Abs(times);
            sideL = (int)(step + 0.5);
            if (!bZoomIn)
            {
                sideL = maxR;
                step = -step;

            }
            while (true)
            {
                if (sideL > maxR || sideL <= 0) break;
                Bitmap bmp = ImgeTool.square(src, sideL);
                delDippaly(bmp);
                Thread.Sleep(interval);
                sideL = (int)(sideL + step + 0.5);

            }
        }

        public static void displayCircle(Bitmap src, delDisplayImg delDippaly, bool bZoomIn, int interval, int times)
        {
            int radius = 0;
            if (times <= 0) return;
            int maxR;
            maxR = (int)(Math.Sqrt(src.Width* src.Width + src.Height* src.Height)/2 +0.5);
           
            int step = maxR / times;
            times = Math.Abs(times);
            if (step < 1)
            {
                step = 1;
            }
            radius = step;
            if (!bZoomIn)
            {
                radius = maxR;
                step = -step;

            }
            while (true)
            {
                if (radius > maxR || radius <= 0) break;
                Bitmap bmp = ImgeTool.circle(src, (int)radius);
                delDippaly(bmp);
                Thread.Sleep(interval);
                radius += step;

            }
        }
        public static void displayTriangle(Bitmap src, delDisplayImg delDippaly, bool bZoomIn, int interval, int times)
        {

            int sideL = 0;
            if (times <= 0) return;
            float maxL;
            double sqrt3 = Math.Sqrt(3.0);
            //maxW =(float)( src.Width*2+2/sqrt3*src.Height);
            maxL = (float)(2.0 * src.Width + 2.0 / sqrt3 * src.Height);

            //if (maxR < src.Height) maxR = src.Height;
            float step = maxL / times;
            times = Math.Abs(times);
            sideL = (int)step;
            if (!bZoomIn)
            {
                sideL = (int)maxL;
                step = -step;

            }
            while (true)
            {
                if (sideL > maxL || sideL <= 0) break;
                Bitmap bmp = ImgeTool.triangle(src, sideL);
                delDippaly(bmp);
                Thread.Sleep(interval);
                sideL += (int)step;

            }
        }
    }
}
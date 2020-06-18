using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Img2ColorfulChars
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            #region Set console

            // Maximize console
            NativeMethods.ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 3); //SW_MAXIMIZE = 3
            // Set flag for colorful output
            var handle = NativeMethods.GetStdHandle(-11); // STD_OUTPUT_HANDLE = -11
            NativeMethods.GetConsoleMode(handle, out int mode);
            NativeMethods.SetConsoleMode(handle, mode | 0x4); // ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4

            #endregion Set console

            // Load image or video path
            OpenFileDialog ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = string.Format("Media(*.jpg;*.jpeg;*.png;*.bmp;*.ico;*.tiff;*.gif;{0})|*.jpg;*.jpeg;*.png;*.bmp;*.ico;*.tiff;*.gif;{0}", VideoConverter.FilterString),
                Multiselect = false,
                Title = "Select an image or video to convert..."
            };
            ofd.ShowDialog();
            if (string.IsNullOrEmpty(ofd.FileName)) { return; }
            string filename = ofd.FileName;

            // Set converter mode
            ConverterMode converterMode = ConverterMode.Image;
            if (VideoConverter.SupportedFormats.Contains(new FileInfo(filename).Extension)) { converterMode = ConverterMode.Video; }
            if (converterMode == ConverterMode.Image)
            {
                // Provide suggested scale
                int suggestedHScale = 1;
                using (Bitmap bmp = new Bitmap(filename))
                {
                    int suggestedHScaleByWidth = (int)Math.Ceiling((double)bmp.Width / Console.WindowWidth);
                    int suggestedHScaleByHeight = (int)Math.Ceiling((double)bmp.Height / Console.WindowHeight / 2);
                    suggestedHScale = Math.Max(suggestedHScaleByWidth, suggestedHScaleByHeight);
                }
                // Set scale
                Application.EnableVisualStyles();
                ScaleBox sb = new ScaleBox(suggestedHScale);
                Application.Run(sb);
                if (sb.DialogResult != DialogResult.OK) { return; }
                int hScale = sb.HScale;
                int vScale = hScale * 2; // Good for console output

                // Draw image
                Stopwatch watch = new Stopwatch();
                watch.Start();
                try
                {
                    using (Bitmap bmp = new Bitmap(filename))
                    {
                        FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
                        int frameCount = bmp.GetFrameCount(fd);
                        if (frameCount > 1) // For GIF
                        {
                            int i = 0;
                            while (i <= frameCount) // Loop playback
                            {
                                for (i = 0; i < frameCount; i += 2)
                                {
                                    bmp.SelectActiveFrame(fd, i);
                                    Console.CursorVisible = false;
                                    Console.SetCursorPosition(0, 0); // Refresh frame
                                    Console.WriteLine(GetChars(bmp, hScale, vScale));
                                }
                            }
                        }
                        else // For other image formats
                        {
                            Console.CursorVisible = false;
                            Console.WriteLine(GetChars(bmp, hScale, vScale));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
                watch.Stop();
                Console.WriteLine($"\n\x1b[38;2;200;200;200mElapsed time: {watch.ElapsedMilliseconds}ms");
            }
            else if (converterMode == ConverterMode.Video)
            {
                VideoConverter v = new VideoConverter(filename);
                int minHScaleByWidth = (int)Math.Ceiling((double)v.OriginalWidth / Console.WindowWidth);
                int minHScaleByHeight = (int)Math.Ceiling((double)v.OriginalHeight / Console.WindowHeight / 2);
                int minHScale = Math.Max(minHScaleByWidth, minHScaleByHeight);
                v.SetScale(minHScale);
                v.CreatePipe();
                v.Decode();
            }

            Console.WriteLine($"\x1b[38;2;200;200;200mPress any key to exit...");
            Console.ReadKey();
        }

        private static string GetChars(Bitmap bmp, int hScale, int vScale)
        {
            // Get pixels
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr ptr = bd.Scan0;
            int width = Math.Abs(bd.Stride);
            int length = width * bmp.Height;
            byte[] pixels = new byte[length];
            Marshal.Copy(ptr, pixels, 0, length);

            // Get chars
            StringBuilder sb = new StringBuilder();
            for (int h = 0; h < bmp.Height; h += vScale)
            {
                for (int w = 0; w < width; w += 4 * hScale) // B,G,R,A
                {
                    byte r = pixels[h * width + w + 2];
                    byte g = pixels[h * width + w + 1];
                    byte b = pixels[h * width + w];
                    double gray = r * 0.30 + g * 0.59 + b * 0.11;
                    char ch = GetChar(gray);
                    // Refer to NativeMethods class
                    sb.Append($"\x1b[38;2;{r};{g};{b}m{ch}");
                }
                sb.AppendLine();
            }
            bmp.UnlockBits(bd);

            return sb.ToString();
        }

        private static readonly List<char> listChar = 
            new List<char>() { ' ', '^', '+', '!', '$', '#', '*', '%', '@' };
        private static char GetChar(double gray)
        {
            int index = (int)(gray / 256 * listChar.Count);
            return listChar[index];
        }
    }

    public enum ConverterMode
    {
        Image,
        Video
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Img2ColorfulChars
{
    internal class ImageConverter : IDisposable
    {
        public static string FilterString
        {
            get
            {
                string imageExtensions = "";
                SupportedFormats.ForEach(u => imageExtensions += $"*{u};");
                return imageExtensions.Remove(imageExtensions.Length - 1);
            }
        }
        public static readonly List<string> SupportedFormats = new List<string>()
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".ico",
            ".tiff", ".gif"
        };

        public string ImagePath { get; }
        public bool IsValid { get; } = true;

        private Bitmap bmp;
        private int hScale;
        private int vScale;

        private ImageConverter() { }

        public ImageConverter(string filename)
        {
            ImagePath = filename;
            try { bmp = new Bitmap(ImagePath); }
            catch (ArgumentException)
            {
                Console.WriteLine("Unable to open this file.");
                IsValid = false;
            }
        }

        public int GetSuggestedHScale()
        {
            int suggestedHScaleByWidth = (int)Math.Ceiling((double)bmp.Width / Console.WindowWidth);
            int suggestedHScaleByHeight = (int)Math.Ceiling((double)bmp.Height / Console.WindowHeight / 2);
            return Math.Max(suggestedHScaleByWidth, suggestedHScaleByHeight);
        }

        public void SetScale(int scale)
        {
            hScale = scale;
            vScale = hScale * 2; // Good for console output
        }

        public void Draw()
        {
            FrameDimension fd = new FrameDimension(bmp.FrameDimensionsList[0]);
            int frameCount = bmp.GetFrameCount(fd);
            if (frameCount > 1) // For GIF
            {
                int i = 0;
                while (i <= frameCount) // Loop playback
                {
                    for (i = 0; i < frameCount; i += 2) // Speed up to 2x
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

        private string GetChars(Bitmap bmp, int hScale, int vScale)
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

        private readonly List<char> listChar =
            new List<char>() { ' ', '^', '+', '!', '$', '#', '*', '%', '@' };
        private char GetChar(double gray)
        {
            int index = (int)(gray / 256 * listChar.Count);
            return listChar[index];
        }

        public void Dispose()
        {
            bmp?.Dispose();
            bmp = null;
        }
    }
}

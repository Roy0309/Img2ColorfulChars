using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Img2ColorfulChars
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Maximize console
            NativeMethods.ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 3); //SW_MAXIMIZE = 3
            // Set flag for colorful output
            var handle = NativeMethods.GetStdHandle(-11); // STD_OUTPUT_HANDLE = -11
            NativeMethods.GetConsoleMode(handle, out int mode);
            NativeMethods.SetConsoleMode(handle, mode | 0x4); // ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4

            string filename;
            int hScale = 0;
            if (args.Length != 2)
            {
                // Load image path
                OpenFileDialog ofd = new OpenFileDialog
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = "Image(*.jpg;.jpeg;*.png;.bmp;.ico;.tiff)|*.jpg;.jpeg;*.png;.bmp;.ico;.tiff|All files(*.*)|*.*",
                    Multiselect = false,
                    Title = "Select an image to process..."
                };
                ofd.ShowDialog();
                if (string.IsNullOrEmpty(ofd.FileName)) { return; }
                filename = ofd.FileName;

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
                hScale = sb.HScale;
            }
            else // Input arguments
            {
                // Load image path
                filename = args[0];
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Failed: File not found.");
                    Environment.Exit(-1);
                }

                // Set scale
                bool validScale = int.TryParse(args[1], out hScale);
                if (!validScale)
                {
                    Console.WriteLine("Failed: Scale should be positive integar.");
                    Environment.Exit(-2);
                }     
            }
            int vScale = hScale * 2; // Good for console output

            // Save original color
            var consoleColor = Console.ForegroundColor;

            // Draw image
            try
            {
                using (Bitmap bmp = new Bitmap(filename))
                {
                    GetChars(bmp, hScale, vScale, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // Recover color
            Console.ForegroundColor = consoleColor;
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static string GetChars(Bitmap bmp, int hScale, int vScale, bool shouldDraw)
        {
            StringBuilder sb = new StringBuilder();
            for (int h = 0; h < bmp.Height; h += vScale)
            {
                for (int w = 0; w < bmp.Width; w += hScale)
                {
                    Color color = bmp.GetPixel(w, h);
                    float brightness = color.GetBrightness();
                    char ch = GetChar(brightness);
                    if (shouldDraw)
                    {
                        // Refer to NativeMethods class
                        Console.Write($"\x1b[38;2;{color.R};{color.G};{color.B}m{ch}");
                    }
                    sb.Append(ch);
                }
                if (shouldDraw) { Console.WriteLine(); }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static readonly List<char> listChar = 
            new List<char>() { ' ', '^', '+', '!', '$', '#', '*', '%', '@' };
        private static char GetChar(float brightness)
        {
            int index = (int)(brightness * 0.99 * listChar.Count);
            return listChar[index];
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
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
                Filter = string.Format("Media({0};{1})|{0};{1}", ImageConverter.FilterString, VideoConverter.FilterString),
                Multiselect = false,
                Title = "Select an image or video to convert..."
            };
            ofd.ShowDialog();
            if (string.IsNullOrEmpty(ofd.FileName)) { return; }
            string filename = ofd.FileName;

            // Set converter mode
            ConverterMode converterMode = ConverterMode.Unsupported;
            string extension = new FileInfo(filename).Extension;
            if (VideoConverter.SupportedFormats.Contains(extension))
            {
                converterMode = ConverterMode.Video;
            }
            else if (ImageConverter.SupportedFormats.Contains(extension))
            {
                converterMode = ConverterMode.Image;
            }

            // Draw
            Application.EnableVisualStyles();
            ScaleBox scaleBox;
            switch (converterMode)
            {
                case ConverterMode.Image:
                    ImageConverter ic = new ImageConverter(filename);
                    if (!ic.IsValid) { break; }
                    scaleBox = new ScaleBox(ic.GetSuggestedHScale());
                    Application.Run(scaleBox);
                    if (scaleBox.DialogResult != DialogResult.OK) { return; }
                    ic.SetScale(scaleBox.HScale);
                    ic.Draw();
                    break;
                case ConverterMode.Video:
                    VideoConverter vc = new VideoConverter(filename);
                    scaleBox = new ScaleBox(vc.GetSuggestedHScale());
                    Application.Run(scaleBox);
                    if (scaleBox.DialogResult != DialogResult.OK) { return; }
                    vc.SetScale(scaleBox.HScale);
                    vc.CreatePipe();
                    vc.Decode();
                    break;
                default:
                    Console.WriteLine($"\x1b[38;2;200;200;200mUnsupported format.");
                    break;
            }

            // Exit
            Console.WriteLine($"\x1b[38;2;200;200;200mPress any key to exit...");
            Console.ReadKey();
        }
    }

    public enum ConverterMode
    {
        Image,
        Video,
        Unsupported
    }
}

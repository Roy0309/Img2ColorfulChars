using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Img2ColorfulChars
{
    internal class VideoConverter
    {
        public static string FFmpegPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
        public static string FilterString
        {
            get
            {
                string videoExtensions = "";
                SupportedFormats.ForEach(u => videoExtensions += $"*{u};");
                return videoExtensions.Remove(videoExtensions.Length - 1);
            }
        }
        public static readonly List<string> SupportedFormats = new List<string>()
        {
            ".3gp", ".asf", ".avi", ".flv", ".m4v",
            ".mov", ".mp4", ".mpg", ".mpeg", ".mkv",
            ".rm", ".rmvb", ".wmv"
        };

        public string VideoPath { get; }
        public int OriginalWidth { get; private set; }
        public int OriginalHeight { get; private set; }
        public string Duration { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Rate { get; private set; } = 8;

        public event FrameReceivedEventHandler FrameReceived;

        private readonly bool toolExists = false;
        private const string pipeName = "I2CCVideoOutput";
        private bool pipeCreated = false;

        private VideoConverter() { }

        public VideoConverter(string filename)
        {
            toolExists = CheckFFmpegExists();
            VideoPath = filename;
            GetVideoInfo();
            FrameReceived += (o, e) =>
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0); // Refresh frame
                Console.WriteLine(GetChars(e.FrameData, Width, Height));
            };
        }

        private bool CheckFFmpegExists()
        {
            return !string.IsNullOrEmpty(FFmpegPath) && File.Exists(FFmpegPath);
        }

        public int GetSuggestedHScale()
        {
            int suggestedHScaleByWidth = (int)Math.Ceiling((double)OriginalWidth / Console.WindowWidth);
            int suggestedHScaleByHeight = (int)Math.Ceiling((double)OriginalHeight / Console.WindowHeight / 2);
            return Math.Max(suggestedHScaleByWidth, suggestedHScaleByHeight);
        }

        public void SetScale(int scale)
        {
            Width = OriginalWidth / scale;
            Height = OriginalHeight / scale / 2;
            Debug.WriteLine($"Info: Expected to set output size {Width}x{Height}.");
            // TODO: Fix awkward output with oversize resolution.
            if (Width > 190)
            {
                Width = 190;
                Height = (int)((double)OriginalHeight / OriginalWidth * Width / 2);
                Debug.WriteLine($"Warning: Oversize width. Revise size to {Width}x{Height}.");
            }
            if (Height > 95)
            {
                Height = 95;
                Width = (int)((double)OriginalWidth / OriginalHeight * Height * 2);
                Debug.WriteLine($"Warning: Oversize height. Revise size to {Width}x{Height}.");
            }
        }

        public void Decode()
        {
            if (!toolExists) { throw new FileNotFoundException("Failed: FFmpeg not found."); }
            if (Width + Height == 0) { Console.WriteLine("Failed: Invalid output size."); return; }

            Process p = new Process();
            p.StartInfo.FileName = FFmpegPath;
            p.StartInfo.Arguments = $"-y -i \"{VideoPath}\" " +
                $"-an -vf scale={Width}:{Height} " +
                $"-r {Rate} -pix_fmt rgb24 " +
               $@"-vcodec rawvideo -f image2pipe \\.\pipe\{pipeName}";
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.UseShellExecute = true;
            p.EnableRaisingEvents = true;
            p.ErrorDataReceived += (o, e) => { Debug.WriteLine(e.Data); };
            p.Exited += (o, e) => { Debug.WriteLine("Info: FFmpeg exited."); };
            p.Start();          
            p.WaitForExit();
        }

        public void CreatePipe()
        {
            if (!toolExists) { throw new FileNotFoundException("Failed: FFmpeg not found."); }
            if (pipeCreated) { Console.WriteLine("Failed: Duplicated pipe."); return; }

            Task.Run(async () =>
            {
                using (NamedPipeServerStream ps = new NamedPipeServerStream(pipeName,
                    PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous,
                    int.MaxValue, int.MaxValue))
                {
                    Debug.WriteLine($"Success: Pipe '{pipeName}' created.");
                    pipeCreated = true;
                    await ps.WaitForConnectionAsync();
                    byte[] data = new byte[Width * Height * 3];

                    while (ps.Read(data, 0, data.Length) > 0)
                    {
                        FrameReceived?.Invoke(this,
                            new FrameReceivedEventArgs() { FrameData = data });
                    }
                    ps.Disconnect();
                    Debug.WriteLine($"Info: No data remaining. Pipe disconnected.");
                }
            });
        }

        private void GetVideoInfo()
        {
            if (!toolExists) { throw new FileNotFoundException("Failed: FFmpeg not found."); }

            Process p = new Process();
            p.StartInfo.FileName = FFmpegPath;
            p.StartInfo.Arguments = $"-i \"{VideoPath}\"";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            p.WaitForExit();
            string output = p.StandardError.ReadToEnd();

            try
            {
                string fileInfo = output.Substring(output.IndexOf("Duration:"));

                // Duration
                Duration = fileInfo.Substring(10, 8);
                Debug.WriteLine($"Success: Video duration {Duration}.");

                // Rotation
                bool shouldRotate = false;
                int rotateIndex = fileInfo.IndexOf("rotate          : ");
                if (rotateIndex > 0)
                {
                    string rotation = fileInfo.Substring(rotateIndex + 18, 3).Trim();
                    shouldRotate = rotation == "90" || rotation == "270";
                    Debug.WriteLine($"Success: Video rotation {shouldRotate}.");
                } 

                // Resolution
                string videoInfo = fileInfo.Substring(fileInfo.IndexOf("Video:") + 7,
                    fileInfo.IndexOf(" (default)") - fileInfo.IndexOf("Video:") - 7);
                Regex r = new Regex("([0-9]{2,}x[0-9]+)");
                string resolution = r.Match(videoInfo).Value;
                int tmpWidth = int.Parse(resolution.Substring(0, resolution.IndexOf('x')));
                int tmpHeight = int.Parse(resolution.Substring(resolution.IndexOf('x') + 1));
                OriginalWidth = shouldRotate ? tmpHeight : tmpWidth;
                OriginalHeight = shouldRotate ? tmpWidth : tmpHeight;
                Debug.WriteLine($"Success: Video resolution {OriginalWidth}x{OriginalHeight}.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed: Unable to get video info.\n" + e);
                Duration = "0";
                OriginalHeight = 0;
                OriginalWidth = 0;
            }
        }

        private string GetChars(byte[] frameData, int frameWidth, int frameHeight)
        {
            StringBuilder sb = new StringBuilder();
            for (int h = 0; h < frameHeight; h++)
            {
                for (int w = 0; w < frameWidth * 3; w += 3)
                {
                    byte r = frameData[h * frameWidth * 3 + w];
                    byte g = frameData[h * frameWidth * 3 + w + 1];
                    byte b = frameData[h * frameWidth * 3 + w + 2];
                    sb.Append($"\x1b[38;2;{r};{g};{b}m@"); // Better than displaying different chars
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public delegate void FrameReceivedEventHandler(object sender, FrameReceivedEventArgs args);

    public class FrameReceivedEventArgs : EventArgs
    {
        public byte[] FrameData { get; set; }
    }
}

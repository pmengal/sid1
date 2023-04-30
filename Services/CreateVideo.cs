using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SID1.Services
{
    internal class Video
    {
        /// <summary>
        /// Creates the video from the specified input.
        /// </summary>
        /// <param name="inputAudio">The input audio.</param>
        /// <param name="output">The output.</param>
        internal static async Task Create(string inputAudio, string backgroundVideo, string outputVideo)
        {
            Log.Information("Creating video to {outputVideo}", outputVideo);

            if (File.Exists(outputVideo))
                File.Delete(outputVideo);

            // More ideas to integrate https://hhsprings.bitbucket.io/docs/programming/examples/ffmpeg/audio_visualization/index.html
            string arguments = $"-y -stream_loop -1 -i \"{backgroundVideo}\" -i \"{inputAudio}\" -filter_complex \"[0:v]scale=1280:720,setpts=PTS-STARTPTS[v1];" +
                $"[1:a]showcqt=1280x720:basefreq=27.5:endfreq=4186.0[a1];" +
                $"[a1]format=yuva420p,colorchannelmixer=aa=0.5[a2];" + // [a1]format=yuva420p,colorchannelmixer=aa=0.5[a2];
                $"[v1][a2]overlay=0:0:format=auto,format=yuv420p\" -c:v libx264 -preset superfast -crf 18 -c:a aac -b:a 192k -movflags +faststart -shortest \"{outputVideo}\"";

            Process process = new Process();
            process.StartInfo.FileName = "ffmpeg.exe"; // Imply it's installed and available system wide
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += (sender, e) => Log.Debug(e.Data);
            process.ErrorDataReceived += (sender, e) => Log.Debug(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();

            File.Delete(inputAudio);

            Log.Information("Done :) > {videoPath}", outputVideo);
        }
    }
}

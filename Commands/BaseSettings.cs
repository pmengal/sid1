using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SID1.Commands
{
    /// <summary>
    /// Base settings for all the compatible commands.
    /// Implements the <see cref="CommandSettings" />
    /// </summary>
    /// <seealso cref="CommandSettings" />
    internal class BaseSettings : CommandSettings
    {
        [Description("Path to background video.")]
        [CommandOption("-b|--background")]
        public string? BackgroundVideo { get; init; }

        [Description("Path to input audio file.")]
        [CommandOption("-i|--input")]
        public string? InputText { get; init; }

        [Description("Path to output video file.")]
        [CommandOption("-o|--ouput")]
        public string? OutputVideo { get; init; }
    }
}

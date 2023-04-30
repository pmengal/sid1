using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SID1.Services;
using Microsoft.CognitiveServices.Speech;
using Serilog;
using Spectre.Console;

namespace SID1.Commands
{
    /// <summary>
    /// Creates video based on Azure Speech Services audio generated content. This class cannot be inherited.
    /// Implements the <see cref="Spectre.Console.Cli.AsyncCommand{SID1.Commands.AzureCommand.Settings}" />
    /// </summary>
    /// <seealso cref="Spectre.Console.Cli.AsyncCommand{SID1.Commands.AzureCommand.Settings}" />
    internal sealed class AzureCommand : AsyncCommand<AzureCommand.Settings>
    {
        public sealed class Settings : BaseSettings
        {
            [Description("Key of Azure Speech Services. Defaults to current directory.")]
            [CommandOption("-k|--key")]
            public string? Key { get; init; }

            [Description("Azure region to use. Defaults to westeurope.")]
            [CommandOption("-r|--region")]
            [DefaultValue("westeurope")]
            public string? Region { get; init; }

            [Description("Language to use. Defaults to fr-FR.")]
            [CommandOption("-l|--language")]
            [DefaultValue("fr-FR")]
            public string? Language { get; init; }

            [Description("Voice to use. Defaults to fr-FR-BrigitteNeural.")]
            [CommandOption("-v|--voice")]
            [DefaultValue("fr-FR-BrigitteNeural")]
            public string? Voice { get; init; }
        }

        /// <summary>
        /// Execute as an asynchronous operation.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            string temporaryFile = Path.GetTempFileName();

            SpeechConfig config = SpeechConfig.FromSubscription(settings.Key, settings.Region);
            config.SpeechSynthesisLanguage = settings.Language;
            config.SpeechSynthesisVoiceName = settings.Voice;
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff24Khz16BitMonoPcm);

            Log.Information("Converting text ({inputText}) to audio ({temporaryFile})", settings.InputText, temporaryFile);

            using SpeechSynthesizer synthesizer = new(config, null);
            SpeechSynthesisResult result = await synthesizer.SpeakTextAsync(File.ReadAllText(settings.InputText));

            using AudioDataStream stream = AudioDataStream.FromResult(result);
            await stream.SaveToWaveFileAsync(temporaryFile);

            Log.Information("Creating video to {outputVideo}", settings.OutputVideo);

            if (File.Exists(settings.OutputVideo))
                File.Delete(settings.OutputVideo);

            await Video.Create(temporaryFile, settings.BackgroundVideo, settings.OutputVideo);

            File.Delete(temporaryFile);

            Log.Information("Done :) > {videoPath}", settings.OutputVideo);

            return 0;
        }
    }
}
// language

//fr-BE
//fr-CA
//fr-CH
//fr-FR

//voice

//fr-BE-CharlineNeural(Female)
//fr-BE-GerardNeural(Male)

//fr-CA-AntoineNeural(Male)
//fr-CA-JeanNeural(Male)
//fr-CA-SylvieNeural(Female)

//fr-CH-ArianeNeural(Female)
//fr-CH-FabriceNeural(Male)

//fr-FR-AlainNeural(Male)
//fr-FR-BrigitteNeural(Female)
//fr-FR-CelesteNeural(Female)
//fr-FR-ClaudeNeural(Male)
//fr-FR-CoralieNeural(Female)
//fr-FR-DeniseNeural1(Female)
//fr-FR-EloiseNeural(Female, Child)
//fr-FR-HenriNeural(Male)
//fr-FR-JacquelineNeural(Female)
//fr-FR-JeromeNeural(Male)
//fr-FR-JosephineNeural(Female)
//fr-FR-MauriceNeural(Male)
//fr-FR-YvesNeural(Male)
//fr-FR-YvetteNeural(Female)

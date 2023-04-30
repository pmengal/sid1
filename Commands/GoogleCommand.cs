using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Microsoft.CognitiveServices.Speech;
using Serilog;
using SID1.Services;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SID1.Commands
{
    /// <summary>
    /// Creates video based on Azure Speech Services audio generated content. This class cannot be inherited.
    /// Implements the <see cref="Spectre.Console.Cli.AsyncCommand{SID1.Commands.GoogleCommand.Settings}" />
    /// </summary>
    /// <seealso cref="Spectre.Console.Cli.AsyncCommand{SID1.Commands.GoogleCommand.Settings}" />
    internal sealed class GoogleCommand : AsyncCommand<GoogleCommand.Settings>
    {
        public sealed class Settings : BaseSettings
        {
            [Description("Private key of Google Cloud Text2Speech to use.")]
            [CommandOption("-k|--key")]
            public string? PrivateKey { get; init; }

            [Description("Client email of Google Cloud Text2Speech to use.")]
            [CommandOption("-c|--clientemail")]
            public string? ClientEmail { get; init; }

            [Description("Language to use. Defaults to fr-FR.")]
            [CommandOption("-l|--language")]
            [DefaultValue("fr-FR")]
            public string? Language { get; init; }

            [Description("Voice to use. Defaults to fr-FR-Standard-B.")]
            [CommandOption("-v|--voice")]
            [DefaultValue("fr-FR-Standard-B")]
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

            ServiceAccountCredential.Initializer initializer = new ServiceAccountCredential.Initializer(settings.ClientEmail) { Scopes = TextToSpeechClient.DefaultScopes }.FromPrivateKey(settings.PrivateKey);
            ServiceAccountCredential credential = new(initializer);
            GoogleCredential googleCredential = GoogleCredential.FromServiceAccountCredential(credential);
            TextToSpeechClientBuilder builder = new()
            {
                GoogleCredential = googleCredential
            };

            var speechClient = builder.Build();

            SynthesisInput input = new() { Text = File.ReadAllText(settings.InputText) };
            AudioConfig audioConfig = new() { AudioEncoding = AudioEncoding.Linear16 };
            VoiceSelectionParams @params = new()
            {
                LanguageCode = settings.Language,
                Name = settings.Voice
            };

            Log.Information("Converting text ({inputText}) to audio ({temporaryFile})", settings.InputText, temporaryFile);

            SynthesizeSpeechResponse response = await speechClient.SynthesizeSpeechAsync(input, @params, audioConfig);
            using Stream output = File.Create(temporaryFile);
            response.AudioContent.WriteTo(output);

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

using System;
using Mono.Options;
using Octokit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace gist_search
{
    class Program
    {
        static Encoding GetOutputEncoding(CommandOptions opts)
        {
            if (opts.OutputFileCodePage == -1)
            {
                return new UTF8Encoding(false);
            }
            else
            {
                return Encoding.GetEncoding(opts.OutputFileCodePage);
            }
        }
        static TextWriter GetTextWriter(CommandOptions opts)
        {
            if (string.IsNullOrEmpty(opts.OutputFilePath))
            {
                return new StreamWriter(Console.OpenStandardOutput(), GetOutputEncoding(opts));
            }
            else
            {
                return new StreamWriter(File.Create(opts.OutputFilePath), GetOutputEncoding(opts));
            }
        }
        static IEnumerable<Gist> FilterItems(IReadOnlyList<Gist> items, CommandOptions opts)
        {
            return items.Where(x =>
            {
                bool ret = true;
                if (!string.IsNullOrEmpty(opts.DescriptionRegex))
                {
                    ret = Regex.IsMatch(x.Description, opts.DescriptionRegex);
                }
                if (!ret && !string.IsNullOrEmpty(opts.FileNameRegex))
                {
                    ret = x.Files.Any(f => Regex.IsMatch(f.Value.Filename, opts.FileNameRegex));
                }
                return ret;
            });
        }
        static async Task Main(string[] args)
        {
            try
            {
                var remaining = CommandOptions.Instance.Parse(args);
                if (CommandOptions.Instance.IsHelp)
                {
                    CommandOptions.Instance.OutputHelp(Console.Out);
                    return;
                }
                var client = new GitHubClient(new ProductHeaderValue("gist-search"));
                var gists = await client.Gist.GetAllForUser(CommandOptions.Instance.UserName).ConfigureAwait(false);
                using (var writer = GetTextWriter(CommandOptions.Instance))
                {
                    Jil.JSON.Serialize(FilterItems(gists, CommandOptions.Instance).ToArray(), writer);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"failed to request:{e}");
                CommandOptions.Instance.OutputHelp(Console.Error);
                Environment.ExitCode = 1;
                return;
            }
        }
    }
}

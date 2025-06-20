using ChangeLogger.Action.Shared;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ChangeLogger.Action.Features.UpdateChangeLog;

public class ChangeLogTransformer : IChangeLogTransformer
{
    private const string UnreleasedHeader = "## [Unreleased]";
    private const string UnreleasedLinkPrefix = "[unreleased]: https://github.com/";

    public string Update(string logContent, Inputs inputs)
    {
        ValidateUnreleasedHeader(logContent);
        
        var previousVersion = DeterminePreviousVersion(logContent);

        var newLogContent = ReplaceUnreleasedSection(logContent, inputs);

        return UpdateTagLinks(newLogContent, inputs, previousVersion);
    }

    public string ExtractUnreleasedContent(string logContent)
    {
        ValidateUnreleasedHeader(logContent);
        
        var lines = logContent.Split('\n');
        var unreleasedIndex = -1;
        var nextReleaseIndex = -1;

        // Find the [Unreleased] header
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim() == UnreleasedHeader)
            {
                unreleasedIndex = i;
                break;
            }
        }

        if (unreleasedIndex == -1)
        {
            return string.Empty;
        }

        // Find the next release header (## [version] - date)
        for (var i = unreleasedIndex + 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.StartsWith("## [") && line.Contains("] - "))
            {
                nextReleaseIndex = i;
                break;
            }
        }

        // Extract content between headers
        var startIndex = unreleasedIndex + 1;
        var endIndex = nextReleaseIndex == -1 ? lines.Length : nextReleaseIndex;
        
        var releaseNotesLines = new string[endIndex - startIndex];
        Array.Copy(lines, startIndex, releaseNotesLines, 0, endIndex - startIndex);
        
        return string.Join('\n', releaseNotesLines).Trim();
    }

    private static string UpdateTagLinks(string logContent, Inputs inputs, string? previousVersion)
    {
        var unreleasedLink = FindUnreleasedLink(logContent);
        string releaseLink;

        if (unreleasedLink is not null)
        {
            // link exists
            releaseLink =
                $"[{inputs.Tag}]: https://github.com/{inputs.Repository.Owner.Name}/{inputs.Repository.Name}/compare/{previousVersion}...{inputs.Tag}";
            var newUnreleasedLink =
                $"{UnreleasedLinkPrefix}{inputs.Repository.Owner.Name}/{inputs.Repository.Name}/compare/{inputs.Tag}...HEAD";

            return logContent.Replace(unreleasedLink, $"{newUnreleasedLink}\n{releaseLink}");
        }

        // link does not exist, add it
        releaseLink =
            $"[{inputs.Tag}]: https://github.com/{inputs.Repository.Owner.Name}/{inputs.Repository.Name}/compare/{inputs.Repository.GenesisHash}...{inputs.Tag}";
        var initialUnreleasedLink =
            $"{UnreleasedLinkPrefix}{inputs.Repository.Owner.Name}/{inputs.Repository.Name}/compare/{inputs.Tag}...HEAD";

        return $"{logContent.TrimEnd()}\n\n{initialUnreleasedLink}\n{releaseLink}";
    }

    private static string? FindUnreleasedLink(string logContent)
    {
        var lines = logContent.Split('\n');
        return lines.FirstOrDefault(line => line.StartsWith(UnreleasedLinkPrefix));
    }

    private static string ReplaceUnreleasedSection(string logContent, Inputs inputs)
    {
        var newHeading = $"## [{inputs.Tag}] - {inputs.Date:yyyy-MM-dd}";
        var replacement = $"{UnreleasedHeader}\n\n{newHeading}";
        return logContent.Replace(UnreleasedHeader, replacement);
    }

    private static string? DeterminePreviousVersion(string logContent)
    {
        var document = Markdown.Parse(logContent);
        var releases = document.Descendants<LinkReferenceDefinition>()
            .ToList();

        return releases.Count <= 1 ? null : releases[1].Label;
    }

    private static void ValidateUnreleasedHeader(string logContent)
    {
        var document = Markdown.Parse(logContent);
        var matchingHeaders = document.Descendants<HeadingBlock>()
            .Where(h => h is {Level: 2} && IsUnreleasedHeader(h));
        var header = matchingHeaders.FirstOrDefault();
        
        if (header is null)
        {
            throw new InvalidChangeLogException(
                "[Unreleased] section not found.");
        }
    }

    private static bool IsUnreleasedHeader(HeadingBlock headingBlock)
    {
        if (headingBlock.Inline is not { } container) return false;
        
        return container.FirstChild switch
        {
            LinkInline {FirstChild: LiteralInline literalInline} => literalInline.Content.ToString()
                .Equals("Unreleased", StringComparison.OrdinalIgnoreCase),
            LiteralInline when string.Join("", container.FindDescendants<LiteralInline>())
                .Equals("[Unreleased]", StringComparison.OrdinalIgnoreCase) => true,
            _ => false
        };
    }
}

public class InvalidChangeLogException(string message) : Exception(message);
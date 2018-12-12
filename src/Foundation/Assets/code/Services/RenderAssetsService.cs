using System;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore;
using WageWorks.Foundation.Assets.Models;
using WageWorks.Foundation.Assets.Repositories;

namespace WageWorks.Foundation.Assets.Services
{
    /// <summary>
    ///     A service which helps add the required JavaScript at the end of a page, and CSS at the top of a page.
    ///     In component based architecture it ensures references and inline scripts are only added once.
    /// </summary>
    public class RenderAssetsService
    {
        private static RenderAssetsService _current;
        public static RenderAssetsService Current => _current ?? (_current = new RenderAssetsService());

        public HtmlString RenderScript(ScriptLocation location, bool bustCache)
        {
            var assets = AssetRepository.Current.Items.Where(asset => (asset.Type == AssetType.JavaScript || asset.Type == AssetType.Raw) && asset.Location == location && this.IsForContextSite(asset));

            var sb = new StringBuilder();
            foreach (var item in assets)
            {
                if (item.Type == AssetType.Raw)
                {
                    sb.Append(item.Content).AppendLine();
                }
                else
                {
                    switch (item.ContentType)
                    {
                        case AssetContentType.File:
                            sb.AppendFormat("<script src=\"{0}\"></script>",
                                (bustCache
                                ? RenderCacheBustUrl(item.Content).ToString()
                                : item.Content)).AppendLine();
                            break;
                        case AssetContentType.Inline:
                            if (item.Type == AssetType.Raw)
                            {
                                sb.AppendLine(HttpUtility.HtmlDecode(item.Content));
                            }
                            else
                            {
                                if (item.UseDocumentReady)
                                {
                                    sb.AppendLine("<script type=\"text/javascript\">\njQuery(document).ready(function() {");
                                    sb.AppendLine(item.Content);
                                    sb.AppendLine("});\n</script>");
                                }
                                else
                                {
                                    sb.AppendLine("<script type=\"text/javascript\">");
                                    sb.AppendLine(item.Content);
                                    sb.AppendLine("\n</script>");
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return new HtmlString(sb.ToString());
        }

        public HtmlString RenderScript(ScriptLocation location)
        {
            return RenderScript(location, false);
        }

        public HtmlString RenderStyles(bool bustCache)
        {
            var sb = new StringBuilder();
            foreach (var item in AssetRepository.Current.Items.Where(asset => asset.Type == AssetType.Css && this.IsForContextSite(asset)))
            {
                switch (item.ContentType)
                {
                    case AssetContentType.File:
                        sb.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />",
                            (bustCache
                            ? RenderCacheBustUrl(item.Content).ToString()
                            : item.Content)).AppendLine();
                        break;
                    case AssetContentType.Inline:
                        sb.AppendLine("<style type=\"text/css\">");
                        sb.AppendLine(item.Content);
                        sb.AppendLine("</style>");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new HtmlString(sb.ToString());
        }

        public HtmlString RenderStyles()
        {
            return RenderStyles(false);
        }

        public static HtmlString RenderCacheBustUrl(string filePath)
        {
            //skip absolute paths
            if (filePath.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                return new HtmlString(filePath);

            var file = HttpContext.Current.Server.MapPath(filePath);
            var dateModified = System.IO.File.GetLastWriteTime(file);
            return new HtmlString($"{filePath}?v={dateModified:yyyyMMddHHmmss}");
        }

        private bool IsForContextSite(Asset asset)
        {
            if (asset.Site == null)
            {
                return true;
            }

            foreach (var part in asset.Site.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries))
            {
                var siteWildcard = part.Trim().ToLowerInvariant();
                if (siteWildcard == "*" || Context.Site.Name.Equals(siteWildcard, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
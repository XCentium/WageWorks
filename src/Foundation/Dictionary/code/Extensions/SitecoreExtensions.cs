using System.Web;
using Sitecore.Mvc.Helpers;
using WageWorks.Foundation.Dictionary.Repositories;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Foundation.Dictionary.Extensions
{
    public static class SitecoreExtensions
  {
    public static string Dictionary(this SitecoreHelper helper, string relativePath, string defaultValue = "")
    {
      return DictionaryPhraseRepository.Current.Get(relativePath, defaultValue);
    }

    public static string DictionaryWithFormat(this SitecoreHelper helper, string relativePath, string defaultValue, params object[] args)
    {
        var stringFormat = DictionaryPhraseRepository.Current.Get(relativePath, defaultValue);
        return string.Format(stringFormat, args);
    }

    public static HtmlString DictionaryField(this SitecoreHelper helper, string relativePath, string defaultValue = "")
    {
      var item = DictionaryPhraseRepository.Current.GetItem(relativePath, defaultValue);
      if (item == null)
        return new HtmlString(defaultValue);
      return helper.Field(Templates.DictionaryEntry.Fields.Phrase, item);
    }
  }
}
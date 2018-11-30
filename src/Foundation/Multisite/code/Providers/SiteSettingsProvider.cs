using System;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Wageworks.Foundation.DependencyInjection;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Foundation.Multisite.Providers
{
    [Service(typeof(ISiteSettingsProvider))]
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly SiteContext siteContext;

        public SiteSettingsProvider(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public static string SettingsRootName => Settings.GetSetting("Multisite.SettingsRootName", "settings");

        public virtual Item GetSetting(Item contextItem, string settingsName, string setting)
        {
            var settingsRootItem = this.GetSettingsRoot(contextItem, settingsName);
            var settingItem = settingsRootItem?.Children.FirstOrDefault(i => i.Key.Equals(setting.ToLower()));
            return settingItem;
        }

        private Item GetSettingsRoot(Item contextItem, string settingsName)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];
            var settingsRootItem = settingsFolder?.Children.FirstOrDefault(i => i.IsDerived(Templates.SiteSettings.ID) && i.Key.Equals(settingsName.ToLower()));
            return settingsRootItem;
        }

        public Item GetSiteSettingsRoot(Item contextItem)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);

            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;

            var settingsRootItem = definitionItem.Children.FirstOrDefault(x => x.IsDerived(Templates.SiteSettingsRoot.ID));

            return settingsRootItem;
        }

        #region -- Site Configuration --

        public Item GetConfigSetting(string SettingsCategory, string Name, string Category)
        {
            return GetConfigSetting(Sitecore.Context.Item, SettingsCategory, Name, Category);
        }
        public Item GetConfigSetting(Item contextItem, string settingsCategory, string name, string category)
        {

            Item returnItem = null;
    
            var db = Sitecore.Context.Database;
            if (db == null) return null;

            if (Sitecore.Context.Site != null)
            {
                var settingsPath = Sitecore.Context.Site.Properties.Get("siteSettingsItemPath");
                if (string.IsNullOrWhiteSpace(settingsPath)) return null;

                try
                {
                    var item = db.SelectSingleItem(settingsPath);
                    if (item != null && item.IsDerived(Templates.SiteSettingsRoot.ID))
                    {
                        var configurationSettingsRootItem = item.Children
                            .FirstOrDefault(
                                x =>
                                    x.IsDerived(Templates.ConfigurationSettingFolder.ID) &&
                                    x.Name.ToLower().Equals(settingsCategory.ToLower()));

                        var configItem = configurationSettingsRootItem?.Children.FirstOrDefault(i =>
                            i.IsDerived(Templates.ConfigurationSetting.ID) && i.Fields[Templates.ConfigurationSetting.Fields.Name].Value.ToLower()
                            .Equals(name.ToLower()) && i.Fields[Templates.ConfigurationSetting.Fields.Category].Value.ToLower().Equals(category.ToLower()));

                        if (configItem != null)
                            returnItem = configItem;

                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error in GetConfigSetting: " + ex.Message, ex);
                }
            }
            
            return returnItem;
        }

        public string GetConfigSettingValue(string SettingsCategory, string Name, string Category, bool blnUseDescription = false)
        {
            string strOutput = String.Empty;
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {


                try
                {
                    var matchingConfig = GetConfigSetting(SettingsCategory, Name, Category);

                    if (matchingConfig != null)
                    {
                        if (blnUseDescription)
                        {
                            strOutput = matchingConfig.Fields[Templates.ConfigurationSetting.Fields.Description].Value;
                        }
                        else
                        {
                            strOutput = matchingConfig.Fields[Templates.ConfigurationSetting.Fields.Value].Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("GetConfigSettingValue: exception: " + ex.Message + "trace: " + ex.StackTrace, this);
                }
            }
            return strOutput;
        }

        public string GetConfigSettingValue(Item contextItem, string SettingsCategory, string Name, string Category, bool blnUseDescription = false)
        {
            string strOutput = String.Empty;

            try
            {
                var matchingConfig = GetConfigSetting(contextItem, SettingsCategory, Name, Category);

                if (matchingConfig != null)
                {
                    if (blnUseDescription)
                    {
                        strOutput = matchingConfig.Fields[Templates.ConfigurationSetting.Fields.Description].Value;
                    }
                    else
                    {
                        strOutput = matchingConfig.Fields[Templates.ConfigurationSetting.Fields.Value].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }

            return strOutput;
        }

        #endregion
    }
}
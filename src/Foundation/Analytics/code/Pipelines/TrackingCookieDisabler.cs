using Sitecore.Analytics;
using Sitecore.Analytics.Lookups;
using Sitecore.Configuration;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using System.Net.Http;
using System.ServiceModel.Channels;

namespace Wageworks.Foundation.Analytics.Pipelines
{
    /// <summary>
    /// Custom processor to conditionally disable analytics tracking for all countries except those listed in setting name="Analytics.TrackingCookieDisabler.AllowedCountries"
    /// </summary>
    public class TrackingCookieDisabler
    {
        private readonly List<string> _allowedCountries;

        /// <summary>
        /// Constructir - init list of allowed countries.
        /// </summary>
        public TrackingCookieDisabler()
        {
            var allowedCountries = Settings.GetSetting("Analytics.TrackingCookieDisabler.AllowedCountries");
            _allowedCountries = new List<string>();
            if (!string.IsNullOrEmpty(allowedCountries))
            {
                _allowedCountries = allowedCountries.Split('|').ToList();
            }
        }

        /// <summary>
        /// Disable analytics tracking is current country is not in the list od allowed countries (setting name="Analytics.TrackingCookieDisabler.AllowedCountries")
        /// </summary>
        /// <param name="args">The args.</param>
        public void Process(PipelineArgs args)
        {
            var currentCountry = GetCountryForCurrentIp();
            
            //we know the country, let's check to see if it's in the allowed list.
            if (!string.IsNullOrEmpty(currentCountry) && _allowedCountries.Contains(currentCountry, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            Log.Warn(
                string.IsNullOrEmpty(currentCountry)
                    ? "Cannot start tracking. Could not determine country from IP."
                    : "Cannot start tracking. Current country is not in allowed list", this);
            Tracker.Enabled = false;
        }

        /// <summary>
        /// Get country name from Sitecore GeoIP
        /// </summary>
        /// <returns></returns>
        public static string GetCountryForCurrentIp()
        {
            IPAddress ipaddress = GetIpAddressFromTracker();
            if (ipaddress == null)
            {
                var request = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                ipaddress = GetClientIpAddress(request);
            }

            string country = string.Empty;

            if (ipaddress != null)
            {
                var geoIpOptions = new GeoIpOptions
                {
                    Ip = ipaddress,
                    MillisecondsTimeout = 1000,
                    Id = GeoIpManager.IpHashProvider.ComputeGuid(ipaddress)
                };

                var geoIpResult = GeoIpManager.GetGeoIpData(geoIpOptions);

                if (geoIpResult?.GeoIpData != null)
                {
                    country = geoIpResult.GeoIpData.Country != null
                        && !geoIpResult.GeoIpData.Country.Equals("N/A")
                        ? geoIpResult.GeoIpData.Country : string.Empty;
                }
            }

            return country;
        }

        /// <summary>
        /// Get current IP address from analytics tracker
        /// </summary>
        /// <returns></returns>
        private static IPAddress GetIpAddressFromTracker()
        {
            return Tracker.Current != null
                && Tracker.Current.Interaction != null
                && Tracker.Current.Interaction.Ip != null
                ? new IPAddress(Tracker.Current.Interaction.Ip) : null;
        }

        public static IPAddress GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress);
            }
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                return IPAddress.Parse(((RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name]).Address);
            }
            return null;
        }
    }
}
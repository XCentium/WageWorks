using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;

namespace Wageworks.Foundation.Analytics.GDPR
{
    public static class GdprHelpers
    {
        public static void GiveConsent()
        {
            var manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;
            if (manager == null) return;
            if (Sitecore.Analytics.Tracker.Current.Contact.IsNew)
            {

                // Save contact to xConnect; at this point, a contact has an anonymous
                // TRACKER IDENTIFIER, which follows a specific format. Do not use the contactId overload
                // and make sure you set the ContactSaveMode as demonstrated
                Sitecore.Analytics.Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;
                manager.SaveContactToCollectionDb(Sitecore.Analytics.Tracker.Current.Contact);

                // Now that the contact is saved, you can retrieve it using the tracker identifier
                // NOTE: Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource is marked internal in 9.0 Initial and cannot be used. If you are using 9.0 Initial, pass "xDB.Tracker" in as a string.
                var trackerIdentifier = new IdentifiedContactReference(Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource, Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString("N"));

                // Get contact from xConnect, update and save the facet
                using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                {
                    try
                    {
                        var contact = client.Get<Contact>(trackerIdentifier, new Sitecore.XConnect.ContactExpandOptions());

                        if (contact == null) return;

                        var consentInfo = new ConsentInformation
                        {
                            ConsentRevoked = false,
                            DoNotMarket = false
                        };
                        //var consentInfo = xContact.GetFacet<ConsentInformation>(facet);


                        client.SetConsentInformation(contact, consentInfo);
                        client.Submit();

                        // Remove contact data from shared session state - contact will be re-loaded
                        // during subsequent request with updated facets
                        manager.RemoveFromSession(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
                        Sitecore.Analytics.Tracker.Current.Session.Contact = manager.LoadContact(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
                    }
                    catch (XdbExecutionException ex)
                    {
                        // Manage conflicts / exceptions
                        Log.Error("CONSENT INFO: Could not save consent info to xConnect", ex, typeof(GdprHelpers));
                    }
                }

            }
            else
            {
                var anyIdentifier = Sitecore.Analytics.Tracker.Current.Contact.Identifiers.FirstOrDefault();
                // Get contact from xConnect, update and save the facet
                using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
                {
                    try
                    {
                        if (anyIdentifier != null)
                        {
                            var contact = client.Get<Contact>(new IdentifiedContactReference(anyIdentifier.Source, anyIdentifier.Identifier),
                                new Sitecore.XConnect.ContactExpandOptions(ConsentInformation.DefaultFacetKey));

                            if (contact != null)
                            {
                                if (contact.ConsentInformation() != null)
                                {
                                    contact.ConsentInformation().ConsentRevoked = false;
                                    contact.ConsentInformation().DoNotMarket = false;

                                    client.SetFacet<ConsentInformation>(contact, ConsentInformation.DefaultFacetKey, contact.ConsentInformation());
                                }
                                else
                                {
                                    client.SetFacet<ConsentInformation>(contact, ConsentInformation.DefaultFacetKey, new ConsentInformation()
                                    {
                                        ConsentRevoked = false,
                                        DoNotMarket = false
                                    });
                                }

                                client.Submit();

                                // Remove contact data from shared session state - contact will be re-loaded
                                // during subsequent request with updated facets
                                manager.RemoveFromSession(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
                                Sitecore.Analytics.Tracker.Current.Session.Contact = manager.LoadContact(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
                            }
                        }
                    }
                    catch (XdbExecutionException ex)
                    {
                        // Manage conflicts / exceptions
                        Log.Error("CONSENT INFO: Could not save consent info to xConnect", ex, typeof(GdprHelpers));
                    }
                }

            }
        }

        public static ConsentInformation GetConsent()
        {
            var xConnectFacet = Sitecore.Analytics.Tracker.Current.Contact.GetFacet<Sitecore.Analytics.XConnect.Facets.IXConnectFacets>("XConnectFacets");
            ConsentInformation consentInfo = xConnectFacet.Facets[ConsentInformation.DefaultFacetKey] as ConsentInformation;
            return consentInfo;

        }
    }
}
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Sites;
using System;
using System.Linq;
using System.Web.Mvc;
using Wageworks.Feature.PageContent.Models;
using Wageworks.Foundation.ORM.Context;
using Convert = System.Convert;

namespace Wageworks.Feature.PageContent.Controllers
{
    public class PageContentController : SitecoreController
    {
        private readonly IControllerSitecoreContext _sitecoreContext;

        public PageContentController(IControllerSitecoreContext sitecoreContext)
        {
            _sitecoreContext = sitecoreContext;
        }

        [HttpGet]
        [Route("api/Wageworks/contentservices/getgenericpages")]
        public JsonResult GetGenericPages()
        {
            Item mobileItem = null;
            SiteContext siteInfo = SiteContext.Current;
            if (!string.IsNullOrEmpty(siteInfo?.Properties[Constants.Mobile.MobileAppRoot]))
            {
                var mobileAppRoot = siteInfo.Properties[Constants.Mobile.MobileAppRoot];
                mobileItem = Sitecore.Context.Database.GetItem(mobileAppRoot);
            }

            if (mobileItem == null) return Json(new { }, JsonRequestBehavior.AllowGet);
            var mobileRoot = _sitecoreContext.Cast<Mobile>(mobileItem);
            if (mobileRoot?.Children != null && mobileRoot.Children.Any())
            {
                var model = mobileRoot.Children.ToList();

                if (Request.Url != null)
                {
                    var baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}";

                    // disable media linking; they're already being fully qualified
                    //model.ToList().ForEach(c => c.Body = c.Body.Replace("/-/media", baseUrl + "/-/media"));
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }


            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("api/Wageworks/contentservices/getappzip")]
        public JsonResult GetDownloadableZip(string dateTime = null)
        {

            var downLoadUrl = string.Empty;

            var mediaItemPath = Constants.Mobile.ZipFilePath;

            var item = Sitecore.Context.Database.GetItem(mediaItemPath);
            // If zip does not exist in Sitecore, return not found
            if (item == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = Constants.Mobile.NotFound;
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            var mediaItem = new MediaItem(item);

            var filePath = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);
            filePath = StringUtil.EnsurePrefix('/', filePath);

            var lastWriteTime = mediaItem.InnerItem.Statistics.Updated;

            if (Request.Url != null)
            {
                var baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}";

                // If date sent is empty return last modified date and download url
                if (string.IsNullOrEmpty(dateTime))
                {


                    return Json(
                        new
                        {
                            success = true,
                            lastModified = lastWriteTime.ToString("MM/dd/yy HH:mm:ss"),
                            downLoadUrl = $"{filePath.Trim("/".ToCharArray())}"
                        },
                        JsonRequestBehavior.AllowGet
                    );
                }

                var requetDate = new DateTime();

                // If date sent is not parsable return 404 status and no download url
                try
                {
                    requetDate = Convert.ToDateTime(dateTime);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error($"{Constants.Mobile.GetAppZipError} - {ex.Message}", ex, this);
                    Response.StatusCode = 404;
                    Response.StatusDescription = Constants.Mobile.NotFound;
                    return Json(new { }, JsonRequestBehavior.AllowGet);
                }




                // if date sent is parseable and greater than or equal to last modified date, return status 304 with no download url 
                if (requetDate >= lastWriteTime)
                {
                    Response.StatusCode = 304;
                    Response.StatusDescription = Constants.Mobile.NotModified;

                    return Json(new { }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // if date sent is parseable and less than or last modified date, return status 200 with download url
                    downLoadUrl = $"{filePath.Trim("/".ToCharArray())}";
                    return Json(
                        new
                        {
                            success = true,
                            lastModified = lastWriteTime.ToString("MM/dd/yy HH:mm:ss"),
                            downLoadUrl = downLoadUrl
                        },
                        JsonRequestBehavior.AllowGet
                    );

                }


            }


            var output = new
            {
                success = false,
                downLoadUrl = ""
            };
            return Json(output, JsonRequestBehavior.AllowGet);
        }

    }
}

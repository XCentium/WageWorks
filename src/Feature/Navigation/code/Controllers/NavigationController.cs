using Sitecore;

namespace WageWorks.Feature.Navigation.Controllers
{
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using WageWorks.Feature.Navigation.Models;
    using WageWorks.Feature.Navigation.Repositories;
    using WageWorks.Foundation.Alerts.Extensions;
    using WageWorks.Foundation.Alerts.Models;
    using WageWorks.Foundation.Dictionary.Repositories;

    public class NavigationController : Controller
    {
        private readonly INavigationRepository navigationRepository;

        public NavigationController(INavigationRepository navigationRepository)
        {
            this.navigationRepository = navigationRepository;
        }

        public ActionResult Breadcrumb()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("Breadcrumb", items);
        }

        public ActionResult PrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenu", items);
        }

        public ActionResult SecondaryMenu()
        {
            var item = this.navigationRepository.GetSecondaryMenuItem();
            return this.View("SecondaryMenu", item);
        }


        public ActionResult MegaMenu()
        {
            var items = this.navigationRepository.GetMegaMenu();
            return this.View("MegaMenu", items);
        }

        public ActionResult NavigationLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View(items);
        }

        public ActionResult LinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenu", items);
        }

        public ActionResult ListingLinks()
        {
            var items = GetMainMenu();
            return this.View(items);
        }
        public ActionResult TopMenu()
        {
            var items = GetMainMenu();
            return this.View("TopMenu",items);
        }

        public ActionResult UtilityNav()
        {
            var items = GetMainMenu();
            return this.View("UtilityNav",items);
        }

        public ActionResult SocialNavigation()
        {
            var items = GetMainMenu();
            return this.View("SocialNavigation",items);
        }

        public ActionResult NavList()
        {
            var items = GetMainMenu();
            return this.View("NavList", items);
        }

        public ActionResult MainNav()
        {
            var items = GetMainMenu();
            return this.View("MainNav", items);
        }

        private NavigationItems GetMainMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 1, 2);
            return items;
        }
    }
}
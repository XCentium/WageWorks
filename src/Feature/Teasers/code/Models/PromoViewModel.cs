using System.Collections.Generic;
using WageWorks.Feature.Teasers.Models.Glass;

namespace WageWorks.Feature.Teasers.Models
{
    #region Product Promo
    public class ProductPromoViewModel
    {
        public string CssClass { get; set; }
        //public ProductPromotionModel Product { get; set; }
    }

    //public class ProductPromotionModel : PromotionModel
    //{
    //    public double Price { get; set; }
    //}


    #endregion

    #region Standard Promo

    public class PromoViewModel
    {
        public string CssClass { get; set; }
        public string Layout { get; set; }
        public PromotionModel PromoItem { get; set; }
    }

    public class PromotionModel
    {
        public PromotionModel()
        {
        }

        public PromotionModel(IPromotion item)
        {
            this.TitleAccentColor = item.TitleAccentColor;
            this.TitleBase = item.TitleBase;
            this.TitleBaseColor = item.TitleBaseColor;
            this.Description = item.Description;
            this.SectionTitle = item.SectionTitle;
            this.BackgroundImage = item.BackgroundImage?.Src;
            
        }

        public string TitleAccentColor { get; set; }
        public string TitleBase { get; set; }
        public string TitleBaseColor { get; set; }
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
        public string SectionTitle { get; set; }
        public string CssClass { get; set; }
        public List<CallToActionLinkModel> Links { get; set; } = new List<CallToActionLinkModel>();
    }

    #endregion

    #region Call To Action Link

    public class CallToActionLinkModel
    {
        public CallToActionLinkModel()
        {
        }

        public CallToActionLinkModel(ICallToAction cta)
        {
            this.CssClass = cta.Link?.Class;
            this.Text = cta.Link?.Text;
            this.Target = cta.Link?.Target;
            this.Url = cta.Link?.Url;
            if (!string.IsNullOrEmpty(cta.Link?.Anchor))
            {
                this.Url = $"{this.Url}#{cta.Link.Anchor}";
            }

            this.Name = cta.Name;
            this.RenderAsLink = cta.RenderAsLink;
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Target { get; set; } = "_self";
        public string CssClass { get; set; }
        public string BackgroundImage { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool RenderAsLink { get; set; }
        public bool IsVideoPopup { get; set; }
    }

    #endregion 
}
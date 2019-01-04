using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace WageWorks.Feature.Teasers.Models
{
    public class RenderingWrapper : IRendering
    {
        private Rendering _rendering;
        public RenderingWrapper(Rendering rendering)
        {
            _rendering = rendering;
        }

        public IItem Item
        {
            get
            {
                if (_rendering.Item != null)
                {
                    return new ItemWrapper(_rendering.Item);
                }

                return null;
            }
        }
        public Rendering Rendering
        {
            get { return _rendering; }
        }
        public RenderingItem RenderingItem
        {
            get { return _rendering.RenderingItem; }
        }

        public RenderingParameters Parameters
        {
            get
            {
                return _rendering.Parameters;
            }
        }

    }
}
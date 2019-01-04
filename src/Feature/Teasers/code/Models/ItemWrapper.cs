using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public class ItemWrapper :IItem
    {
        
            public ItemWrapper(Item item)
            {
                Item = item;
            }

            public ItemWrapper()
            {
            }

            public Item Item { get; set; }

            public Database Database
            {
                get { return Item.Database; }
            }

            public string DisplayName
            {
                get
                {
                    return Item.DisplayName;
                }
            }

            public ID TemplateId
            {
                get
                {
                    return Item.TemplateID;
                }
            }

        }
    }
﻿using System.Web.Mvc;
using WageWorks.Foundation.Alerts.Models;

namespace WageWorks.Foundation.Alerts.Extensions
{
    public static class AlertControllerExtensions
  {
    public static ViewResult InfoMessage(this Controller controller, InfoMessage infoMessage)
    {
      if (infoMessage != null)
      {
        controller.ViewData.Model = infoMessage;
      }

      return new ViewResult
             {
               ViewName = Constants.InfoMessageView,
               ViewData = controller.ViewData,
               TempData = controller.TempData,
               ViewEngineCollection = controller.ViewEngineCollection
             };
    }
  }
}
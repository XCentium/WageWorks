using System.Web.Mvc;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using WageWorks.Foundation.Alerts.Exceptions;
using WageWorks.Foundation.Alerts.Models;

namespace WageWorks.Foundation.Alerts.Pipelines.MvcException
{
    public class InvalidDatasourceItemExceptionProcessor
  {
    public void Process(ExceptionArgs exceptionArgs)
    {
      if (exceptionArgs.ExceptionContext.ExceptionHandled)
      {
        return;
      }

      this.HandleException(exceptionArgs.ExceptionContext);
    }

    protected virtual void HandleException(ExceptionContext exceptionContext)
    {
      var dataSourceException = exceptionContext.Exception as InvalidDataSourceItemException;
      if (dataSourceException == null)
        return;
      Log.Error(dataSourceException.Message, dataSourceException, this);

      if (Context.PageMode.IsNormal)
      {
        exceptionContext.Result = new EmptyResult();
      }
      else
      {
        var viewData = new ViewDataDictionary
                       {
                         Model = InfoMessage.Error(AlertTexts.InvalidDataSource)
                       };
        exceptionContext.Result = new ViewResult
                                  {
                                    ViewName = Constants.InfoMessageView,
                                    ViewData = viewData
                                  };
      }

      exceptionContext.ExceptionHandled = true;
    }
  }
}
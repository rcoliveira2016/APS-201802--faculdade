﻿using APS.Domain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace APS.Presentation.Web.Controllers.Common
{
    public abstract class BaseController : Controller
    {
        private const string XMLHttpRequest = "XMLHttpRequest";
        private const string XRequestedWithHeadername = "X-Requested-With";

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ServiceException)
            {
                if (filterContext.HttpContext.Request.Headers[XRequestedWithHeadername] == XMLHttpRequest)
                {
                    //Return JSON
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { error = true, message = filterContext.Exception.Message }
                    };
                }
                else
                {
                    var result = new RedirectToRouteResult(new RouteValueDictionary(filterContext.RequestContext.RouteData.Values));
                    var obj =filterContext.HttpContext.Request.Form;
                    filterContext.Result = result;
                }
                filterContext.ExceptionHandled = true;

            }
        }

    }
}
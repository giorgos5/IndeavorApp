using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.Client.Controls
{
    public static class Helpers
    {
        //#region Menu
        //private const string DefaultCssClass = "active";
        //public static HtmlString ActiveTab(this HtmlHelper helper, string activeController, string[] activeActions, string cssClass)
        //{
        //    string currentAction = helper.ViewContext.RouteData.Values.try.GetValue("action").RawValue.ToString();
        //    string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();

        //    string cssClassToUse = currentController == activeController && activeActions.Contains(currentAction)
        //                               ? cssClass
        //                               : string.Empty;
        //    return new HtmlString("class=\"" + cssClassToUse + "\"");
        //}

        //public static HtmlString ActiveTab(this HtmlHelper helper, string activeController, string[] activeActions, string cssClass, string DefaultClass)
        //{
        //    string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
        //    string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();

        //    string cssClassToUse = currentController == activeController && activeActions.Contains(currentAction)
        //                               ? cssClass + " " + DefaultClass
        //                               : DefaultClass;
        //    return HtmlString.Create("class=\"" + cssClassToUse + "\"");
        //}

        //public static HtmlString ActiveTab(this HtmlHelper helper, string[] activeControllerActions, string cssClass, string DefaultClass)
        //{
        //    string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
        //    string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();

        //    string cssClassToUse = activeControllerActions.Contains(currentController + "_" + currentAction)
        //                               ? cssClass + " " + DefaultClass
        //                               : DefaultClass;
        //    return HtmlString.Create("class=\"" + cssClassToUse + "\"");
        //}

        //public static HtmlString ActiveTab(this HtmlHelper helper, string activeController, string[] activeActions)
        //{
        //    return helper.ActiveTab(activeController, activeActions, DefaultCssClass);
        //}

        //public static HtmlString ActiveTab(this HtmlHelper helper, string activeController, string activeAction)
        //{
        //    return helper.ActiveTab(activeController, new string[] { activeAction }, DefaultCssClass);
        //}

        //public static HtmlString ActiveTab(this HtmlHelper helper, string activeController, string activeAction, string cssClass)
        //{
        //    return helper.ActiveTab(activeController, new string[] { activeAction }, cssClass);
        //}

        //#endregion
    }
}

﻿using System.Web;
using System.Web.Optimization;

namespace Selama
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                RootJavaScriptFile("jQuery/jquery-{version}.js"),
                RootJavaScriptFile("jQuery/jquery.unobtrusive-ajax.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                RootJavaScriptFile("jQuery/jquery.validate.js"),
                RootJavaScriptFile("jQuery/jquery.validate.unobtrusive.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/MarkdownDeep").Include(
                RootJavaScriptFile("MarkdownDeep/MarkdownDeepLib.min.js")
            ));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                RootJavaScriptFile("require.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Core").Include(
                RootJavaScriptFile("Selama/Core/Common.js"),
                RootJavaScriptFile("Selama/Core/Alert.js"),
                RootJavaScriptFile("Selama/Core/SpinShield.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-RootArea").Include(
                RootJavaScriptFile("Selama/RootArea/Home/Index.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Forums").Include(
                RootJavaScriptFile("Forums/Common.js"),
                RootJavaScriptFile("Forums/Threads.js"),
                RootJavaScriptFile("Forums/Thread.js")
            ));
            bundles.Add(new ScriptBundle("~/bundles/Selama-Root").Include(
                RootJavaScriptFile("Root/Home/Index.js")
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                RootJavaScriptFile("modernizr-*")
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                RootJavaScriptFile("Bootstrap/bootstrap.js"),
                RootJavaScriptFile("Bootstrap/respond.js"))
            );
            bundles.Add(new ScriptBundle("~/bundles/Selama/Forums").Include(
                "~/Areas/Forums/Scripts/js/Forums.js"
            ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap/bootstrap.css",
                      "~/Content/Bootstrap/bootstrap-theme.css",
                      "~/Content/Bootstrap/bootstrap-social.css",
                      "~/Content/FontAwesome/font-awesome.css",
                      "~/Content/jquery.spin.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/MarkdownDeep/css").Include(
                "~/Content/MarkdownDeep/mdd_styles.css",
                "~/Content/MarkdownDeep/MarkdownDeep.css"
            ));
        }

        private static string RootJavaScriptFile(string nameInScriptsFolder)
        {
            return string.Format("~/Scripts/js/{0}", nameInScriptsFolder);
        }

        private static string AreaJavaScriptFile(string areaName, string nameInScriptsFolder)
        {
            return string.Format("~/{0}/Scripts/js/{1}", areaName, nameInScriptsFolder);
        }
    }
}

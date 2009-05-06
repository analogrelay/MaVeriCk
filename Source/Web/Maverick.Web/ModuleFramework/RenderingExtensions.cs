// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RenderModuleExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RenderModuleExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Maverick.Web.Helpers;
using Maverick.Web.Models;
using System;

namespace Maverick.Web.ModuleFramework {
    public static class RenderingExtensions {
        public static void RenderModule(this HtmlHelper helper, ModuleRequestResult moduleResult) {
            helper.RenderWithinCommentedBlock("Body",
                                              moduleResult.Module.Id,
                                              () => {
                                                  ModuleExecutionEngine.Current.ExecuteModuleResult(
                                                      helper.ViewContext.HttpContext.GetPortalContext(),
                                                      moduleResult);
                                              });
        }

        public static void RenderModuleHeader(this HtmlHelper helper, ModuleRequestResult moduleResult) {
            helper.RenderWithinCommentedBlock("Header",
                                              moduleResult.Module.Id,
                                              () => {
                                                  ModuleExecutionEngine.Current.ExecuteModuleHeader(
                                                      helper.ViewContext.HttpContext.GetPortalContext(),
                                                      moduleResult);
                                              });
        }

        public static void RenderModules(this HtmlHelper helper, IEnumerable<ModuleRequestResult> modules) {
            foreach (ModuleRequestResult moduleResult in modules) {
                helper.RenderModule(moduleResult);
            }
        }

        public static void RenderModuleHeaders(this HtmlHelper helper, IEnumerable<ModuleRequestResult> modules) {
            foreach(ModuleRequestResult moduleResult in modules) {
                helper.RenderModuleHeader(moduleResult);
            }
        }

        private static void RenderWithinCommentedBlock(this HtmlHelper helper, string blockName, int? moduleId, Action renderAction) {
            TextWriter output = helper.ViewContext.HttpContext.Response.Output;
            output.WriteLine();
            output.WriteLine("<!-- Start Module{0}{1} -->", moduleId.ToFormattedString("#{0}", String.Empty), blockName);
            renderAction();
            output.WriteLine();
            output.WriteLine("<!-- End Module{0}{1} -->", moduleId.ToFormattedString("#{0}", String.Empty), blockName);
        }

        private static string ToFormattedString<T>(this T? nullable, string formatString, string nullString) where T : struct {
            return nullable.HasValue ? String.Format(formatString, nullable.Value) : nullString;
        }
    }
}

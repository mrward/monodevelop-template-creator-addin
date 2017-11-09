//
// TemplatingServices.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2017 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating
{
	static class TemplatingServices
	{
		static readonly TemplatingEventsService eventsService = new TemplatingEventsService ();
		static readonly TemplatingOptions options = new TemplatingOptions ();
		static readonly TemplatingEngine templatingEngine = new TemplatingEngine ();
		static readonly TemplateJsonFileChangedMonitor templateJsonFileChangedMonitor =
			new TemplateJsonFileChangedMonitor ();

		static TemplatingServices ()
		{
			Initialize ();
		}

		static void Initialize ()
		{
			eventsService.TemplateFoldersChanged += TemplateFoldersChanged;
		}

		static void TemplateFoldersChanged (object sender, EventArgs e)
		{
			try {
				TemplatingOutputPad.Clear ();
				templatingEngine.ReloadTemplates ();
			} catch (Exception ex) {
				TemplatingServices.LogError ("Unable to reset template cache.", ex);
			}
		}

		public static TemplatingEventsService EventsService {
			get { return eventsService; }
		}

		public static TemplatingOptions Options {
			get { return options; }
		}

		public static TemplatingEngine TemplatingEngine {
			get { return templatingEngine; }
		}

		public static IEnumerable<TemplateCategory> GetProjectTemplateCategories ()
		{
			try {
				return GetProjectTemplateCategoriesByReflection ();
			} catch (Exception ex) {
				LogError ("Unable to get project template categories using reflection.", ex);
				return IdeApp.Services.TemplatingService.GetProjectTemplateCategories ();
			}
		}

		/// <summary>
		/// Use reflection to get the project template categories defined by the extension point.
		/// Using the MonoDevelop TemplatingService will only return categories that have project
		/// templates.
		/// </summary>
		static IEnumerable<TemplateCategory> GetProjectTemplateCategoriesByReflection ()
		{
			const string path = "/MonoDevelop/Ide/ProjectTemplateCategories";
			foreach (ExtensionNode node  in AddinManager.GetExtensionNodes (path)) {
				Type templateCategoryCodonType = node.GetType ();
				var flags = BindingFlags.Instance | BindingFlags.Public;
				var method = templateCategoryCodonType.GetMethod ("ToTopLevelTemplateCategory", flags);
				yield return (TemplateCategory)method.Invoke (node, new object [0]);
			}
		}

		public static void LogError (string message, Exception ex)
		{
			LoggingService.LogError (message, ex);
			TemplatingOutputPad.WriteError (message + " " + ex);
		}

		public static void LogInfo (string message)
		{
			TemplatingOutputPad.WriteText (message);
		}
	}
}

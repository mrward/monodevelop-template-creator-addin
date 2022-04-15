//
// CustomProjectTemplatingProvider.cs
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
using System.Linq;
using System.Threading.Tasks;
using MonoDevelop.Ide.Projects;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Projects;

namespace MonoDevelop.Templating
{
	class CustomProjectTemplatingProvider : ProjectTemplatingProvider
	{
		SolutionTemplate[] cachedTemplates;
		DateTime cacheExpiryDate;
		TimeSpan cacheExpiryTimeSpan = TimeSpan.FromSeconds (1);

		public override bool CanProcessTemplate (SolutionTemplate template)
		{
			return template is CustomSolutionTemplate;
		}

		public override Task<IEnumerable<SolutionTemplate>> GetTemplatesAsync ()
		{
			if (!UseCachedTemplates ()) {
				IEnumerable<SolutionTemplate> templates = LoadTemplates ();
				return Task.FromResult (templates);
			}

			return Task.FromResult (cachedTemplates.AsEnumerable ());
		}

		IEnumerable<SolutionTemplate> LoadTemplates ()
		{
			try {
				var templateEngine = TemplatingServices.TemplatingEngine;
				templateEngine.LoadTemplates ();

				return templateEngine.Templates;
			} catch (Exception ex) {
				TemplatingServices.LogError ("Unable to load templates.", ex);
				cachedTemplates = new SolutionTemplate [0];
				cacheExpiryDate = DateTime.UtcNow.Add (cacheExpiryTimeSpan);
			}
			return cachedTemplates;
		}

		public override Task<ProcessedTemplateResult> ProcessTemplateAsync (
			SolutionTemplate template,
			NewProjectConfiguration config,
			SolutionFolder parentFolder)
		{
			var templateProcessor = new TemplateProcessor (
				(CustomSolutionTemplate)template,
				config,
				parentFolder);

			return templateProcessor.ProcessTemplate ();
		}

		/// <summary>
		/// Use cached templates if they were used in the last second. This prevents the
		/// same template load exception occurring when the new project dialog is opened
		/// since it calls GetTemplates multiple times.
		/// </summary>
		bool UseCachedTemplates ()
		{
			if (cachedTemplates == null) {
				return false;
			}

			var currentDate = DateTime.UtcNow;
			if (currentDate > cacheExpiryDate) {
				cachedTemplates = null;
				return false;
			}

			return true;
		}
	}
}

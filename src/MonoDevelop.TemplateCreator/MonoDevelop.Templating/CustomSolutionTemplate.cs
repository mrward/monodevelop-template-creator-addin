//
// CustomSolutionTemplate.cs
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

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Settings;
using MonoDevelop.Ide.Templates;

namespace MonoDevelop.Templating
{
	class CustomSolutionTemplate : SolutionTemplate
	{
		public static readonly string MonoDevelopCategoryTagName = "md-category";
		public static readonly string VSMacCategoryTagName = "vsmac-category";

		static readonly string[] categoryTagNames = new string [] {
			MonoDevelopCategoryTagName,
			VSMacCategoryTagName
		};

		TemplateInfo info;

		public CustomSolutionTemplate (TemplateInfo info)
			: base (info.Identity, info.Name, null)
		{
			this.info = info;

			Category = GetCategory (info, "other/net/general");
			Description = info.Description;
			Language = info.GetLanguage ();
		}

		public TemplateInfo Info {
			get { return info; }
		}

		static string GetCategory (TemplateInfo info, string defaultCategory)
		{
			ICacheTag tag = null;
			foreach (string tagName in categoryTagNames) {
				if (info.Tags.TryGetValue (tagName, out tag)) {
					if (!string.IsNullOrEmpty (tag.DefaultValue)) {
						return tag.DefaultValue;
					}
				}
			}

			return defaultCategory;
		}
	}
}

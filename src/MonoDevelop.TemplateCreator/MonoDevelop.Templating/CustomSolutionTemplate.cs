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

using System;
using System.Collections.Generic;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Utils;
using MonoDevelop.Ide.Templates;

namespace MonoDevelop.Templating
{
	class CustomSolutionTemplate : SolutionTemplate
	{
		ITemplateInfo info;

		public CustomSolutionTemplate (ITemplateInfo info)
			: base (info.Identity, info.Name, null)
		{
			this.info = info;

			Category = info.GetCategory ("other/net/general");
			Description = info.Description;
			FileFormatExclude = info.GetFileFormatExclude ();
			GroupId = info.GroupIdentity;
			Language = info.GetLanguage ();
		}

		public ITemplateInfo Info {
			get { return info; }
		}

		public string FileFormatExclude { get; private set; }

		public bool CanFormatFile (string fileName)
		{
			if (string.IsNullOrEmpty (FileFormatExclude))
				return true;

			if (excludedFileEndings == null) {
				excludedFileEndings = GetExcludedFileEndings (FileFormatExclude);
			}

			foreach (string ending in excludedFileEndings) {
				if (fileName.EndsWith (ending, StringComparison.OrdinalIgnoreCase)) {
					return false;
				}
			}

			return true;
		}

		List<string> excludedFileEndings;

		static List<string> GetExcludedFileEndings (string exclude)
		{
			var result = new List<string> ();
			foreach (string pattern in exclude.Split (';')) {
				string trimmedPattern = pattern.Trim ();
				if (string.IsNullOrEmpty (trimmedPattern)) {
					// ignore
				} else if (trimmedPattern.StartsWith ("*.", StringComparison.Ordinal)) {
					result.Add (trimmedPattern.Substring (1));
				} else {
					result.Add (trimmedPattern);
				}
			}
			return result;
		}
	}
}

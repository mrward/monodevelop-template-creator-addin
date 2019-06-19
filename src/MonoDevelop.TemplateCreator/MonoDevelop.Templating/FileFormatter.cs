//
// FileFormatter.cs
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
using System.IO;
using System.Text;
using MonoDevelop.Core;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeFormatting;
using MonoDevelop.Projects.Policies;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating
{
	static class FileFormatter
	{
		public static void FormatFile (PolicyContainer policies, FilePath fileName)
		{
			CodeFormatter formatter = GetFormatter (fileName);
			if (formatter == null)
				return;

			try {
				string content = File.ReadAllText (fileName);
				string formatted = formatter.FormatText (policies, content);
				if (formatted != null) {
					TextFileUtility.WriteText (fileName, formatted, Encoding.UTF8);
				}
			} catch (Exception ex) {
				TemplatingServices.LogError ("File formatting failed", ex);
			}
		}

		static CodeFormatter GetFormatter (FilePath fileName)
		{
			string mime = IdeServices.DesktopService.GetMimeTypeForUri (fileName);
			if (mime != null) {
				return CodeFormatterService.GetFormatter (mime);
			}

			return null;
		}
	}
}

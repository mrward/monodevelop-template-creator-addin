﻿//
// FileFormattingExcludeTagProvider.cs
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

namespace MonoDevelop.Templating
{
	static class FileFormattingExcludeTagProvider
	{
		public static readonly string MonoDevelopFileFormatExcludeTagName = "md-file-format-exclude";
		public static readonly string VSMacFileFormatExcludeTagName = "vsmac-file-format-exclude";

		public static readonly string[] TagNames = new string [] {
			MonoDevelopFileFormatExcludeTagName,
			VSMacFileFormatExcludeTagName
		};

		public static readonly string DefaultTagName;

		static FileFormattingExcludeTagProvider ()
		{
			DefaultTagName = GetDefaultTagName ();
		}

		static string GetDefaultTagName ()
		{
			if (TemplatingServices.IsVisualStudio ()) {
				return VSMacFileFormatExcludeTagName;
			}

			return MonoDevelopFileFormatExcludeTagName;
		}
	}
}

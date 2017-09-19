//
// TemplateCategoryTagNameProvider.cs
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
using MonoDevelop.Core;

namespace MonoDevelop.Templating
{
	class TemplateCategoryTagNameProvider
	{
		public static readonly string MonoDevelopCategoryTagName = "md-category";
		public static readonly string VSMacCategoryTagName = "vsmac-category";

		public static readonly string[] CategoryTagNames = new string [] {
			MonoDevelopCategoryTagName,
			VSMacCategoryTagName
		};

		public static readonly string DefaultCategoryTagName;

		static TemplateCategoryTagNameProvider ()
		{
			DefaultCategoryTagName = GetDefaultCategoryTagName ();
		}

		static string GetDefaultCategoryTagName ()
		{
			if (BrandingService.ApplicationName != null) {
				if (BrandingService.ApplicationName.StartsWith ("Visual Studio", StringComparison.OrdinalIgnoreCase)) {
					return VSMacCategoryTagName;
				}
			}

			return MonoDevelopCategoryTagName;
		}
	}
}

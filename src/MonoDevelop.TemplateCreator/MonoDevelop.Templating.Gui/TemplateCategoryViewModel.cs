//
// TemplateCategoryViewModel.cs
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

using MonoDevelop.Ide.Templates;

namespace MonoDevelop.Templating.Gui
{
	class TemplateCategoryViewModel
	{
		public TemplateCategoryViewModel (
			TemplateCategoryViewModel parentCategory,
			TemplateCategory category)
		{
			ParentCategory = parentCategory;
			Category = category;

			IsBottomLevel = GetIsBottomLevel ();
		}

		public TemplateCategoryViewModel ParentCategory { get; private set; }
		public TemplateCategory Category { get; private set; }
		public bool IsBottomLevel { get; private set; }

		public string GetCategoryIdPath ()
		{
			if (ParentCategory != null) {
				return ParentCategory.GetCategoryIdPath () + "/" + Category.Id;
			}

			return Category.Id;
		}

		bool GetIsBottomLevel ()
		{
			int parentCount = 0;

			TemplateCategoryViewModel currentParent = ParentCategory;
			while (currentParent != null) {
				parentCount++;
				currentParent = currentParent.ParentCategory;
			}

			return parentCount == 2;
		}
	}
}

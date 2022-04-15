//
// TemplateCategoriesDialog.cs
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
using System.Threading.Tasks;
using MonoDevelop.Ide;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateCategoriesDialog
	{
		string selectedCategoryId;

		public TemplateCategoriesDialog ()
		{
			Build ();

			AddTemplateCategoriesAsync ().Ignore ();
		}

		async Task AddTemplateCategoriesAsync ()
		{
			var categories = await TemplatingServices.GetProjectTemplateCategories ();

			templateCategoriesWidget.AddTemplateCategories (categories);

			templateCategoriesWidget.SelectedCategoryChanged += SelectedCategoryChanged;
		}

		public bool ShowWithParent ()
		{
			return Run (MessageDialog.RootWindow) == Command.Ok;
		}

		void SelectedCategoryChanged (object sender, EventArgs e)
		{
			var category = templateCategoriesWidget.SelectedCategory;
			if (category?.IsBottomLevel == true) {
				SelectedCategoryId = category?.GetCategoryIdPath ();
			} else {
				SelectedCategoryId = null;
			}
		}

		public string SelectedCategoryId {
			get { return selectedCategoryId; }
			private set {
				selectedCategoryId = value;

				okButton.Sensitive = !string.IsNullOrEmpty (selectedCategoryId);
			}
		}
	}
}

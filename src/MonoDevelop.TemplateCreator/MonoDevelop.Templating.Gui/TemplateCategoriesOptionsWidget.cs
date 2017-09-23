//
// TemplateCategoriesOptionsWidget.cs
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
using MonoDevelop.Ide.Templates;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateCategoriesOptionsWidget
	{
		TemplateCategoriesOptionsViewModel viewModel;

		public TemplateCategoriesOptionsWidget (TemplateCategoriesOptionsViewModel viewModel)
		{
			this.viewModel = viewModel;

			Build ();

			AddCategories ();

			addTopLevelCategoryButton.Clicked += AddTopLevelCategoryButtonClicked;
			addCategoryButton.Clicked += AddCategoryButtonClicked;
			removeCategoryButton.Clicked += RemoveCategoryButtonClicked;

			templateCategoriesWidget.SelectedCategoryChanged += SelectedCategoryChanged;

			topLevelCategoryWidget.NameChanged += TemplateCategoryNameChanged;
			secondLevelCategoryWidget.NameChanged += TemplateCategoryNameChanged;
			thirdLevelCategoryWidget.NameChanged += TemplateCategoryNameChanged;
		}

		void AddCategories ()
		{
			templateCategoriesWidget.AddTemplateCategories (viewModel.GetCategories ());
		}

		void AddTopLevelCategoryButtonClicked (object sender, EventArgs e)
		{
			TemplateCategoryViewModel category = viewModel.AddTopLevelCategory ();
			templateCategoriesWidget.AddTopLevelCategory (category);

			topLevelCategoryWidget.SetFocusToIdTextEntry ();
		}

		void SelectedCategoryChanged (object sender, EventArgs e)
		{
			TemplateCategoryViewModel category = templateCategoriesWidget.SelectedCategory;
			viewModel.SelectedCategory = category;

			categoryInformationVBox.Sensitive = viewModel.IsCategoryInformationEnabled;
			addCategoryButton.Sensitive = viewModel.IsAddCategoryButtonEnabled;
			removeCategoryButton.Sensitive = viewModel.IsRemoveCategoryButtonEnabled;

			ShowCategoryInfo (topLevelCategoryWidget, viewModel.TopLevelCategory);
			ShowCategoryInfo (secondLevelCategoryWidget, viewModel.SecondLevelCategory);
			ShowCategoryInfo (thirdLevelCategoryWidget, viewModel.ThirdLevelCategory);
		}

		static void ShowCategoryInfo (TemplateCategoryWidget widget, TemplateCategoryViewModel category)
		{
			if (category != null) {
				widget.DisplayCategory (category);
			} else {
				widget.ClearCategory ();
			}
		}

		void TemplateCategoryNameChanged (object sender, EventArgs e)
		{
			var widget = (TemplateCategoryWidget)sender;
			templateCategoriesWidget.UpdateTemplateCategoryName (widget.Category);
		}

		void AddCategoryButtonClicked (object sender, EventArgs e)
		{
			TemplateCategoryViewModel category = viewModel.AddCategory ();
			templateCategoriesWidget.AddChildTemplateCategory (viewModel.SelectedCategory, category);
		}

		void RemoveCategoryButtonClicked (object sender, EventArgs e)
		{
			viewModel.RemoveSelectedCategory ();
			templateCategoriesWidget.RemoveTemplateCategory (viewModel.SelectedCategory);
		}
	}
}

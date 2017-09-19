//
// TemplateCategoriesWidget.cs
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
using System.Linq;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Templates;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateCategoriesWidget
	{
		TemplateCategoryViewModel selectedCategory;

		public TemplateCategoriesWidget ()
		{
			Build ();

			AddTemplateCategories ();

			treeView.ExpandAll ();

			treeView.SelectionChanged += TreeViewSelectionChanged;
		}

		public TemplateCategoryViewModel SelectedCategory {
			get { return selectedCategory; }
			set {
				if (selectedCategory != value) {
					selectedCategory = value;
					SelectedCategoryChanged?.Invoke (this, new EventArgs ());
				}
			}
		}
		public event EventHandler SelectedCategoryChanged;

		void AddTemplateCategories ()
		{
			foreach (TemplateCategory category in IdeApp.Services.TemplatingService.GetProjectTemplateCategories ()) {
				AddTemplateCategory (null, null, category);
			}
		}

		TreePosition AddTemplateCategory (
			TreePosition position,
			TemplateCategoryViewModel parentViewModel,
			TemplateCategory category)
		{
			var categoryViewModel = new TemplateCategoryViewModel (parentViewModel, category);

			TreeNavigator node = treeStore.AddNode (position);
			node.SetValue (nameColumn, category.Name);
			node.SetValue (categoryColumn, categoryViewModel);

			foreach (TemplateCategory childCategory in category.Categories) {
				AddTemplateCategory (node.CurrentPosition, categoryViewModel, childCategory);
			}

			return position;
		}

		void TreeViewSelectionChanged (object sender, EventArgs e)
		{
			if (treeView.SelectedRows.Any ()) {
				var navigator = treeStore.GetNavigatorAt (treeView.SelectedRows [0]);
				if (navigator != null) {
					SelectedCategory = navigator.GetValue (categoryColumn);
					return;
				}
			}

			SelectedCategory = null;
		}
	}
}

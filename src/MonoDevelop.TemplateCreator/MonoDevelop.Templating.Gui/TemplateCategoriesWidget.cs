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
using System.Collections.Generic;
using System.Linq;
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

		public void AddTemplateCategories (IEnumerable<TemplateCategory> categories)
		{
			AddTemplateCategories (GetCategoryViewModels (categories));
		}

		IEnumerable<TemplateCategoryViewModel> GetCategoryViewModels (
			IEnumerable<TemplateCategory> categories)
		{
			return categories.Select (category => new TemplateCategoryViewModel (null, category));
		}

		public void AddTemplateCategories (IEnumerable<TemplateCategoryViewModel> categories)
		{
			foreach (TemplateCategoryViewModel category in categories) {
				AddTemplateCategory (null, category);
			}

			treeView.ExpandAll ();
		}

		TreePosition AddTemplateCategory (
			TreePosition position,
			TemplateCategoryViewModel categoryViewModel)
		{
			TreeNavigator node = treeStore.AddNode (position);
			node.SetValue (nameColumn, categoryViewModel.Name);
			node.SetValue (categoryColumn, categoryViewModel);

			foreach (TemplateCategoryViewModel childCategory in categoryViewModel.GetChildCategories ()) {
				AddTemplateCategory (node.CurrentPosition, childCategory);
			}

			return node.CurrentPosition;
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

		public void AddTopLevelCategory (TemplateCategoryViewModel category)
		{
			TreePosition position = AddTemplateCategory (null, category);
			treeView.ExpandRow (position, true);

			// Ensure last child is visible and selected.
			TreeNavigator navigator = treeStore.GetNavigatorAt (position);
			navigator.MoveToChild ();
			navigator.MoveToChild ();
			treeView.ScrollToRow (navigator.CurrentPosition);
			treeView.SelectRow (navigator.CurrentPosition);
		}

		public void UpdateTemplateCategoryName (TemplateCategoryViewModel category)
		{
			TreeNavigator navigator = FindCategoryNavigator (category);

			if (navigator != null) {
				navigator.SetValue (nameColumn, category.Name);
			}
		}

		TreeNavigator FindCategoryNavigator (TemplateCategoryViewModel category)
		{
			return treeStore.FindNavigators (category, categoryColumn)
				.FirstOrDefault ();
		}

		public void RemoveTemplateCategory (TemplateCategoryViewModel category)
		{
			TreeNavigator navigator = FindCategoryNavigator (category);
			if (navigator != null) {
				navigator.Remove ();
			}
		}

		public void AddChildTemplateCategory (
			TemplateCategoryViewModel parentCategory,
			TemplateCategoryViewModel category)
		{
			TreeNavigator navigator = FindCategoryNavigator (parentCategory);
			if (navigator != null) {
				AddTemplateCategory (navigator.CurrentPosition, category);
				treeView.ExpandRow (navigator.CurrentPosition, true);
			}
		}
	}
}

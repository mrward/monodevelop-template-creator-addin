//
// TemplateCategoriesOptionsViewModel.cs
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

namespace MonoDevelop.Templating.Gui
{
	class TemplateCategoriesOptionsViewModel
	{
		List<TemplateCategoryViewModel> categories = new List<TemplateCategoryViewModel> ();
		TemplateCategoryViewModel selectedCategory;

		public void Load ()
		{
			categories = TemplateCreatorAddinXmlFile.ReadTemplateCategories ().ToList ();
		}

		public void Save ()
		{
			TemplateCreatorAddinXmlFile.UpdateTemplateCategories (categories);
		}

		public bool ValidateChanges ()
		{
			return true;
		}

		public TemplateCategoryViewModel AddTopLevelCategory ()
		{
			var topLevelCategory = CreateUniqueCategory ("top", "Top Level", categories);
			var secondLevelCategory = new TemplateCategory ("second", "Second Level", null);
			var thirdLevelCategory = new TemplateCategory ("third", "Third Level", null);

			topLevelCategory.AddCategory (secondLevelCategory);
			secondLevelCategory.AddCategory (thirdLevelCategory);

			var viewModel = new TemplateCategoryViewModel (null, topLevelCategory);

			categories.Add (viewModel);

			return viewModel;
		}

		static TemplateCategory CreateUniqueCategory (string id, string name, IEnumerable<TemplateCategoryViewModel> categories)
		{
			int? count = GetUniqueTemplateIdCount (id, categories);
			if (count.HasValue) {
				id = id + count.ToString ();
				name = name + count.ToString ();
			}

			return new TemplateCategory (id, name, null);
		}

		static int? GetUniqueTemplateIdCount (string templateId, IEnumerable<TemplateCategoryViewModel> categories)
		{
			var existingCategory = categories.FirstOrDefault (category => category.Id == templateId);
			if (existingCategory == null) {
				return null;
			}

			int count = 1;
			do {
				count++;
				string newTemplateId = templateId + count.ToString ();
				existingCategory = categories.FirstOrDefault (category => category.Id == newTemplateId);
			} while (existingCategory != null);

			return count;
		}

		public IEnumerable<TemplateCategoryViewModel> GetCategories ()
		{
			return categories;
		}

		public TemplateCategoryViewModel AddCategory ()
		{
			var category = CreateUniqueCategory (
				"categoryid",
				"New Category",
				selectedCategory.GetChildCategories ());

			return selectedCategory.AddCategory (category);
		}

		public void RemoveSelectedCategory ()
		{
			if (selectedCategory.IsTopLevel) {
				categories.Remove (selectedCategory);
			} else {
				selectedCategory.ParentCategory.RemoveCategory (selectedCategory);
			}
		}

		public TemplateCategoryViewModel SelectedCategory {
			get { return selectedCategory; }
			set {
				selectedCategory = value;
				if (selectedCategory != null) {
					ShowCategoryInfo ();
				} else {
					ClearCategoryInfo ();
				}
			}
		}

		void ShowCategoryInfo ()
		{
			IsCategoryInformationEnabled = true;
			IsRemoveCategoryButtonEnabled = true;
			IsAddCategoryButtonEnabled = !selectedCategory.IsBottomLevel;

			if (selectedCategory.IsTopLevel) {
				TopLevelCategory = selectedCategory;
				SecondLevelCategory = null;
				ThirdLevelCategory = null;
			} else if (selectedCategory.IsBottomLevel) {
				TopLevelCategory = selectedCategory.ParentCategory.ParentCategory;
				SecondLevelCategory = selectedCategory.ParentCategory;
				ThirdLevelCategory = selectedCategory;
			} else {
				TopLevelCategory = selectedCategory.ParentCategory;
				SecondLevelCategory = selectedCategory;
				ThirdLevelCategory = null;
			}
		}

		void ClearCategoryInfo ()
		{
			IsCategoryInformationEnabled = false;
			IsAddCategoryButtonEnabled = false;
			IsRemoveCategoryButtonEnabled = false;

			TopLevelCategory = null;
			SecondLevelCategory = null;
			ThirdLevelCategory = null;
		}

		public TemplateCategoryViewModel TopLevelCategory { get; private set; }
		public TemplateCategoryViewModel SecondLevelCategory { get; private set; }
		public TemplateCategoryViewModel ThirdLevelCategory { get; private set; }

		public bool IsCategoryInformationEnabled { get; private set; }
		public bool IsAddCategoryButtonEnabled { get; private set; }
		public bool IsRemoveCategoryButtonEnabled { get; private set; }
	}
}

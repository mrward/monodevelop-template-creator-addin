﻿//
// TemplateInformationDialog.cs
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
using MonoDevelop.Ide;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateInformationDialog
	{
		TemplateInformation viewModel;

		public TemplateInformationDialog (TemplateInformation viewModel)
		{
			this.viewModel = viewModel;

			Build ();

			authorTextEntry.TextEntry.Text = viewModel.Author;
			defaultProjectNameTextEntry.TextEntry.Text = viewModel.DefaultProjectName;
			displayNameTextEntry.TextEntry.Text = viewModel.DisplayName;
			groupIdentityTextEntry.TextEntry.Text = viewModel.GroupIdentity;
			identityTextEntry.TextEntry.Text = viewModel.Identity;
			shortNameTextEntry.TextEntry.Text = viewModel.ShortName;
			categoryTextEntry.TextEntry.Text = viewModel.Category;
			descriptionTextEntry.TextEntry.Text = viewModel.Description;
			fileFormatExcludeTextEntry.TextEntry.Text = viewModel.FileFormatExclude;

			authorTextEntry.TextEntry.Changed += AuthorTextEntryChanged;
			defaultProjectNameTextEntry.TextEntry.Changed += DefaultProjectNameTextEntryChanged;
			displayNameTextEntry.TextEntry.Changed += DisplayNameTextEntryChanged;
			groupIdentityTextEntry.TextEntry.Changed += GroupIdentityTextEntryChanged;
			identityTextEntry.TextEntry.Changed += IdentityTextEntryChanged;
			shortNameTextEntry.TextEntry.Changed += ShortNameTextEntryChanged;
			categoryTextEntry.TextEntry.Changed += CategoryTextEntryChanged;
			descriptionTextEntry.TextEntry.Changed += DescriptionTextChanged;
			fileFormatExcludeTextEntry.TextEntry.Changed += FileFormatExcludeTextChanged ;

			selectCategoryButton.Clicked += SelectCategoryButtonClicked;
		}

		public bool ShowWithParent ()
		{
			return Run (MessageDialog.RootWindow) == Command.Ok;
		}

		void AuthorTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.Author = authorTextEntry.TextEntry.Text;
		}

		void DefaultProjectNameTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.DefaultProjectName = defaultProjectNameTextEntry.TextEntry.Text;
		}

		void DisplayNameTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.DisplayName = displayNameTextEntry.TextEntry.Text;
		}

		void GroupIdentityTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.GroupIdentity = groupIdentityTextEntry.TextEntry.Text;
		}

		void IdentityTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.Identity = identityTextEntry.TextEntry.Text;
		}

		void ShortNameTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.ShortName = shortNameTextEntry.TextEntry.Text;
		}

		void CategoryTextEntryChanged (object sender, EventArgs e)
		{
			viewModel.Category = categoryTextEntry.TextEntry.Text;
		}

		void DescriptionTextChanged (object sender, EventArgs e)
		{
			viewModel.Description = descriptionTextEntry.TextEntry.Text;
		}

		void FileFormatExcludeTextChanged (object sender, EventArgs e)
		{
			viewModel.FileFormatExclude = fileFormatExcludeTextEntry.TextEntry.Text;
		}

		void SelectCategoryButtonClicked (object sender, EventArgs e)
		{
			using (var dialog = new TemplateCategoriesDialog ()) {
				if (dialog.ShowWithParent ()) {
					categoryTextEntry.TextEntry.Text = dialog.SelectedCategoryId;
				}
			}
		}
	}
}

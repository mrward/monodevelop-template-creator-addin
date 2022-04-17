//
// TemplateFoldersOptionsWidget.cs
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
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateFoldersOptionsWidget
	{
		TemplateFoldersOptionsViewModel viewModel;
		ListStore foldersListStore;
		DataField<string> folderDataField = new DataField<string> ();

		public TemplateFoldersOptionsWidget (TemplateFoldersOptionsViewModel viewModel)
		{
			this.viewModel = viewModel;

			Build ();

			CreateListViewColumns ();
			UpdateList ();

			SubscribeEvents ();

			UpdateStatus ();
		}

		void CreateListViewColumns ()
		{
			var column = new ListViewColumn ("Folder");
			foldersListStore = new ListStore (folderDataField);
			foldersListView.DataSource = foldersListStore;

			var cellView = new TextCellView (folderDataField);
			column.Views.Add (cellView, true);

			foldersListView.Columns.Add (column);
		}

		void UpdateList ()
		{
			foldersListStore.Clear ();
			foreach (string folder in viewModel.TemplateFolders)
			{
				int row = foldersListStore.AddRow ();
				foldersListStore.SetValue (row, folderDataField, folder);
			}
		}

		void SubscribeEvents ()
		{
			folderTextEntry.Changed += FolderTextEntryChanged;
			browseButton.Clicked += BrowseButtonClicked;

			foldersListView.SelectionChanged += FoldersListViewSelectionChanged;

			addButton.Clicked += AddButtonClicked;
			removeButton.Clicked += RemoveButtonClicked;
			updateButton.Clicked += UpdateButtonClicked;
			upButton.Clicked += UpButtonClicked;
			downButton.Clicked += DownButtonClicked;
		}

		void FoldersListViewSelectionChanged (object sender, EventArgs e)
		{
			UpdateStatus ();
		}

		void FolderTextEntryChanged (object sender, EventArgs e)
		{
			UpdateStatus ();
		}

		void BrowseButtonClicked (object sender, EventArgs e)
		{
			using var dialog = new SelectFolderDialog ();

			if (dialog.Run ()) {
				string folder = dialog.Folder;
				if (!string.IsNullOrEmpty (folder)) {
					folderTextEntry.Text = folder;
					FolderTextEntryChanged (folderTextEntry, EventArgs.Empty);
				}
			}
		}

		void UpdateStatus ()
		{
			addButton.Sensitive = !string.IsNullOrEmpty (folderTextEntry.Text);

			int selectedRow = foldersListView.SelectedRow;
			if (selectedRow >= 0) {
				updateButton.Sensitive = !string.IsNullOrEmpty (folderTextEntry.Text);
				removeButton.Sensitive = true;
				upButton.Sensitive = (selectedRow > 0) &&
					(foldersListStore.RowCount > 1);

				downButton.Sensitive = (selectedRow + 1 < foldersListStore.RowCount) &&
					(foldersListStore.RowCount > 1);
			} else {
				updateButton.Sensitive = false;
				removeButton.Sensitive = false;
				upButton.Sensitive = false;
				downButton.Sensitive = false;
			}
		}

		void AddButtonClicked (object sender, EventArgs e)
		{
			string path = folderTextEntry.Text;
			viewModel.TemplateFolders.Add (path);

			int row = foldersListStore.AddRow ();
			foldersListStore.SetValue (row, folderDataField, path);

			FocusRow (row);
		}

		void FocusRow (int row)
		{
			foldersListView.SelectRow (row);
			foldersListView.ScrollToRow (row);

			UpdateStatus ();
		}

		void RemoveButtonClicked (object sender, EventArgs e)
		{
			int row = foldersListView.SelectedRow;
			if (row >= 0) {
				viewModel.TemplateFolders.RemoveAt (row);
				foldersListStore.RemoveRow (row);

				row = foldersListView.SelectedRow;
				if (row >= 0) {
					FocusRow (row);
				} else {
					UpdateStatus ();
				}
			}
		}

		void UpdateButtonClicked (object sender, EventArgs e)
		{
			int row = foldersListView.SelectedRow;
			if (row >= 0) {
				string folder = foldersListStore.GetValue (row, folderDataField);
				int index = viewModel.TemplateFolders.IndexOf (folder);
				viewModel.TemplateFolders [index] = folderTextEntry.Text;

				foldersListStore.SetValue (row, folderDataField, folderTextEntry.Text);

				UpdateStatus ();
			}
		}

		void UpButtonClicked (object sender, EventArgs e)
		{
			int row = foldersListView.SelectedRow;
			if (row > 0) {
				string folder = viewModel.TemplateFolders[row];
				string previousFolder = viewModel.TemplateFolders[row - 1];

				foldersListStore.SetValue (row, folderDataField, previousFolder);
				foldersListStore.SetValue (row - 1, folderDataField, folder);

				viewModel.TemplateFolders[row] = previousFolder;
				viewModel.TemplateFolders[row - 1] = folder;

				FocusRow (row - 1);
			}
		}

		void DownButtonClicked (object sender, EventArgs e)
		{
			int row = foldersListView.SelectedRow;
			if ((row >= 0) && (row < foldersListStore.RowCount)) {
				string folder = viewModel.TemplateFolders[row];

				string nextFolder = viewModel.TemplateFolders[row + 1];

				foldersListStore.SetValue (row, folderDataField, nextFolder);
				foldersListStore.SetValue (row + 1, folderDataField, folder);

				viewModel.TemplateFolders[row] = nextFolder;
				viewModel.TemplateFolders[row + 1] = folder;

				FocusRow (row + 1);
			}
		}
	}
}

//
// TemplateFoldersOptionsWidget.UI.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2017 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the S"oftware)", to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED A"S IS," WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateFoldersOptionsWidget : Widget
	{
		//FolderListSelector folderSelector;
		TextEntry folderTextEntry;
		Button browseButton;
		Button addButton;
		Button removeButton;
		Button updateButton;
		Button upButton;
		Button downButton;
		ListView foldersListView;

		void Build ()
		{
			var mainVBox = new VBox ();
			mainVBox.Spacing = 6;

			Content = mainVBox;

			var topLabel = new Label ();
			topLabel.Text = GettextCatalog.GetString (
				"Custom folders where {0} will look for project templates defined with a '.template.config/template.json' file.",
				BrandingService.ApplicationName);
			topLabel.Wrap = WrapMode.Word;
			mainVBox.PackStart (topLabel);

			var hbox = new HBox ();
			mainVBox.PackStart (hbox, true, true);

			var leftHandVBox = new VBox ();
			hbox.PackStart (leftHandVBox, true, true);

			var folderHBox = new HBox ();
			folderHBox.MarginTop = 6;
			folderTextEntry = new TextEntry ();
			folderHBox.PackStart (folderTextEntry, true, true);

			browseButton = new Button ();
			browseButton.Label = "\u2026";
			browseButton.HeightRequest = 10;

			folderHBox.PackStart (browseButton);
			leftHandVBox.PackStart (folderHBox);

			foldersListView = new ListView ();
			foldersListView.HeadersVisible = false;
			leftHandVBox.PackStart (foldersListView, true, true);

			var rightHandVBox = new VBox ();
			hbox.PackStart (rightHandVBox, false, false);

			addButton = new Button ();
			addButton.Label = GettextCatalog.GetString ("Add");
			rightHandVBox.PackStart (addButton);

			removeButton = new Button ();
			removeButton.Label = GettextCatalog.GetString ("Remove");
			rightHandVBox.PackStart (removeButton);

			updateButton = new Button ();
			updateButton.Label = GettextCatalog.GetString ("Update");
			rightHandVBox.PackStart (updateButton);

			upButton = new Button ();
			upButton.Label = GettextCatalog.GetString ("Up");
			rightHandVBox.PackStart (upButton);

			downButton = new Button ();
			downButton.Label = GettextCatalog.GetString ("Down");
			rightHandVBox.PackStart (downButton);
		}
	}
}
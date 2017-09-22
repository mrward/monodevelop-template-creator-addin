//
// TemplateCategoriesOptionsWidget.UI.cs
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
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateCategoriesOptionsWidget : Widget
	{
		TemplateCategoriesWidget templateCategoriesWidget;
		Button addTopLevelCategoryButton;
		VBox categoryInformationVBox;
		TemplateCategoryHBox topLevelCategoryHBox;
		TemplateCategoryHBox secondLevelCategoryHBox;
		TemplateCategoryHBox thirdLevelCategoryHBox;

		void Build ()
		{
			var mainVBox = new VBox ();

			var mainHBox = new HBox ();
			mainVBox.PackStart (mainHBox, true, true);

			templateCategoriesWidget = new TemplateCategoriesWidget ();
			mainHBox.PackStart (templateCategoriesWidget, true, true);

			var buttonsHBox = new HBox ();
			mainHBox.PackStart (buttonsHBox, false, false);

			addTopLevelCategoryButton = new Button ();
			addTopLevelCategoryButton.Label = GettextCatalog.GetString ("Add Top Level Category");
			buttonsHBox.PackStart (addTopLevelCategoryButton, false, false);
			addTopLevelCategoryButton.VerticalPlacement = WidgetPlacement.Start;
			addTopLevelCategoryButton.ExpandVertical = false;

			categoryInformationVBox = new VBox ();
			categoryInformationVBox.Sensitive = false;
			mainVBox.PackStart (categoryInformationVBox, false, false);

			string title = GettextCatalog.GetString ("Top Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			topLevelCategoryHBox = CreateCategoryHBox ();
			categoryInformationVBox.PackStart (topLevelCategoryHBox.HBox);

			title = GettextCatalog.GetString ("Second Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			secondLevelCategoryHBox = CreateCategoryHBox ();
			categoryInformationVBox.PackStart (secondLevelCategoryHBox.HBox);

			title = GettextCatalog.GetString ("Third Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			thirdLevelCategoryHBox = CreateCategoryHBox ();
			categoryInformationVBox.PackStart (thirdLevelCategoryHBox.HBox);

			Content = mainVBox;
		}

		static void AddCategorySectionTitleLabel (VBox vbox, string title)
		{
			var titleLabel = new Label ();
			titleLabel.Markup = "<b>" + title + "</b>";
			titleLabel.TextAlignment = Alignment.Start;

			vbox.PackStart (titleLabel);
		}

		TemplateCategoryHBox CreateCategoryHBox ()
		{
			var hbox = new HBox ();
			hbox.Margin = 10;

			var idLabel = new Label ();
			idLabel.Text = GettextCatalog.GetString ("Id:");
			hbox.PackStart (idLabel);

			var idTextEntry = new TextEntry ();
			hbox.PackStart (idTextEntry);

			var nameLabel = new Label ();
			nameLabel.Text = GettextCatalog.GetString ("Name:");
			hbox.PackStart (nameLabel);

			var nameTextEntry = new TextEntry ();
			hbox.PackStart (nameTextEntry);

			return new TemplateCategoryHBox {
				HBox = hbox,
				IdLabel = idLabel,
				IdTextEntry = idTextEntry,
				NameLabel = nameLabel,
				NameTextEntry = nameTextEntry
			};
		}

		struct TemplateCategoryHBox {
			public HBox HBox;
			public Label IdLabel;
			public TextEntry IdTextEntry;
			public Label NameLabel;
			public TextEntry NameTextEntry;
		}
	}
}

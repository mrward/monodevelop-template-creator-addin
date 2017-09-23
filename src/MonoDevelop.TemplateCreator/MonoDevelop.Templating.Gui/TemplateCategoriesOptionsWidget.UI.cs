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

using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateCategoriesOptionsWidget : Widget
	{
		TemplateCategoriesWidget templateCategoriesWidget;
		Button addTopLevelCategoryButton;
		Button addCategoryButton;
		Button removeCategoryButton;
		VBox categoryInformationVBox;
		TemplateCategoryWidget topLevelCategoryWidget;
		TemplateCategoryWidget secondLevelCategoryWidget;
		TemplateCategoryWidget thirdLevelCategoryWidget;

		void Build ()
		{
			var mainVBox = new VBox ();

			var topLabel = new Label ();
			topLabel.Text = GettextCatalog.GetString ("Category changes will take effect the next time you start {0}.", BrandingService.ApplicationName);
			topLabel.TextAlignment = Alignment.Start;
			topLabel.Margin = 5;
			mainVBox.PackStart (topLabel);

			var mainHBox = new HBox ();
			mainVBox.PackStart (mainHBox, true, true);

			templateCategoriesWidget = new TemplateCategoriesWidget ();
			mainHBox.PackStart (templateCategoriesWidget, true, true);

			var buttonsVBox = new VBox ();
			mainHBox.PackStart (buttonsVBox, false, false);

			addTopLevelCategoryButton = new Button ();
			addTopLevelCategoryButton.Label = GettextCatalog.GetString ("Add Top Level Category");
			buttonsVBox.PackStart (addTopLevelCategoryButton, false, false);

			addCategoryButton = new Button ();
			addCategoryButton.Label = GettextCatalog.GetString ("Add Category");
			addCategoryButton.Sensitive = false;
			buttonsVBox.PackStart (addCategoryButton, false, false);

			removeCategoryButton = new Button ();
			removeCategoryButton.Label = GettextCatalog.GetString ("Remove Category");
			removeCategoryButton.Sensitive = false;
			buttonsVBox.PackStart (removeCategoryButton, false, false);

			categoryInformationVBox = new VBox ();
			categoryInformationVBox.Sensitive = false;
			mainVBox.PackStart (categoryInformationVBox, false, false);

			string title = GettextCatalog.GetString ("Top Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			topLevelCategoryWidget = new TemplateCategoryWidget ();
			categoryInformationVBox.PackStart (topLevelCategoryWidget);

			title = GettextCatalog.GetString ("Second Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			secondLevelCategoryWidget = new TemplateCategoryWidget ();
			categoryInformationVBox.PackStart (secondLevelCategoryWidget);

			title = GettextCatalog.GetString ("Third Level");
			AddCategorySectionTitleLabel (categoryInformationVBox, title);

			thirdLevelCategoryWidget = new TemplateCategoryWidget ();
			categoryInformationVBox.PackStart (thirdLevelCategoryWidget);

			Content = mainVBox;
		}

		static void AddCategorySectionTitleLabel (VBox vbox, string title)
		{
			var titleLabel = new Label ();
			titleLabel.Markup = "<b>" + title + "</b>";
			titleLabel.TextAlignment = Alignment.Start;

			vbox.PackStart (titleLabel);
		}
	}
}

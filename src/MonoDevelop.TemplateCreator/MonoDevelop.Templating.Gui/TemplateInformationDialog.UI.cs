//
// TemplateInformationDialog.UI.cs
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
using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.Templating.Gui
{
	partial class TemplateInformationDialog : Dialog
	{
		DialogButton cancelButton;
		DialogButton okButton;
		TemplateTextEntry authorTextEntry;
		TemplateTextEntry shortNameTextEntry;
		TemplateTextEntry identityTextEntry;
		TemplateTextEntry groupIdentityTextEntry;
		TemplateTextEntry defaultProjectNameTextEntry;
		TemplateTextEntry displayNameTextEntry;
		TemplateTextEntry categoryTextEntry;
		TemplateTextEntry descriptionTextEntry;
		TemplateTextEntry fileFormatExcludeTextEntry;
		Button selectCategoryButton;
		List<Label> allLabels = new List<Label> ();

		void Build ()
		{
			Title = GettextCatalog.GetString ("Template Information");
			Width = 640;

			var mainVBox = new VBox ();
			mainVBox.Margin = 15;
			Content = mainVBox;

			var mainLabel = new Label ();
			mainLabel.MarginBottom = 10;
			mainLabel.Text = GettextCatalog.GetString ("This will create a '.template.config' folder in the root of the project.");
			mainVBox.PackStart (mainLabel);

			authorTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Author:"));
			allLabels.Add (authorTextEntry.Label);

			displayNameTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Display Name:"),
				GettextCatalog.GetString ("Name of template displayed in the New Project dialog."));
			allLabels.Add (displayNameTextEntry.Label);

			descriptionTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Description:"),
				GettextCatalog.GetString ("Template description displayed in the New Project dialog."));
			allLabels.Add (descriptionTextEntry.Label);

			categoryTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Category:"),
				GettextCatalog.GetString ("Defines where the project template will be displayed in the New Project dialog."));
			allLabels.Add (categoryTextEntry.Label);

			fileFormatExcludeTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("File Format Exclude:"),
				GettextCatalog.GetString ("A semi-colon separated list of files (e.g. 'info.plist;*.min.js') which will not be formatted when creating the project."));
			allLabels.Add (fileFormatExcludeTextEntry.Label);

			selectCategoryButton = new Button ();
			selectCategoryButton.Label = "\u2026";
			categoryTextEntry.HBox.PackStart (selectCategoryButton);

			shortNameTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Short Name:"),
				GettextCatalog.GetString ("Name that can be used to reference this template when using the .NET Core command line. Not used in {0}.", BrandingService.ApplicationName));
			allLabels.Add (shortNameTextEntry.Label);

			defaultProjectNameTextEntry = CreateTemplateTextEntry (
				mainVBox,
					GettextCatalog.GetString ("Default Project Name:"),
					GettextCatalog.GetString ("Default project name when using the .NET Core command line. Not used in {0}.", BrandingService.ApplicationName));
			allLabels.Add (defaultProjectNameTextEntry.Label);

			identityTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Identity:"),
				GettextCatalog.GetString ("Unique id for the template."));
			allLabels.Add (identityTextEntry.Label);

			groupIdentityTextEntry = CreateTemplateTextEntry (
				mainVBox,
				GettextCatalog.GetString ("Group Identity:"),
				GettextCatalog.GetString ("This should typically be a substring of the template's identity."));
			allLabels.Add (groupIdentityTextEntry.Label);

			// OK and Cancel buttons.
			cancelButton = new DialogButton (Command.Cancel);
			cancelButton.Clicked += (sender, e) => Close ();

			Buttons.Add (cancelButton);

			okButton = new DialogButton (Command.Ok);
			Buttons.Add (okButton);
		}

		static TemplateTextEntry CreateTemplateTextEntry (VBox vbox, string labelText, string tooltipText = null)
		{
			var hbox = new HBox ();
			vbox.PackStart (hbox);

			if (!string.IsNullOrEmpty (tooltipText)) {
				hbox.TooltipText = tooltipText;
			}

			var label = new Label ();
			label.Text = labelText;
			hbox.PackStart (label);

			var textEntry = new TextEntry ();
			hbox.PackStart (textEntry, true, true);

			return new TemplateTextEntry {
				HBox = hbox,
				Label = label,
				TextEntry = textEntry
			};
		}

		struct TemplateTextEntry {
			public HBox HBox;
			public Label Label;
			public TextEntry TextEntry;
		}

		/// <summary>
		/// Ensure labels have the same width.
		/// </summary>
		protected override void OnBoundsChanged (BoundsChangedEventArgs a)
		{
			double width = 0;
			foreach (Label label in allLabels) {
				width = Math.Max (width, label.WindowBounds.Width);
			}

			foreach (Label label in allLabels) {
				if (label.WindowBounds.Width < width) {
					label.WidthRequest = width;
				}
			}

			base.OnBoundsChanged (a);
		}
	}
}

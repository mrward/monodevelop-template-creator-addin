//
// TemplateCategoryWidget.UI.cs
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
	partial class TemplateCategoryWidget : Widget
	{
		HBox hbox;
		TextEntry idTextEntry;
		TextEntry nameTextEntry;

		void Build ()
		{
			hbox = new HBox ();
			hbox.Margin = 10;

			Content = hbox;

			var idLabel = new Label ();
			idLabel.Text = GettextCatalog.GetString ("Id:");
			hbox.PackStart (idLabel);

			idTextEntry = new TextEntry ();
			hbox.PackStart (idTextEntry);

			var nameLabel = new Label ();
			nameLabel.Text = GettextCatalog.GetString ("Name:");
			hbox.PackStart (nameLabel);

			nameTextEntry = new TextEntry ();
			hbox.PackStart (nameTextEntry);
		}
	}
}

//
// TemplatingOutputPad.cs
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
using AppKit;
using MonoDevelop.Components;
using MonoDevelop.Components.Declarative;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components.LogView;

namespace MonoDevelop.Templating.Gui
{
	public class TemplatingOutputPad : PadContent
	{
		static TemplatingOutputPad instance;
		static NSView logView;
		static LogViewProgressMonitor progressMonitor;
		static LogViewController logViewController;
		ToolbarButtonItem clearButton;

		public TemplatingOutputPad ()
		{
			instance = this;

			CreateLogView ();
		}

		static void CreateLogView ()
		{
			if (logViewController != null) {
				return;
			}

			logViewController = new LogViewController ("LogMonitor");
			logView = logViewController.Control.GetNativeWidget<NSView> ();

			// Need to create a progress monitor to avoid a null reference exception
			// when LogViewController.WriteText is called.
			progressMonitor = (LogViewProgressMonitor)logViewController.GetProgressMonitor ();
		}

		public static TemplatingOutputPad Instance {
			get { return instance; }
		}

		protected override void Initialize (IPadWindow window)
		{
			var toolbar = new Toolbar ();

			clearButton = new ToolbarButtonItem (toolbar.Properties, nameof (clearButton));
			clearButton.Icon = Stock.Broom;
			clearButton.Clicked += ButtonClearClick;
			clearButton.Tooltip = GettextCatalog.GetString ("Clear");
			toolbar.AddItem (clearButton);

			window.SetToolbar (toolbar, DockPositionType.Right);
		}

		public override Control Control {
			get { return logView; }
		}

		public static LogViewController LogView {
			get { return logViewController; }
		}

		void ButtonClearClick (object sender, EventArgs e)
		{
			Clear ();
		}

		public static void Clear ()
		{
			if (logViewController != null) {
				Runtime.RunInMainThread (() => {
					logViewController.Clear ();
				});
			}
		}

		public static void WriteText (string message)
		{
			Runtime.RunInMainThread (() => {
				CreateLogView ();
				logViewController.WriteText (progressMonitor, message + Environment.NewLine);
			});
		}

		public static void WriteError (string message)
		{
			Runtime.RunInMainThread (() => {
				CreateLogView ();
				logViewController.WriteError (progressMonitor, message + Environment.NewLine);
			});
		}
	}
}

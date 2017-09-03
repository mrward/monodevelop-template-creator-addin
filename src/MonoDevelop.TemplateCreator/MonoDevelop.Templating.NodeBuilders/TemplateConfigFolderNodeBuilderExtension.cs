﻿//
// TemplateConfigFolderNodeBuilderExtension.cs
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
using System.IO;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using Xwt.Drawing;

namespace MonoDevelop.Templating.NodeBuilders
{
	class TemplateConfigFolderNodeBuilderExtension : NodeBuilderExtension
	{
		public override bool CanBuildNode (Type dataType)
		{
			return typeof (TemplateConfigFolder).IsAssignableFrom (dataType);
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			var folder = (TemplateConfigFolder)dataObject;
			return folder.HasFiles ();
		}

		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
			var folder = (TemplateConfigFolder)dataObject;

			foreach (string file in Directory.EnumerateFiles (folder.BaseDirectory)) {
				var node = new SystemFile (file, folder.Project);
				treeBuilder.AddChild (node);
			}

			foreach (string directory in Directory.EnumerateDirectories (folder.BaseDirectory)) {
				var node = new TemplateConfigFolder (directory, folder.DotNetProject);
				treeBuilder.AddChild (node);
			}
		}

		/// <summary>
		/// Ensure folder is faded so it does not look like it is part of the project.
		/// </summary>
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			FadeFolderIcon (nodeInfo);
		}

		void FadeFolderIcon (NodeInfo nodeInfo)
		{
			nodeInfo.Icon = FadeIcon (nodeInfo.Icon);
			nodeInfo.ClosedIcon = FadeIcon (nodeInfo.ClosedIcon);
		}

		Image FadeIcon (Image icon)
		{
			Image fadedIcon = Context.GetComposedIcon (icon, "fade");
			if (fadedIcon == null) {
				fadedIcon = icon.WithAlpha (0.5);
				Context.CacheComposedIcon (fadedIcon, "fade", icon);
			}
			return fadedIcon;
		}
	}
}

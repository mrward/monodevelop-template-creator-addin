//
// SolutionTemplateConfigFolderNodeBuilder.cs
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
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Templating.Commands;
using Xwt.Drawing;

namespace MonoDevelop.Templating.NodeBuilders
{
	public class SolutionTemplateConfigFolderNodeBuilder : TypeNodeBuilder
	{
		Image folderOpenIcon;
		Image folderClosedIcon;

		public override Type NodeDataType {
			get { return typeof (SolutionTemplateConfigFolder); }
		}

		public override Type CommandHandlerType {
			get { return typeof (SolutionTemplateConfigFolderNodeCommandHandler); }
		}

		protected override void Initialize ()
		{
			base.Initialize ();

			folderOpenIcon = Context.GetIcon (Stock.OpenFolder);
			folderClosedIcon = Context.GetIcon (Stock.ClosedFolder);
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			var folder = (SolutionTemplateConfigFolder)dataObject;
			return folder.HasFiles ();
		}

		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
			var folder = (SolutionTemplateConfigFolder)dataObject;

			foreach (string file in Directory.EnumerateFiles (folder.BaseDirectory)) {
				var node = new SystemFile (file, folder.Solution);
				treeBuilder.AddChild (node);
			}

			foreach (string directory in Directory.EnumerateDirectories (folder.BaseDirectory)) {
				var node = new SolutionTemplateConfigFolder (directory, folder.Solution);
				treeBuilder.AddChild (node);
			}
		}

		/// <summary>
		/// Ensure folder is faded so it does not look like it is part of the project.
		/// </summary>
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var folder = (SolutionTemplateConfigFolder)dataObject;

			nodeInfo.Label = folder.Name;
			nodeInfo.Icon = folderOpenIcon;
			nodeInfo.ClosedIcon = folderClosedIcon;

			nodeInfo.FadeFolderIcon (Context);
		}

		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			var folder = (SolutionTemplateConfigFolder)dataObject;
			return folder.Name;
		}

		public override int GetSortIndex (ITreeNavigator node)
		{
			// Same sort index as the default SolutionFolderNodeBuilder.
			return -1000;
		}
	}
}

//
// SolutionNodeBuilderExtension.cs
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
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;

namespace MonoDevelop.Templating.NodeBuilders
{
	class SolutionNodeBuilderExtension : NodeBuilderExtension
	{
		public SolutionNodeBuilderExtension ()
		{
			TemplatingServices.EventsService.SolutionTemplateFileCreated += TemplateFileCreated;
			TemplatingServices.EventsService.RefreshSolutionTemplateConfigFolder += RefreshSolutionTemplateConfigFolder;
		}

		public override void Dispose ()
		{
			TemplatingServices.EventsService.SolutionTemplateFileCreated -= TemplateFileCreated;
			TemplatingServices.EventsService.RefreshSolutionTemplateConfigFolder -= RefreshSolutionTemplateConfigFolder;
		}

		public override bool CanBuildNode (Type dataType)
		{
			return typeof (Solution).IsAssignableFrom (dataType);
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			if (ShowingAllFiles (builder))
				return false;

			return HasTemplateConfigDirectory (dataObject);
		}

		bool ShowingAllFiles (ITreeBuilder builder)
		{
			return builder.Options ["ShowAllFiles"];
		}

		bool HasTemplateConfigDirectory (object dataObject)
		{
			var solution = dataObject as Solution;
			if (solution == null)
				return false;

			return solution.HasTemplateConfigDirectory ();
		}

		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
			if (ShowingAllFiles (treeBuilder))
				return;

			if (!HasTemplateConfigDirectory (dataObject))
				return;

			var solution = (Solution)dataObject;
			//if (solution.TemplateConfigDirectoryExistsInSolution ())
			//	return;

			var folder = new SolutionTemplateConfigFolder (solution);
			treeBuilder.AddChild (folder);
		}

		void TemplateFileCreated (object sender, SolutionEventArgs e)
		{
			RefreshSolutionNode (e.Solution);
		}

		void RefreshSolutionTemplateConfigFolder (object sender, SolutionEventArgs e)
		{
			RefreshSolutionNode (e.Solution);
		}

		void RefreshSolutionNode (Solution solution)
		{
			Runtime.RunInMainThread (() => {
				ITreeBuilder builder = Context.GetTreeBuilder (solution);
				if (builder != null) {
					builder.UpdateAll ();
				}
			});
		}
	}
}

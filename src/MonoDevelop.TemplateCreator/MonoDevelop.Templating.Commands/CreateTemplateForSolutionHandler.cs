//
// CreateTemplateForSolutionHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating.Commands
{
	class CreateTemplateForSolutionHandler : CreateTemplateHandlerBase
	{
		protected override void Update (CommandInfo info)
		{
			info.Visible = IsCommandVisible ();
		}

		Solution GetSelectedSolution ()
		{
			return IdeApp.ProjectOperations.CurrentSelectedSolution as Solution;
		}

		bool IsCommandVisible ()
		{
			Solution solution = GetSelectedSolution ();
			if (solution != null) {
				return solution.HasAnyDotNetProjects () &&
					!solution.HasTemplateJsonFile () &&
					!solution.AnyProjectHasTemplateJsonFile ();
			}

			return false;
		}

		protected override void Run ()
		{
			Solution solution = GetSelectedSolution ();
			if (solution == null)
				return;

			try {
				var viewModel = new TemplateInformation (solution);
				using (var dialog = new TemplateInformationDialog (viewModel)) {
					if (dialog.ShowWithParent ()) {
						FilePath templateJsonFile = CreateTemplateJsonFile (solution, viewModel);
						TemplatingServices.EventsService.OnTemplateFileCreated (solution);
						RegisterTemplateFolder (templateJsonFile);
					}
				}
			} catch (Exception ex) {
				LoggingService.LogError ("Error creating template.json", ex);
				MessageService.ShowError ("Unable to create template.json file.", ex.Message);
			}
		}
	}
}

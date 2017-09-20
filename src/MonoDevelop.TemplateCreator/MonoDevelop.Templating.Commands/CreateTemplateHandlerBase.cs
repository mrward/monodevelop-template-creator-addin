//
// CreateTemplateHandlerBase.cs
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

using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Templating.Commands
{
	abstract class CreateTemplateHandlerBase : CommandHandler
	{
		protected FilePath CreateTemplateJsonFile (Solution solution, TemplateInformation viewModel)
		{
			return CreateTemplateJsonFile (solution.BaseDirectory, viewModel, solution.Policies);
		}

		protected FilePath CreateTemplateJsonFile (DotNetProject project, TemplateInformation viewModel)
		{
			return CreateTemplateJsonFile (project.BaseDirectory, viewModel, project.Policies);
		}

		FilePath CreateTemplateJsonFile (
			FilePath baseDirectory,
			TemplateInformation viewModel,
			PolicyBag policies)
		{
			var templateJsonFileCreator = new TemplateJsonFileCreator ();
			templateJsonFileCreator.CreateInDirectory (baseDirectory, viewModel);

			FileFormatter.FormatFile (policies, templateJsonFileCreator.TemplateJsonFilePath);

			IdeApp.Workbench.OpenDocument (
				templateJsonFileCreator.TemplateJsonFilePath,
				null,
				true);

			return templateJsonFileCreator.TemplateJsonFilePath;
		}

		protected void RegisterTemplateFolder (FilePath templateJsonFile)
		{
			string templateFolder = templateJsonFile.ParentDirectory.ParentDirectory;
			TemplatingServices.Options.AddTemplateFolder (templateFolder);

			// Force a refresh of the template cache. This handles the case
			// where the folder was already added as a template folder and the
			// template.json file was added afterwards.
			TemplatingServices.EventsService.OnTemplateFoldersChanged ();
		}
	}
}

//
// TemplateProcessor.cs
//
// Author:
//       David Karlaš <david.karlas@xamarin.com>
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

// Based on MicrosoftTemplateEngineProjectTemplatingProvider.cs from MonoDevelop.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using MonoDevelop.Core;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeFormatting;
using MonoDevelop.Ide.Projects;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Policies;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating
{
	class TemplateProcessor
	{
		TemplatingEngine templateEngine = TemplatingServices.TemplatingEngine;
		CustomSolutionTemplate template;
		NewProjectConfiguration config;
		SolutionFolder parentFolder;
		List<IWorkspaceFileObject> workspaceItems;

		public TemplateProcessor (
			CustomSolutionTemplate template,
			NewProjectConfiguration config,
			SolutionFolder parentFolder)
		{
			this.template = template;
			this.config = config;
			this.parentFolder = parentFolder;
		}

		public async Task<ProcessedTemplateResult> ProcessTemplate ()
		{
			string[] filesBeforeCreation = Directory.GetFiles (
				config.ProjectLocation,
				"*",
				SearchOption.AllDirectories);

			var parameters = new Dictionary<string, string> ();
			var result = await templateEngine.Create (template, config, parameters);

			if (result.Status != CreationResultStatus.Success) {
				string message = GettextCatalog.GetString ("Could not create template. Id='{0}' {1} {2}", template.Id, result.Status, result.Message);
				throw new InvalidOperationException (message);
			}

			List<string> filesToOpen = GetFilesToOpen (result).ToList ();
			workspaceItems = await GetWorkspaceItems (result);

			ProcessedCustomTemplateResult processResult = CreateTemplateResult ();

			await FormatFiles (filesBeforeCreation);

			processResult.AddFilesToOpen (filesToOpen);
			return processResult;
		}

		ProcessedCustomTemplateResult CreateTemplateResult ()
		{
			if (parentFolder == null) {
				var solution = new Solution ();
				solution.SetLocation (config.SolutionLocation, config.SolutionName);
				foreach (var item in workspaceItems.Cast<SolutionFolderItem> ()) {
					AddConfigurations (solution, item);
					solution.RootFolder.AddItem (item);
				}

				return new ProcessedCustomTemplateResult (solution, config.ProjectLocation);
			}

			return new ProcessedCustomTemplateResult (
				workspaceItems,
				parentFolder.ParentSolution.FileName,
				config.ProjectLocation);
		}

		static void AddConfigurations (Solution solution, SolutionFolderItem item)
		{
			var configurationTarget = item as IConfigurationTarget;
			if (configurationTarget == null)
				return;

			foreach (ItemConfiguration configuration in configurationTarget.Configurations) {
				bool flag = false;
				foreach (SolutionConfiguration solutionCollection in solution.Configurations) {
					if (solutionCollection.Id == configuration.Id) {
						flag = true;
					}
				}
				if (!flag) {
					solution.AddConfiguration (configuration.Id, true);
				}
			}
		}

		IEnumerable<string> GetFilesToOpen (TemplateCreationResult result)
		{
			foreach (var postAction in result.ResultInfo.PostActions) {
				switch (postAction.ActionId.ToString ().ToUpper ()) {
					case "84C0DA21-51C8-4541-9940-6CA19AF04EE6":
						if (postAction.Args.TryGetValue ("files", out var files))
							foreach (var fi in files.Split (';'))
								if (int.TryParse (fi.Trim (), out var i))
									yield return (Path.Combine (config.ProjectLocation, GetPath (result.ResultInfo.PrimaryOutputs [i])));
						break;
				}
			}
		}

		string GetPath (ICreationPath path)
		{
			if (Path.DirectorySeparatorChar != '\\')
				return path.Path.Replace ('\\', Path.DirectorySeparatorChar);
			return path.Path;
		}

		async Task<List<IWorkspaceFileObject>> GetWorkspaceItems (TemplateCreationResult result)
		{
			var items = new List<IWorkspaceFileObject> ();

			foreach (ICreationPath path in result.ResultInfo.PrimaryOutputs) {
				string fullPath = Path.Combine (config.ProjectLocation, GetPath (path));
				if (Services.ProjectService.IsSolutionItemFile (fullPath)) {
					items.Add (await Services.ProjectService.ReadSolutionItem (new ProgressMonitor (), fullPath));
				}
			}

			return items;
		}

		/// <summary>
		/// Format all source files generated during the project creation
		/// </summary>
		async Task FormatFiles (string[] filesBeforeCreation)
		{
			foreach (var p in workspaceItems.OfType<Project> ()) {
				foreach (var file in p.Files) {
					//Format only newly created files
					if (!filesBeforeCreation.Contains ((string)file.FilePath, FilePath.PathComparer)) {
						if (template.CanFormatFile (file.FilePath)) {
							await FileFormatter.FormatFileAsync (parentFolder, p, file.FilePath);
						}
					}
				}
			}
		}
	}
}

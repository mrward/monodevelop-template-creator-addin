//
// TemplatingEngine.cs
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.TemplatePackage;
using Microsoft.TemplateEngine.Edge;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using MonoDevelop.Ide.Projects;

namespace MonoDevelop.Templating
{
	class TemplatingEngine
	{
		TemplateCreator templateCreator;
		CustomTemplateEngineHost host;
		EngineEnvironmentSettings settings;
		TemplatePackageProvider templatePackageProvider;
		TemplatePackageManager templatePackageManager;
		bool loaded;
		List<CustomSolutionTemplate> templates = new List<CustomSolutionTemplate> ();

		void Initialize ()
		{
			host = new CustomTemplateEngineHost ();

			IEnvironment environment = new DefaultEnvironment ();
			IPathInfo pathInfo = new DefaultPathInfo (environment, host, globalSettingsDir: null, hostSettingsDir: null, hostVersionSettingsDir: null);
			settings = new EngineEnvironmentSettings (
				host,
				virtualizeSettings: false,
				environment: environment,
				pathInfo: pathInfo);

			var defaultComponents = Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Components.AllComponents
				.Concat (Microsoft.TemplateEngine.Edge.Components.AllComponents);

			foreach ((Type Type, IIdentifiedComponent Instance) component in defaultComponents) {
				settings.Components.AddComponent (component.Type, component.Instance);
			}

			var factory = new TemplatePackageProviderFactory ();
			templatePackageProvider = (TemplatePackageProvider)factory.CreateProvider (settings);
			settings.Components.AddComponent (typeof (ITemplatePackageProviderFactory), factory);

			templateCreator = new TemplateCreator (settings);
			templatePackageManager = new TemplatePackageManager (settings);
		}

		public void ReloadTemplates ()
		{
			if (settings == null)
				return;

			templates.Clear ();
			loaded = false;
		}

		void EnsureInitialized ()
		{
			if (host == null) {
				Initialize ();
			}
		}

		public async Task LoadTemplates ()
		{
			if (loaded || !TemplatingServices.Options.TemplateFolders.Any ())
				return;

			EnsureInitialized ();

			var scanPaths = new List<string> ();

			foreach (string folder in TemplatingServices.Options.TemplateFolders) {
				if (Directory.Exists (folder)) {
					scanPaths.Add (folder);
				}
			}

			try {
				templatePackageProvider.UpdatePackages (nupkgs: scanPaths);
			} catch (Exception ex) {
				TemplatingServices.LogError ("Could not update scan paths", ex);
			}

			try {
				await templatePackageManager.RebuildTemplateCacheAsync (CancellationToken.None);
			} catch (Exception ex) {
				TemplatingServices.LogError ("Could not rebuild template cache", ex);
			}

			IReadOnlyList<ITemplateInfo> templateInfos = await templatePackageManager.GetTemplatesAsync (CancellationToken.None);

			foreach (ITemplateInfo templateInfo in templateInfos) {
				var template = new CustomSolutionTemplate (templateInfo);
				templates.Add (template);
			}

			loaded = true;
		}

		public IEnumerable<CustomSolutionTemplate> Templates {
			get { return templates; }
		}

		public Task<ITemplateCreationResult> Create (
			CustomSolutionTemplate template,
			NewProjectConfiguration config,
			IReadOnlyDictionary<string, string> parameters)
		{
			return templateCreator.InstantiateAsync (
				template.Info,
				config.ProjectName,
				config.ProjectName,
				config.ProjectLocation,
				parameters,
				forceCreation: false,
				dryRun: false,
				cancellationToken: CancellationToken.None
			);
		}
	}
}

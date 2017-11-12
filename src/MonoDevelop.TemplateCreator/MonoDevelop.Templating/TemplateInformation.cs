//
// TemplateInformation.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoDevelop.Core;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Projects;

namespace MonoDevelop.Templating
{
	class TemplateInformation : IStringTagModel
	{
		DotNetProject project;
		List<DotNetProject> projects;
		FilePath baseDirectory;

		public TemplateInformation (DotNetProject project)
			: this (project.BaseDirectory, new [] { project })
		{
		}

		public TemplateInformation (Solution solution)
			: this (solution.BaseDirectory, solution.GetAllDotNetProjects ())
		{
		}

		public TemplateInformation (FilePath baseDirectory, IEnumerable<DotNetProject> projects)
		{
			this.projects = projects.OrderBy (p => p.Name).ToList ();
			this.project = projects.First ();
			this.baseDirectory = baseDirectory;

			GenerateDefaults ();
		}

		public string Author { get; set; }
		public string DisplayName { get; set; }
		public string ShortName { get; set; }
		public string DefaultProjectName { get; set; }
		public string Identity { get; set; }
		public string GroupIdentity { get; set; }
		public string SourceName { get; set; }
		public string Language { get; set; }
		public string CategoryTagName { get; set; }
		public string Category { get; set; }
		public string Description { get; set; }
		public string[] ProjectFilePrimaryOutputs { get; set; }
		public string[] ProjectGuids { get; set; }

		void GenerateDefaults ()
		{
			Author = AuthorInformation.Default.Name;
			DefaultProjectName = project.Name;
			DisplayName = project.Name;
			Language = project.LanguageName;
			ShortName = project.Name;
			Description = string.Empty;

			Category = "other/net/general";
			CategoryTagName = TemplateCategoryTagNameProvider.DefaultCategoryTagName;

			GroupIdentity = $"MyTemplate.{project.Name}";
			Identity = $"{GroupIdentity}.{project.GetTemplateLanguageName ()}";

			SourceName = project.FileName.FileNameWithoutExtension;

			ProjectFilePrimaryOutputs = GetProjectFilePrimaryOutputs ().ToArray ();
			ProjectGuids = GetProjectGuids ().ToArray ();
		}

		public object GetValue (string name)
		{
			var bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
			PropertyInfo property = GetType ().GetProperty (name, bindingFlags);
			if (property != null) {
				return property.GetValue (this) as string;
			}
			return null;
		}

		IEnumerable<string> GetProjectGuids ()
		{
			return projects.Where (p => !string.IsNullOrEmpty (p.ItemId))
				.Select (p => p.ItemId);
		}

		IEnumerable<string> GetProjectFilePrimaryOutputs ()
		{
			return projects.Select (p => p.FileName.ToRelative (baseDirectory).ToString ());
		}
	}
}

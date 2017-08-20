//
// TemplateInformationViewModel.cs
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

using MonoDevelop.Projects;
using MonoDevelop.Core.StringParsing;
using System;

namespace MonoDevelop.Templating
{
	class TemplateInformationViewModel : IStringTagModel
	{
		DotNetProject project;

		public TemplateInformationViewModel (DotNetProject project)
		{
			this.project = project;
			GenerateDefaults ();
		}

		public string Author { get; set; }
		public string DisplayName { get; set; }
		public string ShortName { get; set; }
		public string DefaultProjectName { get; set; }
		public string Identity { get; set; }
		public string GroupIdentity { get; set; }
		public string SourceName { get; set; }

		void GenerateDefaults ()
		{
			Author = AuthorInformation.Default.Name;
			DefaultProjectName = project.Name;
			DisplayName = project.Name;
			ShortName = project.Name;

			GroupIdentity = $"MyTemplate.{project.Name}";
			Identity = $"{GroupIdentity}.{project.GetTemplateLanguageName ()}";

			SourceName = project.FileName.FileNameWithoutExtension;
		}

		public object GetValue (string name)
		{
			if (IsMatch (nameof (Author), name)) {
				return Author;
			} else if (IsMatch (nameof (DisplayName), name)) {
				return DisplayName;
			} else if (IsMatch (nameof (ShortName), name)) {
				return ShortName;
			} else if (IsMatch (nameof (DefaultProjectName), name)) {
				return DefaultProjectName;
			} else if (IsMatch (nameof (Identity), name)) {
				return Identity;
			} else if (IsMatch (nameof (GroupIdentity), name)) {
				return GroupIdentity;
			} else if (IsMatch (nameof (SourceName), name)) {
				return SourceName;
			}
			return null;
		}

		static bool IsMatch (string x, string y)
		{
			return StringComparer.OrdinalIgnoreCase.Equals (x, y);
		}
	}
}

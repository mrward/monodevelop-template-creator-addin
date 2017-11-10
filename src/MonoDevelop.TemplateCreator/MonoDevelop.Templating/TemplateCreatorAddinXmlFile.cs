//
// TemplateCreatorAddinXmlFile.cs
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
using System.IO;
using System.Linq;
using System.Xml;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating
{
	class TemplateCreatorAddinXmlFile
	{
		static readonly string addinXmlTemplateFileName;
		static readonly string addinXmlFileName;
		static List<TemplateCategoryViewModel> originalCategories;

		const string PlaceHolderText = "<!--CUSTOM-PROJECT-TEMPLATE-CATEGORIES-->";

		static TemplateCreatorAddinXmlFile ()
		{
			string directory = Path.GetDirectoryName (typeof (TemplateCreatorAddinXmlFile).Assembly.Location);
			addinXmlTemplateFileName = Path.Combine (directory, "TemplateCreator.addin-template.xml");
			addinXmlFileName = Path.Combine (directory, "TemplateCreator.addin.xml");
		}

		public static bool IsModified { get; private set; }

		public static void UpdateTemplateCategories (IEnumerable<TemplateCategoryViewModel> categories)
		{
			string text = ProjectTemplateCategoriesXmlGenerator.Generate (categories);

			string addinXml = File.ReadAllText (addinXmlTemplateFileName);
			addinXml = addinXml.Replace (PlaceHolderText, text);

			File.WriteAllText (addinXmlFileName, addinXml);

			IsModified = true;
		}

		public static IEnumerable<TemplateCategoryViewModel> GetOriginalTemplateCategories ()
		{
			if (originalCategories != null) {
				return originalCategories;
			}
			return ReadTemplateCategories ();
		}

		public static IEnumerable<TemplateCategoryViewModel> ReadTemplateCategories ()
		{
			var categories = ReadTemplateCategoriesInternal ().ToList ();
			if (originalCategories == null) {
				originalCategories = categories;
			}
			return categories;
		}

		static IEnumerable<TemplateCategoryViewModel> ReadTemplateCategoriesInternal ()
		{
			var doc = new XmlDocument ();
			doc.Load (addinXmlFileName);

			foreach (XmlElement node in doc.SelectNodes ("//Extension[@path='/MonoDevelop/Ide/ProjectTemplateCategories']/Category")) {
				TemplateCategory category = CreateTemplateCategory (node);
				yield return new TemplateCategoryViewModel (null, category);
			}
		}

		static TemplateCategory CreateTemplateCategory (XmlElement node)
		{
			var category = new TemplateCategory (
				node.GetAttribute ("id"),
				node.GetAttribute ("name"),
				null);

			foreach (XmlElement childNode in node.SelectNodes ("./Category")) {
				category.AddCategory (CreateTemplateCategory (childNode));
			}

			return category;
		}
	}
}

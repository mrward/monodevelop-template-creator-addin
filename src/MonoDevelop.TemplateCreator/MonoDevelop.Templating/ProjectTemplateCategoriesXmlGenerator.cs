//
// ProjectTemplateCategoriesXmlGenerator.cs
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
using System.Text;
using System.Xml;
using MonoDevelop.Templating.Gui;

namespace MonoDevelop.Templating
{
	class ProjectTemplateCategoriesXmlGenerator
	{
		IEnumerable<TemplateCategoryViewModel> categories;
		XmlWriter writer;

		ProjectTemplateCategoriesXmlGenerator (IEnumerable<TemplateCategoryViewModel> categories)
		{
			this.categories = categories;
		}

		public static string Generate (IEnumerable<TemplateCategoryViewModel> categories)
		{
			var generator = new ProjectTemplateCategoriesXmlGenerator (categories);
			return generator.Generate ();
		}

		string Generate ()
		{
			var builder = new StringBuilder ();

			var settings = new XmlWriterSettings {
				Indent = true,
				IndentChars = "\t",
				OmitXmlDeclaration = true
			};

			using (writer = XmlWriter.Create (builder, settings)) {

				foreach (TemplateCategoryViewModel category in categories) {
					WriteCategory (category);
				}
			}

			return builder.ToString ();
		}

		void WriteCategory (TemplateCategoryViewModel category)
		{
			writer.WriteStartElement ("Category");

			writer.WriteAttributeString ("id", GetAttributeValueOrDefault (category.Id, "id"));
			writer.WriteAttributeString ("name", GetAttributeValueOrDefault (category.Name, "Category"));

			foreach (TemplateCategoryViewModel childCategory in category.GetChildCategories ()) {
				WriteCategory (childCategory);
			}

			writer.WriteEndElement ();
		}

		static string GetAttributeValueOrDefault (string value, string defaultValue)
		{
			if (string.IsNullOrEmpty (value)) {
				return defaultValue;
			}

			return value;
		}
	}
}

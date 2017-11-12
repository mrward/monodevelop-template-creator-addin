//
// TemplateJsonFileCreator.cs
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

using System.IO;
using System.Text;
using MonoDevelop.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MonoDevelop.Templating
{
	class TemplateJsonFileCreator
	{
		public void CreateInDirectory (FilePath rootDirectory, TemplateInformation template)
		{
			FilePath templateConfigDirectory = rootDirectory.Combine (".template.config");
			Directory.CreateDirectory (templateConfigDirectory);

			CopyTemplateJsonFile (templateConfigDirectory, template);
		}

		void CopyTemplateJsonFile (FilePath templateConfigDirectory, TemplateInformation template)
		{
			FilePath sourceFilePath = GetDefaultTemplateJsonFilePath ();
			FilePath destinationFilePath = templateConfigDirectory.Combine (sourceFilePath.FileName);

			string json = File.ReadAllText (sourceFilePath);
			json = StringParserService.Parse (json, template);

			json = AddProjectInfo (template, json);

			File.WriteAllText (destinationFilePath, json, Encoding.UTF8);

			TemplateJsonFilePath = destinationFilePath;
		}

		FilePath GetDefaultTemplateJsonFilePath ()
		{
			string directory = Path.GetDirectoryName (GetType ().Assembly.Location);
			return FilePath.Build (directory, "ConfigurationFiles", "template.json");
		}

		public FilePath TemplateJsonFilePath { get; private set; }

		static string AddProjectInfo (TemplateInformation template, string json)
		{
			var jsonObject = JObject.Parse (json);

			// Set description here so any double quote characters are escaped.
			jsonObject ["description"] = template.Description;

			var guids = (JArray)jsonObject ["guids"];

			foreach (string projectGuid in template.ProjectGuids) {
				guids.Add (projectGuid);
			}

			var outputs = (JArray)jsonObject ["primaryOutputs"];

			foreach (string primaryOutput in template.ProjectFilePrimaryOutputs) {
				var pathProperty = new JObject {
					["path"] = primaryOutput
				};
				outputs.Add (pathProperty);
			}

			return ToString (jsonObject);
		}

		/// <summary>
		/// Indent with tabs otherwise tabs will not be used. The formatting
		/// option tabs to spaces handles the conversion if enabled.
		/// </summary>
		static string ToString (JObject jsonObject)
		{
			var json = new StringBuilder ();

			using (var writer = new StringWriter (json)) {
				using (var jsonWriter = new JsonTextWriter (writer)) {
					jsonWriter.IndentChar = '\t';
					jsonWriter.Indentation = 1;
					jsonWriter.Formatting = Formatting.Indented;
					jsonObject.WriteTo (jsonWriter);
				}
			}

			return json.ToString ();
		}
	}
}

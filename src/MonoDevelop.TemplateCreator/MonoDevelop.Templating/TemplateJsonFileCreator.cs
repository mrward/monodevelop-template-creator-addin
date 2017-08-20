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
using MonoDevelop.Core;

namespace MonoDevelop.Templating
{
	class TemplateJsonFileCreator
	{
		public void CreateInDirectory (FilePath rootDirectory)
		{
			FilePath templateConfigDirectory = rootDirectory.Combine (".template.config");
			Directory.CreateDirectory (templateConfigDirectory);

			CopyTemplateJsonFile (templateConfigDirectory);
		}

		void CopyTemplateJsonFile (FilePath templateConfigDirectory)
		{
			FilePath sourceFilePath = GetDefaultTemplateJsonFilePath ();
			FilePath destinationFilePath = templateConfigDirectory.Combine (sourceFilePath.FileName);

			File.Copy (sourceFilePath, destinationFilePath);

			TemplateJsonFilePath = destinationFilePath;
		}

		FilePath GetDefaultTemplateJsonFilePath ()
		{
			string directory = Path.GetDirectoryName (GetType ().Assembly.Location);
			return FilePath.Build (directory, "ConfigurationFiles", "template.json");
		}

		public FilePath TemplateJsonFilePath { get; private set; }
	}
}

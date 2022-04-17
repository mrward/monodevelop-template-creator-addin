//
// MicrosoftTemplateEngineFileFormatter.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2021 Microsoft Corporation
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
using System.Threading.Tasks;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Core.Text;
using MonoDevelop.Ide.Templates;
using MonoDevelop.Projects;

namespace MonoDevelop.Templating
{
    class MicrosoftTemplateEngineFileFormatter : SingleFileDescriptionTemplate
    {
        string content;

        public void FormatFile (SolutionFolderItem policyParent, Project project, string fileName)
        {
            try {
                content = File.ReadAllText (fileName);
#pragma warning disable CS0618 // Type or member is obsolete
				Stream stream = CreateFileContent (policyParent, project, string.Empty, fileName, string.Empty);
#pragma warning restore CS0618 // Type or member is obsolete
				WriteFile (stream, fileName);
            } finally {
                content = null;
            }
        }

        public async Task FormatFileAsync (SolutionFolderItem policyParent, Project project, string fileName)
        {
            try {
                TextContent textContent = await TextFileUtility.ReadAllTextAsync (fileName);
                content = textContent.Text;
                Stream stream = await CreateFileContentAsync (policyParent, project, string.Empty, fileName, string.Empty);
                WriteFile (stream, fileName);
            } finally {
                content = null;
            }
        }

        void WriteFile (Stream stream, string fileName)
        {
            byte[] buffer = new byte[2048];
            int nr;
            FileStream fs = null;
            try {
                fs = File.Create (fileName);
                while ((nr = stream.Read (buffer, 0, 2048)) > 0)
                    fs.Write (buffer, 0, nr);
            } finally {
                stream.Close ();
                if (fs != null)
                    fs.Close ();
            }
        }

        public override string CreateContent (Project project, Dictionary<string, string> tags, string language)
        {
            return content;
        }

        protected override string ProcessContent (string content, IStringTagModel tags)
        {
            return content;
        }
    }
}

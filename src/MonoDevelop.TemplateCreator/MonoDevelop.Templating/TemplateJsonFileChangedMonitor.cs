//
// TemplateJsonFileChangedMonitor.cs
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

using MonoDevelop.Core;

namespace MonoDevelop.Templating
{
	class TemplateJsonFileChangedMonitor
	{
		public TemplateJsonFileChangedMonitor()
		{
			FileService.FileChanged += FileChanged;
			FileService.FileRemoved += FileRemoved;
			FileService.FileMoved += FileMoved;
		}

		void FileChanged (object sender, FileEventArgs e)
		{
			foreach (FileEventInfo file in e) {
				OnFileChanged (file);
			}
		}

		/// <summary>
		/// Refresh the template folders if a template.json file has changed.
		/// </summary>
		void OnFileChanged (FileEventInfo file)
		{
			if (file.FileName.IsTemplateJsonFile ()) {
				TemplatingServices.EventsService.OnTemplateFoldersChanged ();
			}
		}

		void FileRemoved (object sender, FileEventArgs e)
		{
			foreach (FileEventInfo file in e) {
				OnFileRemoved (file);
			}
		}

		/// <summary>
		/// Refresh the template folders if a template.json file is removed or a
		/// .template.config folder is removed.
		/// </summary>
		void OnFileRemoved (FileEventInfo file)
		{
			if (file.FileName.IsTemplateJsonFile () || file.FileName.IsTemplateConfigDirectory ()) {
				TemplatingServices.EventsService.OnTemplateFoldersChanged ();
			}
		}

		void FileMoved (object sender, FileCopyEventArgs e)
		{
			foreach (FileCopyEventInfo file in e) {
				FileRenamed (file);
			}
		}

		void FileRenamed (FileCopyEventInfo file)
		{
			if (file.SourceFile.IsTemplateJsonFile () || file.TargetFile.IsTemplateJsonFile ()) {
				TemplatingServices.EventsService.OnTemplateFoldersChanged ();
			}
		}
	}
}

//
// SolutionTemplateConfigFolderNodeCommandHandler.cs
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
using System.IO;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Templating.NodeBuilders;

namespace MonoDevelop.Templating.Commands
{
	class SolutionTemplateConfigFolderNodeCommandHandler : NodeCommandHandler
	{
		public override bool CanDeleteMultipleItems()
		{
			return false;
		}

		public override bool CanDeleteItem()
		{
			return true;
		}

		[CommandUpdateHandler (EditCommands.Delete)]
		public void UpdateRemoveItem (CommandInfo info)
		{
			info.Enabled = CanDeleteItem ();
			info.Text = GettextCatalog.GetString ("Remove");
		}

		public override void DeleteItem ()
		{
			var folder = GetCurrentFolder ();
			if (folder == null)
				return;

			if (ConfirmDelete (folder.BaseDirectory)) {
				DeleteFolder (folder.BaseDirectory);

				// Should really refresh the solution template config folder node
				// by using the FileService events.
				TemplatingServices.EventsService.OnRefreshSolutionTemplateConfigFolder (folder.Solution);
			}
		}

		SolutionTemplateConfigFolder GetCurrentFolder ()
		{
			return CurrentNode?.DataItem as SolutionTemplateConfigFolder;
		}

		static bool ConfirmDelete (FilePath folder)
		{
			var question = new QuestionMessage {
				Text = GettextCatalog.GetString ("Are you sure you want to remove directory {0}?", folder),
				SecondaryText = GettextCatalog.GetString ("The directory and any files it contains will be permanently removed from your hard disk.")
			};

			question.Buttons.Add (AlertButton.Delete);
			question.Buttons.Add (AlertButton.Cancel);

			AlertButton result = MessageService.AskQuestion (question);
			return result == AlertButton.Delete;
		}

		static void DeleteFolder (FilePath folder)
		{
			try {
				if (Directory.Exists (folder)) {
					FileService.DeleteDirectory (folder);
				}
			}
			catch (Exception ex) {
				MessageService.ShowError (GettextCatalog.GetString ("The folder {0} could not be deleted: {1}", folder, ex.Message));
			}
		}
	}
}

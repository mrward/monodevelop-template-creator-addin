//
// NodeInfoExtensions.cs
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

using MonoDevelop.Ide.Gui.Components;
using Xwt.Drawing;

namespace MonoDevelop.Templating.NodeBuilders
{
	static class NodeInfoExtensions
	{
		public static void FadeFolderIcon (this NodeInfo nodeInfo, ITreeBuilderContext context)
		{
			nodeInfo.Icon = FadeIcon (nodeInfo.Icon, context);
			nodeInfo.ClosedIcon = FadeIcon (nodeInfo.ClosedIcon, context);
		}

		static Image FadeIcon (Image icon, ITreeBuilderContext context)
		{
			Image fadedIcon = context.GetComposedIcon (icon, "fade");
			if (fadedIcon == null) {
				fadedIcon = icon.WithAlpha (0.5);
				context.CacheComposedIcon (fadedIcon, "fade", icon);
			}
			return fadedIcon;
		}
	}
}

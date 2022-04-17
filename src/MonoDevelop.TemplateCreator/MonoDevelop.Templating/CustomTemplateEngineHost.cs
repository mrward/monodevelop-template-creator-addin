//
// CustomTemplateEngineHost.cs
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

namespace MonoDevelop.Templating
{
	class CustomTemplateEngineHost : Microsoft.TemplateEngine.Edge.DefaultTemplateEngineHost
	{
		static readonly string hostIdentifier = "MonoDevelopTemplateCreator";
		static readonly string hostVersion = "0.1";

		static readonly Dictionary<string, string> preferences = new Dictionary<string, string> {
			{ "prefs:language", "C#" }
		};

		public CustomTemplateEngineHost ()
			: this (hostIdentifier, hostVersion)
		{
		}

		public CustomTemplateEngineHost (string name, string version)
			: base (name,
				version,
				new Dictionary<string, string> { { "dotnet-cli-version", "0" } },
				null,
				new List<string> () { "vs-2017.3", "ide" },
				new TemplateEngineLoggerFactory ())
		{
		}

		public override bool TryGetHostParamDefault (string paramName, out string value)
		{
			if (paramName == "HostIdentifier") {
				value = HostIdentifier;
				return true;
			}
			value = null;
			return false;
		}
	}
}

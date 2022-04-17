//
// TemplatePackageProvider.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2022 Microsoft
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.TemplatePackage;

namespace MonoDevelop.Templating
{
	class TemplatePackageProvider : ITemplatePackageProvider
	{
		List<string> nupkgs = new List<string> ();
		IEngineEnvironmentSettings environmentSettings;

		public ITemplatePackageProviderFactory Factory { get; }

		public TemplatePackageProvider (ITemplatePackageProviderFactory factory, IEngineEnvironmentSettings environmentSettings)
		{
			Factory = factory;
			this.environmentSettings = environmentSettings;
		}

		public event Action TemplatePackagesChanged;

		/// <summary>
		/// Updates list of packages and triggers <see cref="TemplatePackagesChanged"/> event.
		/// </summary>
		/// <param name="nupkgs">List of "*.nupkg" files.</param>
		public void UpdatePackages (IEnumerable<string> nupkgs = null)
		{
			this.nupkgs.Clear ();
			if (nupkgs != null) {
				this.nupkgs.AddRange (nupkgs);
			}
			TemplatePackagesChanged?.Invoke ();
		}

		public void AddPackage (string nupkg)
		{
			nupkgs.Add (nupkg);
			TemplatePackagesChanged?.Invoke ();
		}

		public Task<IReadOnlyList<ITemplatePackage>> GetAllTemplatePackagesAsync (CancellationToken cancellationToken)
		{
			var list = new List<ITemplatePackage> ();
			foreach (string nupkg in nupkgs) {
				list.Add (new TemplatePackage (this, nupkg, environmentSettings.Host.FileSystem.GetLastWriteTimeUtc (nupkg)));
			}
			return Task.FromResult<IReadOnlyList<ITemplatePackage>> (list);
		}
	}
}


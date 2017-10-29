#!/bin/sh

mono ./packages/ILRepack.2.0.13/tools/ILRepack.exe \
  /targetPlatform:v4 \
  /lib:../lib \
  /out:../lib/merged/Microsoft.TemplateEngine.Merged.dll \
  ../lib/Microsoft.TemplateEngine.Abstractions.dll \
  ../lib/Microsoft.TemplateEngine.Edge.dll \
  ../lib/Microsoft.TemplateEngine.Core.Contracts.dll \
  ../lib/Microsoft.TemplateEngine.Orchestrator.RunnableProjects.dll \
  ../lib/Microsoft.TemplateEngine.Core.dll \
  ../lib/Microsoft.TemplateEngine.Utils.dll
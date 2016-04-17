namespace DNS_query
{
    using Bdev.Net.Dns;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    internal static class Program
    {
        private static IPAddress dnsserveraddress;
        static string root;

        private static int Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Three arguments required - max-parallel-request, path, dns-ip and domain list");
                return -1;
            }
            try
            {
                int maxParallel = int.Parse(args[0]);
                root = args[1];
                dnsserveraddress = IPAddress.Parse(args[2]);
                string[] domains = args[3].Split(',').Select(d => d.Trim()).ToArray();
                var results = Check(maxParallel, domains);
                var sorted = results.OrderBy(r => r.Index).ToArray();
                WriteResults(sorted);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Buuuuu " + exception.Message);
            }
#if DEBUG
            Console.WriteLine("Ended");
            Console.ReadLine();
#endif
            return 0;
        }

        private static void WriteResults(ICollection<Result> results)
        {
            string successFile = "success.txt";
            string errorFile = "error.txt";
            File.WriteAllText(successFile,
                string.Join(Environment.NewLine, results.Where(r => !r.IsError).Select(r => r.Content)));
            File.WriteAllText(errorFile,
                string.Join(Environment.NewLine, results.Where(r => r.IsError).Select(r => r.Content)));
        }

        private static BlockingCollection<Result> Check(int maxParallel, string[] domains)
        {
            DateTime now = DateTime.Now;
            BlockingCollection<Result> results = new BlockingCollection<Result>();
            var allDomains = File.ReadAllLines(Path.Combine(root, "out.txt"));
            ParallelOptions options;
            if (maxParallel>0)
            {
                options = new ParallelOptions { MaxDegreeOfParallelism = maxParallel };
            }
            else
            {
                options = new ParallelOptions();
            }
            Parallel.ForEach(Partitioner.Create(0, allDomains.Length), options,
                (range) =>
                {
                    for (int i=range.Item1; i<range.Item2;i++)
                    {
                        results.Add(CheckDomain(i, allDomains[i], domains));
                    }
                });
            return results;
        }


    private static Result CheckDomain(int index, string domain, string[] domains)
    {
        Request request = new Request();
        request.AddQuestion(new Question(domain, DnsType.SOA, DnsClass.IN));
        var source = from a in Bdev.Net.Dns.Resolver.Lookup(request, dnsserveraddress).Answers
                     let record = (SoaRecord)a.Record
                     select record.PrimaryNameServer;
        var domainName = source.SingleOrDefault();
        if (!string.IsNullOrEmpty(domainName))
        {
            if (domains.Any(d => string.Equals(d, domainName, StringComparison.OrdinalIgnoreCase)))
            {
                string text = $"{index:000} OK ({domainName}) {domain}";
                return new Result { Index = index, IsError = false, Content = text };
            }
            else
            {
                string text = $"{index:000} Error (nothosted) ({domainName}) {domain}";
                OutputError(text);
                return new Result { Index = index, IsError = true, Content = text };
            }
        }
        else
        {
            string text = $"{index:000} Error (nonexistent) ({domainName}) {domain}";
            OutputError(text);
            return new Result { Index = index, IsError = true, Content = text };
        }
    }

    private static void OutputError(string content)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(content);
        Console.ForegroundColor = oldColor;
    }

    private static void Append(string target, string content, bool isError)
    {
        using (StreamWriter writer = File.AppendText(Path.Combine(root, target)))
        {
            writer.WriteLine(content);
        }
    }
}

public class Result
{
    public int Index;
    public bool IsError;
    public string Content;
}
}


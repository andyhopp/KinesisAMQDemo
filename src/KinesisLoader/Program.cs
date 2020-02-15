using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;

namespace LoadKinesis
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var kpl = new AmazonKinesisClient();
            var request = new PutRecordsRequest
            {
                StreamName = "KinesisDemo-CFn-DataStream-X087RH1MQG6N", //"KinesisDemo-DataStream-16FJ43Y3JTEX7",
                Records = new List<PutRecordsRequestEntry>()
            };
            var rng = new Random();

            Console.WriteLine("Loading Messages!\nPress ESC to stop.\n");
            do
            {
                var iteration = 0L;
                while (!Console.KeyAvailable)
                {
                    Console.Write("{0}        \r", iteration);
                    var record = new MemoryStream();

                    using (var writer = new StreamWriter(record))
                    {
                        writer.WriteLine($"{{ \"Key\": \"{iteration}\", \"VehicleId\":\"{rng.Next(1, 10000)}\" \"RPM\":\"{rng.Next(0, 8000)}\" }}");
                        writer.Flush();
                        record.Position = 0;
                        request.Records.Add(
                            new PutRecordsRequestEntry
                            {
                                PartitionKey = iteration.ToString(),
                                Data = record
                            }
                        );
                        await kpl.PutRecordsAsync(
                            request
                        );
                    }
                    iteration++;
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            return 0;
        }
    }
}

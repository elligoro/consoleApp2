using Grpc.Net.Client;
using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.Services
{
    public class GreeterClient 
    {
        public GreeterClient()
        {

        }

        public async Task<string> Greet()
        {
            var msg = new HelloRequest { Name = "iliya" };
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(msg);
            return reply.Message;
        }

    }
}

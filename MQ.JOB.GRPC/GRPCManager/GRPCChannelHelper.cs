using Grpc.Core;
using HZP.MQ.AdminManager;

namespace MQ.JOB.GRPC.GRPCManager
{
    public class GRPCChannelHelper
    {
        private static readonly Channel channel = new Channel("localhost:40001", ChannelCredentials.Insecure);

        private static readonly gRPC.gRPCClient client = new gRPC.gRPCClient(channel);

        public static TestReply SayHello(TestRequest request)
        {
            return client.SayHello(request);
        }
    }
}

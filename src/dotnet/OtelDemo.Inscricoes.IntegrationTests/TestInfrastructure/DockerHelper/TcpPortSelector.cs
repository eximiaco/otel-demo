using System.Net;
using System.Net.Sockets;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public class TcpPortSelector
{
    public static int GetFreePort()
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, 0);
        tcpListener.Start();
        var port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
        tcpListener.Stop();
        return port;
    }
}

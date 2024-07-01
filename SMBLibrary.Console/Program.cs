using System;
using System.Net;
using System.Threading;
using SMBLibrary;
using SMBLibrary.Adapters;
using SMBLibrary.Authentication.GSSAPI;
using SMBLibrary.Authentication.NTLM;
using SMBLibrary.Client;
using SMBLibrary.Server;
using SMBLibrary.Win32;

namespace SMBServe;

public static class Program
{
    public static void Main()
    {
        Run();
    }

    public static void Run()
    {
        IGSSMechanism gssMechanism = new IndependentNTLMAuthenticationProvider((username) =>
        {
            return username == "Guest" ? "" : null;
        });
        GSSProvider gssProvider = new GSSProvider(gssMechanism);

        ushort port = 11445;

        //port = 139;
        var transport = SMBTransportType.DirectTCPTransport;
        var address = (Address: IPAddress.Parse("192.168.1.101"), Port: port);
        address = (Address: IPAddress.Parse("127.0.0.10"), Port: port);
        address = (address.Address, port);

        address.Address = IPAddress.Parse("10.0.0.47");
        address.Address = IPAddress.Loopback;
        address.Address = IPAddress.Parse("192.168.176.1");
        address.Address = IPAddress.Parse("192.168.176.1");
        address.Address = IPAddress.Parse("10.0.3.33");
        //address.Address = IPAddress.Parse("6ca9:2520:8efe:4b42:bb51:6574:274d:cc73");
        address.Port = 11445;
        ushort? overridePort = 11445;
        overridePort = null;
        IPAddress serverAddress = null;
        serverAddress = IPAddress.Any;

        //address = (IPAddress.Loopback, (int)server.OverridePort.Value);

        var server = new SMBServer(new SMBShareCollection()
        {
#if WINDOWS
            new FileSystemShare("Code", new NTDirectoryFileSystem(@"C:\Code"))
#endif
        },
        gssProvider)
        {
            OverridePort = overridePort ?? address.Port
        };

        Console.WriteLine($"Starting SMB Server on {server.OverridePort}");

        server.Start(serverAddress ?? address.Address, transport);
        //server.Start(IPAddress.Loopback, SMBTransportType.DirectTCPTransport);

        Console.WriteLine($"Started SMB Server. Press enter to exit...");

        Thread.Sleep(1000);

        var client = new SMB2Client();
        bool result = client.Connect(address.Address, transport, port: address.Port);

        var loginResult = client.Login("", "Guest", "");

        var shares = client.ListShares(out var status);


        Console.ReadLine();

    }
}
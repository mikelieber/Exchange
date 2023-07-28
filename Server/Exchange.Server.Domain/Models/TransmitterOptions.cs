using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace Exchange.Server.Domain.Models;

[ExcludeFromCodeCoverage]
public sealed class TransmitterOptions
{
    public const string Section = "TransmitterOptions";
    public Dictionary<string, string> MulticastGroups { get; set; }

    public IEnumerable<KeyValuePair<string, IPEndPoint>> MulticastGroupsAddresses
    {
        get
        {
            foreach (var group in MulticastGroups)
                yield return new KeyValuePair<string, IPEndPoint>(group.Key, IPEndPoint.Parse(group.Value));
        }
    }

    public string LocalAddress { get; set; }

    public IPAddress LocalIPAddress =>
        string.IsNullOrEmpty(LocalAddress) ? IPAddress.Any : IPAddress.Parse(LocalAddress);

    public string LocalPort { get; set; }
    public IPEndPoint LocalEndpoint => new(LocalIPAddress, int.Parse(LocalPort));
    public bool IsExclusiveAddress { get; set; }

    public SocketOptionName OptionName =>
        IsExclusiveAddress ? SocketOptionName.ExclusiveAddressUse : SocketOptionName.ReuseAddress;
}
using System.Net;
using System.Net.Sockets;

namespace Server.Services.Transmitter;

internal sealed class TransmitterOptions
{
    internal const string Section = "TransmitterOptions";
    public Dictionary<string, string> MulticastGroups { get; set; }
    public IEnumerable<KeyValuePair<string, IPEndPoint>> MulticastGroupsAddresses
    {
        get
        {
            foreach (var group in MulticastGroups)
                yield return new(group.Key, IPEndPoint.Parse(group.Value));
        }
    }
    public string LocalAddress { get; set; }
    public IPAddress LocalIPAddress { get => string.IsNullOrEmpty(LocalAddress) ? IPAddress.Any : IPAddress.Parse(LocalAddress); }
    public string LocalPort { get; set; }
    public IPEndPoint LocalEndpoint { get => new IPEndPoint(LocalIPAddress, int.Parse(LocalPort)); }
    public bool IsExclusiveAddress { get; set; }
    public SocketOptionName OptionName { get => IsExclusiveAddress ? SocketOptionName.ExclusiveAddressUse : SocketOptionName.ReuseAddress; }
}
﻿using System.Net;
using System.Net.Sockets;

namespace Client;

internal sealed class ReceiverOptions
{
    internal const string Section = "ReceiverOptions";
    public string[] MulticastGroups { get; set; }

    public IEnumerable<IPAddress> MulticastGroupsAddresses
    {
        get
        {
            foreach (var group in MulticastGroups)
                yield return IPAddress.Parse(group);
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
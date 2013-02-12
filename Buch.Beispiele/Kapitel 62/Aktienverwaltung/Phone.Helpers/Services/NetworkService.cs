using System;
using System.Net.NetworkInformation;
using Phone.Helpers.Interfaces;

namespace Phone.Helpers.Services
{
  public class NetworkService : INetworkService
  {
    public bool IsAvailable { get; set; }

    public NetworkService()
    {
      CheckNetworkConnection();

      NetworkChange.NetworkAddressChanged
          += (s,e) => CheckNetworkConnection();
    }

    private void NetworkAddressChanged(object sender, EventArgs e)
    {
      CheckNetworkConnection();
    }

    private void CheckNetworkConnection()
    {
      IsAvailable = NetworkInterface.GetIsNetworkAvailable();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phone.Helpers.Interfaces
{
    public interface INetworkService
    {
        bool IsAvailable { get; set; }
    }
}

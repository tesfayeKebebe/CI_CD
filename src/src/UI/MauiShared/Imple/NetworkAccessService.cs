using MauiShared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiShared.Imple
{
    public class NetworkAccessService : INetworkAcessService
    {
        public bool GetNetWorkAccess()
        {
            NetworkAccess connect = Connectivity.Current.NetworkAccess;
            if (connect != NetworkAccess.Internet)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

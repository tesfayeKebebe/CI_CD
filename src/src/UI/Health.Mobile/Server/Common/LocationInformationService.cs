using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Mobile.Server.Common
{
    public class LocationInformationService
    {
        private CancellationTokenSource _cancellationToken  = null!;
        public async Task<Location?> CheckAndRequestLocationPermission()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium,TimeSpan.FromSeconds(5));
                _cancellationToken = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(request, _cancellationToken.Token);
                if (location != null)
                    return location;
                else
                    return null;

            }
            catch (Exception e )
            {

                throw new Exception(e.Message);
            }


        }

    }
}

namespace Application.Business;

public static class DistanceCalculatorService
{
    public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
    {
        var  newLat1 = Math.PI*lat1/180;
        var newLat2 = Math.PI*lat2/180;
        var theta = lon1 - lon2;
        var newTheta = Math.PI*theta/180;
        var dist =
            Math.Sin(newLat1)*Math.Sin(newLat2) + Math.Cos(newLat1)*
            Math.Cos(newLat2)*Math.Cos(newTheta);
        dist = Math.Acos(dist);
        dist = dist*180/Math.PI;
        dist = dist*60*1.1515*1.609344;
        return dist;
    }
}
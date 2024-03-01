using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BingMapsRESTService.Common.JSON;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace WpfMapControls
{
    public static class RouteProvider
    {
        
        public static LocationCollection GetLocations(string key, List<BusStop> locacations, out double distance)
        {
            
            LocationCollection locs = new LocationCollection();
             string query =

                     "https://dev.virtualearth.net/REST/V1/Routes/Driving?travelMode=Walking&optimizeWaypoints=false&optmz=time&routeAttributes=routePath&key="+key;
             for(int i=0; i<locacations.Count; i++)
             {
                if (i == 0 || i == locacations.Count-1) {
                    query += "&wp." + i + "=" + (string)locacations[i].location;
                    continue;
                }
                if (i <= 9)
                {
                    query += "&vwp." + i + "=" + (string)locacations[i].location;
                    continue;
                }
                query += "&wp."+i+"=" + (string)locacations[i].location;
             }
             
            //string query = "https://dev.virtualearth.net/REST/V1/Routes/Driving?optmz=time&tl=0.1&routeAttributes=routePath&key=Au1qF3-RmEKTs2aAiGvmGvpDVBcg-KD5tp1pdbRUM4qGMvBdMAHccJgFOyddKMVr&wp.0=43.22094625406707,27.88060220662592&wp.1=43.21175608968687,27.9069032199717";
            Uri geocodeRequest = new Uri(query);
            double temp=0;
            GetResponse(geocodeRequest, (x) =>
            {
                
                Console.ReadLine();
                if (x.ResourceSets[0].Resources.Length == 0) return;
                BingMapsRESTService.Common.JSON.Route? route = x.ResourceSets[0].Resources[0] as BingMapsRESTService.Common.JSON.Route;

                temp = route.TravelDistance;

                double[][] routePath = route.RoutePath.Line.Coordinates;
                for (int i = 0; i < routePath.Length; i++)
                {
                    if (routePath[i].Length >= 2)
                    {
                        locs.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0], routePath[i][1]));
                    }
                }

            });
            distance = temp;
            return locs;
        }

        private static void GetResponse(Uri uri, Action<Response> callback)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (o, a) =>
            {
                if (callback != null)
                {
                    
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                    callback(ser.ReadObject(a.Result) as Response);
                }
            };
            wc.OpenReadAsync(uri);
        }

    }


}

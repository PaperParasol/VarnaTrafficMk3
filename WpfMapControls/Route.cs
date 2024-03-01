using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMapControls
{
    internal class Route
    {
        public static List<BusStop> GetLocationsList(Dictionary<int, List<BusStop>> busStopList, Location A, Location B, out int liniqOut)
        {
            var results = new List<BusStop>();

            // Tozi RETURN vrushta gotoviq spisuk s marshruta
            var start = new HashSet<int>();
            var finish = new HashSet<int>();
            var reject = new HashSet<string>();

            var l = new List<string>();
            int number = 0;
            string stopA = "";
            string stopB = "";

            int i;
            bool b = false;

            for (i = 0; ; i += 2)
            {
                l.Add(NearestPoint(busStopList, A, reject));
                l.Add(NearestPoint(busStopList, B, reject));
                reject.Add(l[i]);
                reject.Add(l[i + 1]);

                foreach (var line in busStopList)
                {
                    foreach (var stop in line.Value)
                    {
                        if (stop.Label == l[i]) start.Add(line.Key);
                        if (stop.Label == l[i + 1]) finish.Add(line.Key);
                    }
                }

                foreach (int x in start)
                {
                    if (finish.Contains(x))
                    {
                        number = x;
                        b = true;
                        break;
                    }
                }
                if (b == true) break;
            }

            for (int j = 0; j < i + 2; j += 2)
            {
                if (busStopList[number].Any(x => x.Label == l[j]))
                {
                    for (int t = 1; t < i + 2; t += 2)
                    {
                        if (busStopList[number].Any(x => x.Label == l[t]))
                        {
                            stopA = l[j];
                            stopB = l[t];
                            break;
                        }
                    }
                }
            }

            //results.Add(stopA);
            //results.Add(stopB);

            int inda = busStopList[number].FindIndex(x => x.Label.Equals(stopA));
            int indb = busStopList[number].FindIndex(x => x.Label.Equals(stopB));

            //results.Add(inda.ToString());
            //results.Add(indb.ToString());

            if (inda > indb)
                for (int j = inda; j >= indb; j--) results.Add(busStopList[number][j]);
            else
                for (int j = inda; j <= indb; j++) results.Add(busStopList[number][j]);

            liniqOut = number;
            return results;
        }

        public static string NearestPoint(Dictionary<int, List<BusStop>> busStopList, Location A, HashSet<string> h)
        {
            int lineNum;
            string label = "";
            double mn = 100;
            double dist, distLat, distLong;

            foreach (var line in busStopList)
            {
                foreach (var stop in line.Value)
                {
                    if (!h.Contains(stop.Label))
                    {
                        distLong = A.Longitude - stop.location.Longitute;
                        distLat = A.Latitude - stop.location.Latitude;
                        dist = Math.Sqrt(distLat * distLat + distLong * distLong);
                        if (dist < mn)
                        {
                            mn = dist;
                            label = stop.Label;
                        }
                    }
                }
            }

            return label;
        }
    }
}


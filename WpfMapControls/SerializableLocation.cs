using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMapControls
{
    [Serializable]
    public class SerializableLocation
    {
        public double Latitude { get; set; }
        public double Longitute { get; set; }

        //public Location LocationData { get; set; }
        public SerializableLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitute = longitude;
            //LocationData = new Location(Latitude, Longitute);
        }
        
        public static explicit operator SerializableLocation(Location v) => new SerializableLocation(v.Latitude, v.Longitude);
        
        public static explicit operator Location(SerializableLocation v) => new Location(v.Latitude,v.Longitute);

        public static explicit operator string(SerializableLocation v) => $"{v.Latitude},{v.Longitute}";
    }
}

using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMapControls
{
    [Serializable]
    public class BusStop
    {
        
        public SerializableLocation location { get; set; }
        
        public string ProgramName { get; set; }

        public string Label { get; set; }

        public BusStop(SerializableLocation location, string programName, string label)
        {
            
            this.location = location;
            ProgramName = programName;
            Label = label;
        }

        public static explicit operator string(BusStop a) => new string(a.Label);
    }
}

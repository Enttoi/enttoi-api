using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class StatsSensor
    {
        public StatsSensorStates States { get; set; }

        public int sensorId { get; set; }
    }

    /// <summary>
    /// Each key represents a state type and value is the 
    /// time in ms the state remained in
    /// </summary>
    public class StatsSensorStates : Dictionary<int, long>
    {
    }
}

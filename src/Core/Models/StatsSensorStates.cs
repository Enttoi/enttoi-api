using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Each key represents a state type and value is the 
    /// time in ms the state remained in
    /// </summary>
    public class StatsSensorStates : Dictionary<int, long>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Examples.Callouts
{
    public class Common
    {
        public static Random gRandom = new Random();

        public enum CallResponseType
        {
            Code_2 = 2,
            Code_3 = 3
        }
        
        public enum CalloutState
        {
            Cancelled, Created, Dispatched, UnitResponding, AtScene, Completed
        }
        
        public enum PedType
        {
            Unknown, Witness, Victim, Suspect
        }
    }
}

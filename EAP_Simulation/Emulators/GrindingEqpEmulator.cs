using EAP_Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP_Simulation.Emulators
{
   public class GrindingEqpEmulator : EqpEmulator
    {
        public GrindingEqpEmulator(string eqpId):base(eqpId, nameof(EqpType.Grinding))
        {
        }


        public override KeyValuePair<string, string> GenError()
        {
            List<KeyValuePair<string, string>> errores = new List<KeyValuePair<string, string>> {
               new KeyValuePair<string, string>( "E000", "Mount wheel error"),
               new KeyValuePair<string, string>( "E001", "No wheel")
            };
            return errores[this._Random.Next(0, errores.Count)];
        }


    }
}

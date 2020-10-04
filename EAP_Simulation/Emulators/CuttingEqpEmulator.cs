using EAP_Library.DTO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace EAP_Simulation.Emulators
{
    public class CuttingEqpEmulator : EqpEmulator
    {
        public CuttingEqpEmulator(string eqpId) : base(eqpId, nameof(EqpType.Cutting))
        {
        }


        public override KeyValuePair<string, string> GenError()
        {
            List<KeyValuePair<string, string>> errores = new List<KeyValuePair<string, string>> {
               new KeyValuePair<string, string>( "E000", "Insufficient blade length"),
               new KeyValuePair<string, string>( "E001", "No blade")
            };
            return errores[this._Random.Next(0, errores.Count)];
        }

    }
}

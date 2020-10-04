using EAP_Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP_Simulation.Emulators
{
  public  class TapeEqpEmulator : EqpEmulator
    {
        public TapeEqpEmulator(string eqpId) : base(eqpId, nameof(EqpType.Tape))
        {

        }

        public override KeyValuePair<string, string> GenError()
        {
            List<KeyValuePair<string, string>> errores = new List<KeyValuePair<string, string>> {
               new KeyValuePair<string, string>( "E000", "Out of tape"),
               new KeyValuePair<string, string>( "E001", "No tape")
            };
            return errores[this._Random.Next(0, errores.Count)];
        }





    }
}

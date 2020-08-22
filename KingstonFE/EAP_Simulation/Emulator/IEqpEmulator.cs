using EAP_Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAP_Simulation.Emulator
{
   public interface IEqpEmulator
    {

        public EqpInfo GetLatestEqpInfo();

    }
}

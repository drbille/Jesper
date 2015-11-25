using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarPerformanceSimulator
{
    interface Car
    {
        int[] torqueCurve
        {
            get;
        }
        double[] gearRatios
        {
            get;
        } 

    }
}

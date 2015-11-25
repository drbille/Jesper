using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarPerformanceSimulator
{
    class _2015FordFusion : Car
    {
        public int[] torqueCurve
        {
            get
            {
                int[] _torqueCurve = new int[35];
                _torqueCurve[0] = 112;
                _torqueCurve[1] = 137;
                _torqueCurve[2] = 162;
                _torqueCurve[3] = 175;
                _torqueCurve[4] = 188;
                _torqueCurve[5] = 200;
                _torqueCurve[6] = 206;
                _torqueCurve[7] = 212;
                _torqueCurve[8] = 218;
                _torqueCurve[9] = 222;
                _torqueCurve[10] = 225;
                _torqueCurve[11] = 228;
                _torqueCurve[12] = 232;
                _torqueCurve[13] = 235;
                _torqueCurve[14] = 237;
                _torqueCurve[15] = 235;
                _torqueCurve[16] = 232;
                _torqueCurve[17] = 230;
                _torqueCurve[18] = 226;
                _torqueCurve[19] = 214;
                _torqueCurve[20] = 208;
                _torqueCurve[21] = 198;
                _torqueCurve[22] = 180;
                for (int i = 22; i < 35; i++)
                {
                    _torqueCurve[i] = -1;
                }
                return _torqueCurve;
            }
        }

        public double[] gearRatios
        {
            get
            {
                double[] _gearRatios = new double[9];
                _gearRatios[0] = -4.58;
                _gearRatios[1] = 4.58;
                _gearRatios[2] = 2.96;
                _gearRatios[3] = 1.91;
                _gearRatios[4] = 1.45;
                _gearRatios[5] = 1.00;
                _gearRatios[6] = 0.75;
                _gearRatios[7] = 3.07;
                _gearRatios[8] = 0;
                return _gearRatios;
            }
        } 
    }
}

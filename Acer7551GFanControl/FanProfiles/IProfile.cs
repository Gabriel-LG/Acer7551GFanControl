using System;
using System.Collections.Generic;
using System.Text;

namespace Acer7551GFanControl
{
    interface IProfile
    {
        int intervalMs {get;}
        string name { get;}
        float CalcFanSpeed(float temperature);
    }
}

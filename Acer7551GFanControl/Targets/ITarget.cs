using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acer7551GFanControl.Targets
{
    interface ITarget
    {
        float GetTemperature();
        void SetBiosControl(Boolean b);
        bool GetBiosControl();        
        void SetFanSpeed(float fanSpeed);
        float GetFanSpeed();
        string Information { get; }
    }
}

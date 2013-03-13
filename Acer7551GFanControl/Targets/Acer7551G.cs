using Acer7551GFanControl.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Acer7551GFanControl
{
    class Acer7551G : EmbeddedController, ITarget
    {
        private float fanSpeed = 0;
        private int cpuTemp = 0;
        private int gpuTemp = 0;
        private bool biosControl = true;

        public float GetTemperature()
        {
            cpuTemp = ReadEC(0xA8);
            gpuTemp = ReadEC(0xAF);
            return (float)Math.Max(cpuTemp, gpuTemp);
        }

        public void SetFanSpeed(float speed)
        {
            if (speed > 100) speed = 100;
            if (speed < 0) speed = 0;
            byte value = (byte)(255 - (speed * 135) / 100);
            WriteEC(0x94, value);
            fanSpeed = speed;
        }

        public float GetFanSpeed()
        {
            return fanSpeed;
        }

        public void SetBiosControl(Boolean enable)
        {
            if (enable) WriteEC(0x93, 0x04);
            else WriteEC(0x93, 0x14);
            biosControl = enable;
        }

        public bool GetBiosControl()
        {
            return biosControl;
        }

        public string Information
        {
            get
            {
                return String.Format("CPU:{0:0}°C, GPU:{1:0}°C, Fan:{2:0}", cpuTemp, gpuTemp, biosControl ? "BIOS" : String.Format("{0:0}%", fanSpeed));
            }
        }

    }
}

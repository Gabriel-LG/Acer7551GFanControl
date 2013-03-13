using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acer7551GFanControl.FanProfiles
{
    class Linear: IProfile
    {
        //points[temperature] =  fanspeed;
        Dictionary<float, float> points = new Dictionary<float,float>();

        private int _intervalMs;
        private string _name;

        public Linear(XmlNode config)
        {
            _name = config.SelectSingleNode("name").InnerText;
            _intervalMs = int.Parse(config.SelectSingleNode("interval").InnerText);
            if (intervalMs > 2000 || intervalMs < 100) throw new Exception(String.Format("{0}: invalid interval (must be 100~2000ms)", name, intervalMs)); 
            XmlNodeList cfgPoints = config.SelectNodes("point");
            foreach(XmlNode cfgPoint in cfgPoints)
            {
                int temperature = int.Parse(cfgPoint.Attributes["temp"].Value);
                if (temperature < 0  || temperature > 99) throw new Exception(String.Format("{0}: invalid temperature (must be 0~99)", name, temperature));
                int fanspeed = int.Parse(cfgPoint.Attributes["fan"].Value);
                if (fanspeed < 0 || fanspeed > 100) throw new Exception(String.Format("{0}: invalid fanspeed", name, fanspeed));
                if (!points.ContainsKey(0)) points[0] = fanspeed;
                points[temperature] = fanspeed;
                points[100] = fanspeed;
            }
        }


        #region IProfile Members


        public int intervalMs
        {
            get { return _intervalMs; }
        }

        public string name
        {
            get { return _name; }
        }

        public float CalcFanSpeed(float temperature)
        {
            float lTemp = 0;
            float hTemp = 100;

            foreach (float pTemp in points.Keys)
            {
                if (pTemp <= temperature && pTemp > lTemp ) lTemp = pTemp;
                if (pTemp >= temperature && pTemp < hTemp) hTemp = pTemp;
            }
            if (lTemp == hTemp) return points[lTemp];

            float slope = (points[hTemp]-points[lTemp])/(hTemp - lTemp);
            return (points[lTemp] + slope * (temperature - lTemp));
        }

        #endregion
    }
}

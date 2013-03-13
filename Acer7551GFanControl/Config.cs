using System;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using Acer7551GFanControl.FanProfiles;
using System.Collections;
using System.Collections.Generic;

namespace Acer7551GFanControl
{
    class Config
    {
        private List<IProfile> _profiles = new List<IProfile>();
        private IProfile _defaultProfile = null;

        public Config(String path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //profiles
            foreach (XmlNode profile in doc.SelectNodes("//fanspeed/profile"))
            {
                switch (profile.Attributes["type"].Value)
                {
                    case "linear":
                        profiles.Add(new Linear(profile));
                        break;
                    default:
                        throw new Exception(String.Format("Profile type: {0} not supported", profile.Attributes["type"].Value));
                }
                //default profile
                if (profile.Attributes["default"] != null && profile.Attributes["default"].Value.ToLower() == "true") _defaultProfile = profiles[profiles.Count - 1];
            }
        }

        public List<IProfile> profiles
        {
            get { return _profiles; }
        }

        public IProfile defaultProfile
        {
            get { return _defaultProfile; }
        }
    }
}

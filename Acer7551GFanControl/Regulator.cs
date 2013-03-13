using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Acer7551GFanControl.Targets;
using System.Windows.Forms;

namespace Acer7551GFanControl
{
    class Regulator
    {
        public delegate void RegulatorUpdateHandler(Regulator regulator, float temperature, float fanSpeed, string text);
        public event RegulatorUpdateHandler UpdateEvent;
        public delegate void RegulatorStoppedHandler(Regulator regulator, Exception e);
        public event RegulatorStoppedHandler RegulatorStopped;

        private Boolean running;
        private IProfile _profile;
        private Thread thread;
        public Regulator()
        {
            running = true;
            thread = new Thread(new ThreadStart(loop));
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        public IProfile profile
        {
            get { return _profile; }
            set {
                _profile = value;
            }
        }

        private void loop()
        {
            ITarget target = new Acer7551G();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                long next = 0;

                while (running)
                {
                    try
                    {
                        float temperature = target.GetTemperature();
                        float fanSpeed = 0;
                        if (profile != null)
                        {
                            fanSpeed = profile.CalcFanSpeed(temperature);
                            target.SetBiosControl(false);
                            target.SetFanSpeed(fanSpeed);
                            if (UpdateEvent != null) UpdateEvent(this, temperature, fanSpeed, profile.name+"\n"+target.Information);
                        }
                        else
                        {
                            target.SetBiosControl(true);
                            if (UpdateEvent != null) UpdateEvent(this, temperature, fanSpeed, "BIOS\n"+target.Information);
                        }
                    }
                    catch (TimeoutException e)
                    {
                        System.Console.WriteLine(e.Message);
                    }
                    //System.Console.WriteLine("time: {0} ms", oStopWatch.ElapsedMilliseconds);
                    long now = stopwatch.ElapsedMilliseconds;
                    int interval = profile != null ? profile.intervalMs : 1000;
                    next += interval;
                    if (now > next)
                    {
                        System.Console.WriteLine("skipped {0} cycles", (now - next) / interval);
                        next = now;
                    }

                    Thread.Sleep((int)(next - now));
                }
            }
            catch (Exception e)
            {
                try
                {
                    target.SetBiosControl(true);
                }
                finally
                {
                    MessageBox.Show(e.ToString(), "Acer7551G Fan Control", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                target.SetBiosControl(true);
            }
        }
    }
}

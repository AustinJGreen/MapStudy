using System;
using System.Threading;

namespace MapStudy
{
    public class Clock : IDisposable
    {
        private bool running;
        private int ticks;
        private int tickTimeMs;
        private Timer timer;

        public int Elapsed => ticks * tickTimeMs;
        public int TickTime => tickTimeMs;
        public bool Running => running;

        public event EventHandler OnTick;

        public void Start()
        {
            if (timer != null && tickTimeMs != 0)
            {
                running = true;
                timer.Change(0, tickTimeMs);
            }
        }

        public void Start(int time)
        {
            tickTimeMs = time;
            Start();
        }

        public void Stop()
        {
            if (timer != null)
            {
                running = false;
                timer.Change(-1, tickTimeMs);
            }
        }

        public void Reset()
        {
            ticks = 0;
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public Clock()
        {
            tickTimeMs = 0;
            timer = new Timer(OnTickHandler);
            running = false;
        }

        public Clock(int tickTimeMs)
        {
            this.tickTimeMs = tickTimeMs;
            timer = new Timer(OnTickHandler);
            running = false;
        }

        private void OnTickHandler(object state)
        {
            if (OnTick != null)
            {
                ticks++;
                OnTick(this, EventArgs.Empty);
            }
        }
    }
}

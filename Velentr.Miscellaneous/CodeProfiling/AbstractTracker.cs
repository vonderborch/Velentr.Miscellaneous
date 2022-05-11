using System;

namespace Velentr.Miscellaneous.CodeProfiling
{
    public abstract class AbstractTracker
    {
        public int MaximumSamples { get; protected set; }

        protected AbstractTracker(int maximumSamples)
        {
            MaximumSamples = maximumSamples;
        }

        public abstract void Update(TimeSpan timeSpan);

        public abstract void StartTracking();

        public abstract void StopTracking();
    }
}

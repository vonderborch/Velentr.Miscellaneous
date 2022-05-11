using System;
using System.Collections.Generic;
using System.Linq;

namespace Velentr.Miscellaneous.CodeProfiling
{
    public class FpsTracker : AbstractTracker
    {
        public long TotalFrames { get; private set; }
        public double TotalSeconds { get; private set; }
        public double AverageFramesPerSecond { get; private set; }
        public double CurrentFramesPerSecond { get; private set; }

        private Queue<double> _samples;

        public FpsTracker(int maximumSamples = 100) : base(maximumSamples)
        {
            _samples = new Queue<double>(maximumSamples);
        }

        public override void Update(TimeSpan timeSpan)
        {
            CurrentFramesPerSecond = 1.0f / timeSpan.TotalSeconds;

            _samples.Enqueue(CurrentFramesPerSecond);

            if (_samples.Count > MaximumSamples)
            {
                _samples.Dequeue();
                AverageFramesPerSecond = _samples.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += timeSpan.TotalSeconds;
        }

        public override void StartTracking()
        {
            throw new NotImplementedException();
        }

        public override void StopTracking()
        {
            throw new NotImplementedException();
        }
    }
}

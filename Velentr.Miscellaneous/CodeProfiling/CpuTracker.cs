using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Velentr.Math;

namespace Velentr.Miscellaneous.CodeProfiling
{
    public class CpuTracker : AbstractTracker
    {
        public double CpuPercent { get; private set; }

        private PerformanceCounter _cpuTracker;

        private Queue<double> _samples;

        private Task _updateThread;

        private readonly int _sleepTime;

        private CancellationTokenSource _cancellationToken;

        public bool ContinueTrackingForever = false;

        public CpuTracker(int maximumSamples = 100, bool automaticallyStartTracking = false, int sleepTime = 1000) : base(maximumSamples)
        {
            _cpuTracker = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName, true);
            _samples = new Queue<double>(maximumSamples);
            _sleepTime = MathHelpers.Clamp(sleepTime, 1000);
            ContinueTrackingForever = automaticallyStartTracking;
            _cancellationToken = new CancellationTokenSource();
            _updateThread = new Task(UpdateInternal, _cancellationToken.Token);
            if (automaticallyStartTracking)
            {
                _updateThread.Start();
            }
        }

        public override void Update(TimeSpan timeSpan)
        {
            if (_updateThread.IsCompleted)
            {
                _updateThread = new Task(UpdateInternal, _cancellationToken.Token);
                _updateThread.Start();
            }
            else if (_updateThread.Status == TaskStatus.Created)
            {
                _updateThread.Start();
            }
        }

        public override void StartTracking()
        {
            if (_updateThread.Status != TaskStatus.Running && _updateThread.Status != TaskStatus.WaitingForActivation && _updateThread.Status != TaskStatus.WaitingForChildrenToComplete && _updateThread.Status != TaskStatus.WaitingToRun)
            {
                if (_updateThread.IsCanceled)
                {
                    _cancellationToken = new CancellationTokenSource();
                }

                _updateThread = new Task(UpdateInternal, _cancellationToken.Token);
                _updateThread.Start();
            }
        }

        public override void StopTracking()
        {
            _cancellationToken.Cancel();
        }

        private void UpdateInternal()
        {
            do
            {
                _cpuTracker.NextValue();
                Thread.Sleep(_sleepTime); // wait a second to get a valid reading
                _samples.Enqueue(_cpuTracker.NextValue() / (double) Environment.ProcessorCount);

                while (_samples.Count > MaximumSamples)
                {
                    _samples.Dequeue();
                }

                CpuPercent = _samples.Sum() / _samples.Count;
            } while (ContinueTrackingForever);
        }
    }
}

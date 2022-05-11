using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Velentr.Math.ByteConversion;

namespace Velentr.Miscellaneous.CodeProfiling
{
    public class MemoryTracker : AbstractTracker
    {
        public long MemoryUsageBytes { get; private set; }
        public double MemoryUsageKb => MemoryUsageBytes.ToSizeUnit(SizeUnits.KB);
        public double MemoryUsageMb => MemoryUsageBytes.ToSizeUnit(SizeUnits.MB);
        public double MemoryUsageGb => MemoryUsageBytes.ToSizeUnit(SizeUnits.GB);

        private Queue<long> _samples;

        private readonly Process _process;

        private readonly int _sleepTime;

        private Task _updateThread;

        private CancellationTokenSource _cancellationToken;

        public bool ContinueTrackingForever = false;

        public MemoryTracker(int maximumSamples = 100, bool automaticallyStartTracking = false, int sleepTime = 1000) : base(maximumSamples)
        {
            _process = Process.GetCurrentProcess();
            _sleepTime = sleepTime;
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
                _samples.Enqueue(_process.WorkingSet64);
                while (_samples.Count > MaximumSamples)
                {
                    _samples.Dequeue();
                }
                MemoryUsageBytes = _samples.Sum() / _samples.Count;
                Thread.Sleep(_sleepTime);
            } while (ContinueTrackingForever) ;
        }
    }
}

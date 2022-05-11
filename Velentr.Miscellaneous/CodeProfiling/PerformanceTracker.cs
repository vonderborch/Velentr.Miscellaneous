using System;

namespace Velentr.Miscellaneous.CodeProfiling
{
    public class PerformanceTracker : AbstractTracker
    {
        public MemoryTracker MemoryTracker;
        public CpuTracker CpuTracker;
        public FpsTracker FpsTracker;

        public PerformanceTracker(int maximumSamples = 100, bool enableCpuTracker = true, int cpuSleepTime = 1000, bool automaticallyEnableCpuTracking = false, bool enableMemoryTracker = true, bool automaticallyEnableMemoryTracking = false, int memorySleepTime = 1000, bool enableFpsTracker = false) : base(maximumSamples)
        {
            if (enableCpuTracker)
            {
                CpuTracker = new CpuTracker(maximumSamples, automaticallyEnableCpuTracking, cpuSleepTime);
            }
            if (enableMemoryTracker)
            {
                MemoryTracker = new MemoryTracker(maximumSamples, automaticallyEnableMemoryTracking, memorySleepTime);
            }
            if (enableFpsTracker)
            {
                FpsTracker = new FpsTracker(maximumSamples);
            }
        }

        public override void Update(TimeSpan timeSpan)
        {
            MemoryTracker?.Update(timeSpan);
            CpuTracker?.Update(timeSpan);
            FpsTracker?.Update(timeSpan);
        }

        public override void StartTracking()
        {
            MemoryTracker?.StartTracking();
            CpuTracker?.StartTracking();
        }

        public override void StopTracking()
        {
            MemoryTracker?.StopTracking();
            CpuTracker?.StopTracking();
        }
    }
}

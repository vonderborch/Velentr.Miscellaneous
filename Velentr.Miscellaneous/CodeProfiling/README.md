# Velentr.Miscellaneous.CodeProfiling
Miscellaneous helpers for code profiling

# Usage Note
The MemoryTracker (and the MemoryTracker when used with the PerformanceTracker) require the following additional Nuget: https://www.nuget.org/packages/System.Diagnostics.PerformanceCounter/6.0.1

# Current Systems/Helpers
Class | Description | Min Supported Version
----- | ----------- | ---------------------
AbstractTracker | An abstract performance tracker object. | 1.2.0
CpuTracker | A CPU Usage tracker object. | 1.2.0
FpsTracker | An FPS tracker object. | 1.2.0
MemoryTracker | A Memory Usage tracker object. | 1.2.0
PerformanceTracker | An all-in-one performance tracker object (can track CPU, FPS, and Memory). | 1.2.0

# CpuTracker Usage
```
// Initialize the object
var tracker = new CpuTracker();

// Update the object (should be done every tick/update interval in your program. Can also run independently)
tracker.Update(gameTime.ElapsedGameTime);
// if the tracker is initialized such as `var tracker = new CpuTracker(automaticallyStartTracking: true);`, the above is not needed!

var currentUsage = tracker.CpuPercent;
```

# FpsTracker Usage
```
// Initialize the object
var tracker = new FpsTracker();

// Update the object (should be done every tick/update interval in your program)
tracker.Update(gameTime.ElapsedGameTime);

var currentFps = tracker.AverageFramesPerSecond;
```

# MemoryTracker Usage
```
// Initialize the object
var tracker = new MemoryTracker();

// Update the object (should be done every tick/update interval in your program. Can also run independently)
tracker.Update(gameTime.ElapsedGameTime);
// if the tracker is initialized such as `var tracker = new MemoryTracker(automaticallyStartTracking: true);`, the above is not needed!

var currentUsage = tracker.MemoryUsageKb;
```

# PerformanceTracker Usage
```
// Initialize the object
var tracker = new PerformanceTracker();

// Update the object (should be done every tick/update interval in your program. Can also run independently)
tracker.Update(gameTime.ElapsedGameTime);
// if the tracker is initialized such as `var tracker = new PerformanceTracker(automaticallyEnableCpuTracking: true, automaticallyEnableMemoryTracking: true);`, the above is not needed!

var currentCpuUsage = tracker.CpuTracker.CpuPercent;
var currentMemoryUsage = tracker.MemoryTracker.MemoryUsageKb;
```

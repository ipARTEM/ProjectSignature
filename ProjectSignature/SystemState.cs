

using System.Diagnostics;

namespace ProjectSignature
{
    static class SystemState
    {
        static public PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        static public PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        static public bool IsValid(long partSize, int threadsCounter)
        {
            bool ok;

            cpuCounter.NextValue();


            if ((ramCounter.NextValue() < (((partSize / 1000000) * 4) * threadsCounter)) || cpuCounter.NextValue() > 80)
            {
                return ok = false;
            }
            else return ok = true;

        }
    }
}

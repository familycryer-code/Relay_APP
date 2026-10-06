using System;
using System.Diagnostics;
using System.Threading;

namespace RelayControl
{
    public partial class MainControl
    {
        internal bool WaitUntil(
            Func<bool> condition,
            TimeSpan timeout,
            int pollMs = 50,
            string reason = null,
            string caller = null)
        {
            var sw = Stopwatch.StartNew();

            if (reason == null)
                reason = "unspecified";

            if (caller == null)
                caller = "unknown";

            logger.Info("WAIT START caller={0}, reason={1}, timeoutMs={2}, pollMs={3}",
                caller, reason, (int)timeout.TotalMilliseconds, pollMs);

            while (sw.Elapsed < timeout)
            {
                try
                {
                    if (condition())
                    {
                        logger.Info("WAIT OK caller={0}, reason={1}, elapsedMs={2}",
                            caller, reason, sw.ElapsedMilliseconds);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "WAIT condition threw caller={0}, reason={1}", caller, reason);
                }

                Thread.Sleep(pollMs);
            }

            logger.Warn("WAIT TIMEOUT caller={0}, reason={1}, elapsedMs={2}",
                caller, reason, sw.ElapsedMilliseconds);
            return false;
        }

        internal void DelayWithLog(
            int delayMs,
            string reason,
            string caller = null)
        {
            if (caller == null)
                caller = "unknown";

            logger.Info("DELAY caller={0}, reason={1}, delayMs={2}",
                caller, reason, delayMs);

            Thread.Sleep(delayMs);
        }
    }
}
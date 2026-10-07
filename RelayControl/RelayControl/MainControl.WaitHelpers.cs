using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RelayControl
{
    public partial class MainControl
    {
        // ------------------------------------------------------------
        // Sync versions: keep for worker/background-thread use only.
        // These should not be used on the UI thread for long-running waits.
        // ------------------------------------------------------------

        internal bool WaitUntil(
            Func<bool> condition,
            TimeSpan timeout,
            int pollMs = 50,
            string reason = null,
            string caller = null)
        {
            var sw = Stopwatch.StartNew();
            if (IsUiThread())
            {
                logger.Warn("SYNC WAIT ON UI THREAD caller={0}, reason={1}", caller ?? "unknown", reason ?? "unspecified");
            }

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
            if (IsUiThread())
            {
                logger.Warn("SYNC WAIT ON UI THREAD caller={0}, reason={1}", caller ?? "unknown", reason ?? "unspecified");
            }

            Thread.Sleep(delayMs);
        }

        // ------------------------------------------------------------
        // Async versions: use in UI-thread call paths such as Apply All,
        // startup sequencing, modal decision flow, and any method that
        // should not freeze the main window.
        // ------------------------------------------------------------

        internal async Task<bool> WaitUntilAsync(
            Func<bool> condition,
            TimeSpan timeout,
            int pollMs = 50,
            string reason = null,
            string caller = null,
            CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();

            if (reason == null)
                reason = "unspecified";

            if (caller == null)
                caller = "unknown";

            logger.Info("WAIT START async caller={0}, reason={1}, timeoutMs={2}, pollMs={3}",
                caller, reason, (int)timeout.TotalMilliseconds, pollMs);

            while (sw.Elapsed < timeout)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    if (condition())
                    {
                        logger.Info("WAIT OK async caller={0}, reason={1}, elapsedMs={2}",
                            caller, reason, sw.ElapsedMilliseconds);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex, "WAIT condition threw async caller={0}, reason={1}", caller, reason);
                }

                await Task.Delay(pollMs, ct).ConfigureAwait(true);
            }

            logger.Warn("WAIT TIMEOUT async caller={0}, reason={1}, elapsedMs={2}",
                caller, reason, sw.ElapsedMilliseconds);
            return false;
        }

        internal async Task DelayWithLogAsync(
            int delayMs,
            string reason,
            string caller = null,
            CancellationToken ct = default)
        {
            if (caller == null)
                caller = "unknown";

            logger.Info("DELAY async caller={0}, reason={1}, delayMs={2}",
                caller, reason, delayMs);

            await Task.Delay(delayMs, ct).ConfigureAwait(true);
        }

        // ------------------------------------------------------------
        // Optional: helper to avoid accidental UI-thread blocking calls
        // ------------------------------------------------------------

        internal bool IsUiThread()
        {
            return SynchronizationContext.Current != null;
        }
    }
}
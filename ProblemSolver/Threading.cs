using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProblemSolver
{
    class CAutoMutex : IDisposable {
        private Mutex mut;
        bool disposed = false;
        public CAutoMutex(Mutex m)
        {
            mut = m;
            m.WaitOne();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                mut.ReleaseMutex();
            }
            disposed = true;
        }
    }

    class CAutoScopeGuard
    {
        public delegate void CleanupMethod();
        private CleanupMethod cleanupMethod;
        bool disposed = false;
        public CAutoScopeGuard(CleanupMethod cleanup)
        {
            cleanupMethod = cleanup;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                cleanupMethod();
            }
            disposed = true;
        }
    }

    class ThreadingHelper
    {
        public delegate object SelfContainedMethod();
        public delegate void SuccessCallback(long ElapsedMilliseconds, object output);
        public delegate void AbortCallback(long ElapsedMilliseconds);

        private Mutex mutex = new Mutex();
        private Thread ExecutionThread;
        public void RunMethodAsync(SelfContainedMethod method, SuccessCallback successCallback, AbortCallback abortCallback) {
            ThreadPool.QueueUserWorkItem(delegate
            {
                ExecutionThread = Thread.CurrentThread;

                // Register kill thread
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        Thread.Sleep(2000);
                        using (CAutoMutex autoMutex = new CAutoMutex(mutex))
                        {
                            if (null != ExecutionThread)
                            {
                                ExecutionThread.Abort();
                                ExecutionThread.Join();
                            }
                        }
                    });

                    object output = method();
                    sw.Stop();

                    using (CAutoMutex autoMutex = new CAutoMutex(mutex))
                    {
                        successCallback(sw.ElapsedMilliseconds, output);
                        ExecutionThread = null;
                    }
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                    abortCallback(sw.ElapsedMilliseconds);
                }
            });
        }
    }
}

﻿
using CheckPCBSystem.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckPCBSystem
{
    public class App : AutoGeneratedSingleton<App>
    {
        private SynchronizationContext m_SyncContext = null;
        public SynchronizationContext SyncContext
        {
            get { return m_SyncContext; }
        }

        public void InitSyncContext(SynchronizationContext contexnt)
        {
            m_SyncContext = contexnt;
        }
    }
}
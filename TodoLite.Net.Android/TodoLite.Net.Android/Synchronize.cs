using System;

using Couchbase.Lite;

namespace TodoLite.Net.Android
{
    public class Synchronize
    {
        private Replication m_pullReplication;
        private Replication m_pushReplication;

        public Synchronize(Database database, string syncUrl, bool isContinuousPull)
        {
            if (m_pullReplication == null && m_pushReplication == null) {
                Uri replicationEndpoint;
                try {
                    replicationEndpoint = new Uri(syncUrl);
                } catch (Exception e) {
                    throw new ArgumentException("Invalid endpoint for replication");
                }

                m_pullReplication = database.CreatePullReplication(replicationEndpoint);
                if (isContinuousPull) {
                    m_pullReplication.Continuous = true;
                }

                m_pushReplication = database.CreatePushReplication(replicationEndpoint);
                m_pushReplication.Continuous = true;
            }
        }

        public void DestroyReplications()
        {
            m_pullReplication.Stop();
            m_pullReplication.DeleteCookie("SyncGatewaySession");
            m_pullReplication = null;

            m_pushReplication.Stop();
            m_pushReplication.DeleteCookie("SyncGatewaySession");
            m_pushReplication = null;
        }

        public void Start()
        {
            m_pullReplication.Start();
            m_pushReplication.Start();
        }
    }
}


using System;
using Android.App;

using Couchbase.Lite;
using Java.Security;

namespace TodoLite.Net.Android
{
    public enum AuthenticationType
    {
        Facebook
    }

    public class App : Application
    {
        public static string Tag = "TodoLite";
        public static string DatabaseName = "todos";
        public static string GuestDatabaseName = "guest";
        public static string SyncUrlHttp = "localhost:4984";
        // TODO public static string SyncUrlHttps = "localhost:4984";
        public static string SyncUrl = SyncUrlHttp;

        private Manager m_manager;
        private Database m_database;
        private Synchronize m_synchronize;

        private void InitDatabase()
        {
            try {
                m_manager = new Manager();
            } catch (CouchbaseLiteException cble) {
                throw new ApplicationException("Could not instantiate Couchbase Lite manager");
            }
        }

        public void SetDatabaseForName(string name)
        {
            MessageDigest digest = null;
            try {
                digest = MessageDigest.GetInstance("MD5");
            } catch (NoSuchAlgorithmException e) {
                e.PrintStackTrace();
            }
            try {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(name);
                byte[] hashBytes = digest.Digest(inputBytes);
                var options = new DatabaseOptions();
                options.Create = true;
                m_database = m_manager.OpenDatabase("db-" + BitConverter.ToString(hashBytes), options);
            } catch (CouchbaseLiteException e) {
                throw new ApplicationException("Could not create database");
            }
        }

        public void StartReplicationWithAuthentication(AuthenticationType authenticationType)
        {
            if (authenticationType == AuthenticationType.Facebook) {
                if (m_synchronize != null) {
                    m_synchronize.DestroyReplications();
                }

                m_synchronize = new Synchronize(m_database, SyncUrl, true);
                // TODO m_synchronize.SetAuthentication(token);
                // TODO Set up change listeners
                m_synchronize.Start();
            } else {
                throw new InvalidParameterException("Applicaiton does not support this authentication type");
            }   
        }

    }
}


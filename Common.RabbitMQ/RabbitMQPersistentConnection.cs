using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{
    public class RabbitMQPersistentConnection
    {
        readonly object sync_root = new object();
        readonly IConnectionFactory connectionFactory;
        IConnection connection;
        bool disposed;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.disposed = false;
        }

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.IsOpen && !disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return connection.CreateModel();
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            try
            {
                connection.Dispose();
            }
            catch (IOException ex)
            {
                //log ex
            }
        }

        public bool TryConnect()
        {
            //log "RabbitMQ Client is trying to connect"

            lock (sync_root)
            {

                connection = connectionFactory.CreateConnection();

                if (IsConnected)
                {
                    connection.ConnectionShutdown += OnConnectionShutdown;
                    connection.CallbackException += OnCallbackException;
                    connection.ConnectionBlocked += OnConnectionBlocked;

                    //log $"RabbitMQ persistent connection acquired a connection {connection.Endpoint.HostName} and is subscribed to failure events" 

                    return true;
                }
                else
                {
                    //log "FATAL ERROR: RabbitMQ connections could not be created and opened"

                    return false;
                }
            }
        }

        void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (disposed) return;

            //log "A RabbitMQ connection is shutdown. Trying to re-connect..."

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (disposed) return;

            //log "A RabbitMQ connection throw exception. Trying to re-connect..."

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (disposed) return;

            //log "A RabbitMQ connection is on shutdown. Trying to re-connect..."

            TryConnect();
        }
    }
}

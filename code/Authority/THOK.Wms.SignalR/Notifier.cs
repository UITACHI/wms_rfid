using SignalR;

namespace THOK.Wms.SignalR
{
    public class Notifier<TPersistentConnection> where TPersistentConnection : PersistentConnection
    {
        protected string ConnectionId = "";

        public void Notify(object message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<TPersistentConnection>();
            context.Connection.Broadcast(message);
        }

        public void NotifyGroup(string group, object message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<TPersistentConnection>();
            context.Groups.Send(group, message);
        }

        public void NotifyConnection(string connectionId, object message)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<TPersistentConnection>();
            context.Connection.Send(connectionId, message);
        }

        public void NotifyConnection(object message)
        {
            try
            {
                var context = GlobalHost.ConnectionManager.GetConnectionContext<TPersistentConnection>();
                context.Connection.Send(ConnectionId, message);
            }
            catch (System.Exception)
            {
            }
        }
    }
}

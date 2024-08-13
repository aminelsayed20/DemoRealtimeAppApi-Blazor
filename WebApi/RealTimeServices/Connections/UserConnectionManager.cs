namespace WebApi.RealTimeServices.Connections
{
    public class UserConnectionManager
    {
        private readonly Dictionary<string, List<string>> _connections = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);


        // Add a connection ID for a specific user
        public void AddConnection(string username, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.ContainsKey(username))
                {
                    _connections[username] = new List<string>();
                }
                _connections[username].Add(connectionId);
            }
        }

        // Remove a connection ID and return the username if the connection ID is found
        public string? RemoveConnection(string connectionId)
        {
            lock (_connections)
            {
                foreach (var kvp in _connections)
                {
                    if (kvp.Value.Contains(connectionId))
                    {
                        kvp.Value.Remove(connectionId);
                        if (kvp.Value.Count == 0)
                        {
                            _connections.Remove(kvp.Key);
                        }
                        return kvp.Key;
                    }
                }
                return null;
            }
        }

        // Get all connection IDs for a specific user
        public List<string> GetConnectionIds(string username)
        {
            lock (_connections)
            {
                return _connections.TryGetValue(username, out var connectionIds) ? connectionIds : new List<string>();
            }
        }

    }
}

using CanvasSignalR.Models;
using System.Collections.Concurrent;

namespace CanvasSignalR.Services
{
    public class CanvasService
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _canvasUsers = new();
        private readonly ConcurrentDictionary<string, string> _userByCanvas = new();
        private readonly ConcurrentDictionary<string, List<LineCommand>> _canvasByLineCommands = new();

        public void AddUserToCanvas(string canvasName, string connectionId) 
        {
            _canvasUsers.AddOrUpdate(
               canvasName,
               new HashSet<string> { connectionId },
               (key, existing) => { existing.Add(connectionId); return existing; }
           );

            _userByCanvas.AddOrUpdate(
                connectionId,
                canvasName,
                (key, existing) => canvasName);
        }
        public void RemoveUserFromCanvas(string canvasName, string connectionId)
        {
            if (_canvasUsers.TryGetValue(canvasName, out var users))
            {
                users.Remove(connectionId);
                if(users.Count == 0) _canvasUsers.TryRemove(canvasName, out _);
            }

            _userByCanvas.TryRemove(connectionId, out _);
        }
        public List<string> GetCanvases() => [.. _canvasUsers.Keys];

        public void AddLineCommandToCanvas(string canvasName, LineCommand lineCommand)
        {
            _canvasByLineCommands.AddOrUpdate(
                canvasName,
                [lineCommand],
                (key, existing) => { existing.Add(lineCommand); return existing; }
            );
        }

        public List<LineCommand> GetLineCommandsForCanvas(string canvasName)
        {
            if (_canvasByLineCommands.TryGetValue(canvasName, out var lineCommands))
            {
                return lineCommands;
            }
            return [];
        }
    }
}

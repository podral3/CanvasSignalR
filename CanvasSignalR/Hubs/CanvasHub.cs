using CanvasSignalR.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CanvasSignalR.Hubs
{
    public class CanvasHub(CanvasService canvasService) : Hub
    {
        private readonly CanvasService _canvasService = canvasService;

        public async Task JoinCanvas(string canvasName)
        {
            _canvasService.AddUserToCanvas(canvasName, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, canvasName);
        }
        public async Task LeaveCanvas(string canvasName)
        {
            _canvasService.RemoveUserFromCanvas(canvasName, Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, canvasName);
        }

        public async Task SendLine(string canvasName, int x1, int y1, int x2, int y2, int r, int g, int b, int penSize)
        {
            await Clients.OthersInGroup(canvasName).SendAsync("ReceiveCanvasUpdate", x1, y1, x2, y2, r, g, b, penSize);
        }

        public async Task<List<string>> GetCanvases()
        {
            return await Task.FromResult(_canvasService.GetCanvases());
        }

    }
}

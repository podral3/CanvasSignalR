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

        public async Task SendLine(string canvasName, int x1, int y1, int x2, int y2, string color, int penSize)
        {
            _canvasService.AddLineCommandToCanvas(canvasName, new Models.LineCommand
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Color = color,
                PenSize = penSize
            });
            await Clients.OthersInGroup(canvasName).SendAsync("ReceiveCanvasUpdate", x1, y1, x2, y2, color, penSize);
        }

        public async Task<List<string>> GetCanvases()
        {
            return await Task.FromResult(_canvasService.GetCanvases());
        }

        public async Task<List<Models.LineCommand>> GetCanvasCommands(string canvasName)
        {
            return await Task.FromResult(_canvasService.GetLineCommandsForCanvas(canvasName));
        }

    }
}

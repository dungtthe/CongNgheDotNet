using Microsoft.AspNetCore.SignalR;

namespace SignalR.GroupChat.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> UserGroups = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> UserNames = new Dictionary<string, string>();


        //người dùng vào 1 group theo tên group
        public async Task JoinGroup(string groupName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            UserGroups[Context.ConnectionId] = groupName;
            UserNames[Context.ConnectionId] = username; 

            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{username} has joined the group {groupName}.");
        }


        //người dùng rời group theo tên group
        public async Task LeaveGroup(string groupName)
        {
            if (UserNames.TryGetValue(Context.ConnectionId, out string? username))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                UserGroups.Remove(Context.ConnectionId);
                UserNames.Remove(Context.ConnectionId);

                await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{username} has left the group {groupName}.");
            }
        }



        //người dùng gửi msg tới group của mình
        public async Task SendMessageToGroup(string groupName, string message)
        {
            if (UserGroups.TryGetValue(Context.ConnectionId, out string? currentGroup) && currentGroup == groupName)
            {
                string username = UserNames[Context.ConnectionId];
                await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{username}: {message}");
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "You are not a member of this group.");
            }
        }

    }


}

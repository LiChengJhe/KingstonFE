using EAP_Library.DTO;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Hubs
{
    public class DashboardHub : Hub<IDashboardHubClient>
    {
        private IServiceProvider _Provider;

        public DashboardHub(IServiceProvider provide)
        {
            this._Provider = provide;
        }

        public async Task<bool> JoinGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) throw new Exception("GroupId is null");
            else
            {
                // await ReplyAllEqpInfo(this.Context.ConnectionId, groupId);
                // await Clients.Client(this.Context.ConnectionId).ReplyTest(0);
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
            }
            return true;
        }

        public async Task<bool> ExitGroup(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) throw new Exception("GroupId is null");
            else await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupId);
            return true;
        }


        public async Task ReplyAllEqpInfo(string connId, string groupId)
        {
            // to query db or redis
            await Clients.Client(connId).ReplyEqpInfo(new List<EqpInfoDTO>());
        }

      
    }
    public interface IDashboardHubClient
    {
        Task ReplyEqpInfo(List<EqpInfoDTO> eqps);
        Task ReplyTest(int i);
    }
}

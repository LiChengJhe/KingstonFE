using EAP_Library.DTO;
using EAP_Library.Configs;
using EAP_WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using EAP_WebAPI.Repositories;
using AutoMapper;
using EAP_WebAPI.Entities;
using EAP_WebAPI.Configs;
using System.Reactive.Concurrency;

namespace EAP_WebAPI.HostedServices
{
    public class DashboardHostedService : IHostedService, IDisposable
    {
        private IHubContext<DashboardHub, IDashboardHubClient> _Hub;
        private QueueHelper _QueueHelper;
        private IOptions<MQConfig> _MQConfig;
        private EqpCachedRepository _CachedRepository;
        public DashboardHostedService(IHubContext<DashboardHub, IDashboardHubClient> hub, IOptions<MQConfig> options, EqpCachedRepository cachedRepository)
        {
            this._Hub = hub;
            this._MQConfig = options;
            this._QueueHelper = new QueueHelper(this._MQConfig.Value.Dashboard);
            this._CachedRepository = cachedRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            this._QueueHelper.ReceiveMessages<EqpInfoDTO>(async (eqp) =>
            {
                try
                {
                    await this._Hub.Clients.Group("group-1").ReplyEqpInfo(new List<EqpInfoDTO> { eqp });
                    await this._CachedRepository.Add(eqp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 偵測多餘的呼叫

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置 Managed 狀態 (Managed 物件)。
                }

                // TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下方的完成項。
                // TODO: 將大型欄位設為 null。
                this._QueueHelper?.Dispose();
                this._QueueHelper = null;
                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
        ~DashboardHostedService()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 加入這個程式碼的目的在正確實作可處置的模式。
        public void Dispose()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

using AutoMapper;
using EAP_Library.Configs;
using EAP_Library.DTO;
using EAP_WebAPI.Configs;
using EAP_WebAPI.Entities;
using EAP_WebAPI.Hubs;
using EAP_WebAPI.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace EAP_WebAPI.HostedServices
{
    public class RepositoryHostedService : IHostedService, IDisposable
    {
        private bool disposedValue;
        private QueueHelper _QueueHelper;
        private IOptions<MQConfig> _MQConfig;
        private UnitOfWork _UnitOfWork;
        private IMapper _Mapper;
        public RepositoryHostedService( IMapper mapper, IOptions<MQConfig> options, UnitOfWork unitOfWork)
        {
            this._Mapper = mapper;
            this._MQConfig = options;
            this._UnitOfWork = unitOfWork;
            this._QueueHelper = new QueueHelper(this._MQConfig.Value.Repository);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._QueueHelper.ReceiveMessages<EqpInfoDTO>((eqp) =>
            {

                try
                {

                    SaveToRepositories(this._UnitOfWork, eqp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

               
            });

            return Task.CompletedTask;
        }
        public void SaveToRepositories(UnitOfWork unitOfWork,EqpInfoDTO eqp)
        {
            SaveToInfoRepository(unitOfWork.EqpInfoRepo, eqp);
            SaveToStatusRepository(unitOfWork.EqpStatusRepo, eqp.Id, eqp.StatusInfo);
            SaveToAlarmRepository(unitOfWork.EqpAlarmRepo, eqp.Id, eqp.AlarmInfo);
            SaveToWipRepository(unitOfWork.EqpWipRepo, eqp.Id, eqp.CassetteInfo);
            unitOfWork.SaveChangesAsync();
        }
        private void SaveToInfoRepository(EqpInfoRepository repository, EqpInfoDTO eqp)
        {
            if (!repository.IsExist(eqp.Id))
            {
                repository.Add(this._Mapper.Map<EqpInfo>(eqp));
            }
        }
        private void SaveToStatusRepository(EqpStatusRepository repository, string eqpId, EqpStatusInfoDTO statusDto)
        {
            EqpStatus status = this._Mapper.Map<EqpStatus>(statusDto);
            status.EqpId = eqpId;
            repository.Add(status);
        }

        private void SaveToAlarmRepository(EqpAlarmRepository repository, string eqpId, EqpAlarmInfoDTO alarmDto)
        {
            if (alarmDto != null)
            {

                EqpAlarm alarm = new EqpAlarm
                {
                    EqpId = eqpId,
                    AlarmId = alarmDto.Id,
                    AlarmText = alarmDto.Text,
                    AlarmType = alarmDto.Type
                };


                if (alarmDto.Type == nameof(EqpAlarmType.ON_GOING))
                {
                    alarm.AlarmTime = alarmDto.TimeFrom;
                }
                else
                {
                    alarm.AlarmTime = alarmDto.TimeTo.Value;
                }
                repository.Add(alarm);
            }


        }

        private void SaveToWipRepository(EqpWipRepository repository, string eqpId, CassetteInfoDTO cassetteInfoDto)
        {

            if (cassetteInfoDto != null && cassetteInfoDto.EventName == nameof(BaseEventType.CassetteIn))
            {
                List<EqpWip> wips = new List<EqpWip>();
                cassetteInfoDto.Lots.ForEach(lot =>
                {
                    lot.Wafers.ForEach(wafer =>
                    {
                        wips.Add(new EqpWip { EqpId = eqpId, Lot = lot.Id, WaferId = wafer.Id, Time = cassetteInfoDto.EventTime });
                    });
                });

                repository.Add(wips);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                this._QueueHelper?.Dispose();
                this._QueueHelper = null;

                this._UnitOfWork?.Dispose();
                this._UnitOfWork = null;
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~RepositoryHostedService()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        
    }
}

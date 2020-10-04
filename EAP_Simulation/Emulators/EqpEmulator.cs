using EAP_Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP_Simulation.Emulators
{
   public abstract class EqpEmulator
    {
        public EqpInfoDTO _Eqp;
        public Random _Random = new Random();

        public EqpEmulator(string eqpId,string type)
        {
            this._Eqp = new EqpInfoDTO
            {
                Id = eqpId,
                 Type= type
            };
        }
       

        public virtual EqpAlarmInfoDTO GetNewAlarmInfo()
        {
      
            bool isNull = this._Eqp.AlarmInfo == null;
            if (isNull)
            {
                KeyValuePair<string, string> error= GenError();
                return new EqpAlarmInfoDTO
                {
                    Id = error.Key,
                    Type = nameof(EqpAlarmType.ON_GOING),
                    Text = error.Value,
                    TimeFrom = DateTime.Now,
                    TimeTo = new Nullable<DateTime>()
                };
            }
            else
            {
                bool isOngoing = this._Eqp.AlarmInfo.Type == nameof(EqpAlarmType.ON_GOING);
                return new EqpAlarmInfoDTO
                {
                    Id = this._Eqp.AlarmInfo.Id,
                    Type = isOngoing ? nameof(EqpAlarmType.CLEAR) : nameof(EqpAlarmType.ON_GOING),
                    Text = this._Eqp.AlarmInfo.Text,
                    TimeFrom = isOngoing ? this._Eqp.AlarmInfo.TimeFrom : DateTime.Now,
                    TimeTo = isOngoing ? DateTime.Now : new Nullable<DateTime>()
                };
            }
        }

        public virtual KeyValuePair<string, string> GenError()
        {
            List<KeyValuePair<string, string>> errores = new List<KeyValuePair<string, string>> {
               new KeyValuePair<string, string>( "E000", "Insufficient blade length"),
               new KeyValuePair<string, string>( "E001", "No blade")
            };
            return errores[this._Random.Next(0, errores.Count)];
        }

        public virtual CassetteInfoDTO GetNewCassetteInfo()
        {

            List<WaferInfoDTO> waferIds = new List<WaferInfoDTO>();
            for (int i = 1; i <= 25; i++)
            {
                waferIds.Add(new WaferInfoDTO { Id = i.ToString("00") });
            }
            List<LotInfoDTO> lots = new List<LotInfoDTO>();
            string lotId = GetNewId();
            lots.Add(new LotInfoDTO
            {

                Id = $"{lotId}.{0}",
                Wafers = waferIds

            });

            return new CassetteInfoDTO
            {
                Id = GetNewId(),
                Lots = lots,
                EventName = nameof(BaseEventType.CassetteIn),
                EventTime = DateTime.Now
            };

        }

        public virtual EqpStatusInfoDTO GetNewStatus()
        {
            bool isNull = this._Eqp.StatusInfo == null;
            EqpStatusType status = (EqpStatusType)this._Random.Next(0, 9);
            return new EqpStatusInfoDTO
            {

                CurType = status.ToString(),
                CurTime = DateTime.Now,
                PrevType = isNull ? null : this._Eqp.StatusInfo.CurType,
                PrevTime = isNull ? new Nullable<DateTime>() : this._Eqp.StatusInfo.CurTime
            };
        }

        public virtual string GetNewId()
        {
            List<char> chars = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper().OrderByDescending(o => o).ToList();
            string result = string.Empty;
            foreach (var i in chars)
            {
                result += i;
            }
            return result;
        }

        public virtual EqpInfoDTO GetNewEqpInfo()
        {
            EqpStatusInfoDTO status = null;
            EqpAlarmInfoDTO alarm = this._Eqp.AlarmInfo;
            CassetteInfoDTO cassette = this._Eqp.CassetteInfo;
            status = GetNewStatus();
            switch (status.CurType)
            {
                case nameof(EqpStatusType.DOWN):
                    alarm = GetNewAlarmInfo();

                    break;
                case nameof(EqpStatusType.PM):
                case nameof(EqpStatusType.OFF):
                case nameof(EqpStatusType.IDLE):
                    cassette = null;
                    break;
                case nameof(EqpStatusType.HOLD):
                    break;
                default:
                    if (this._Eqp.CassetteInfo != null)
                    {
                        if (this._Random.Next(0, 10) == 0)
                        {
                            cassette = GetNewCassetteInfo();
                        }
                    }
                    else
                    {
                        cassette = GetNewCassetteInfo();
                    }

                    if (this._Eqp.AlarmInfo != null && this._Eqp.AlarmInfo.Type == nameof(EqpAlarmType.ON_GOING))
                    {
                        alarm = GetNewAlarmInfo();
                    }
                    break;
            }

            if (this._Eqp.CassetteInfo != null && this._Eqp.CassetteInfo == cassette)
            {
                cassette.EventName = nameof(BaseEventType.Unknow);
                cassette.EventTime = DateTime.Now;
            }

            this._Eqp.StatusInfo = status;
            this._Eqp.CassetteInfo = cassette;
            this._Eqp.AlarmInfo = alarm;

            return this._Eqp;
        }
    }
}

using EAP_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace EAP_Simulation.Emulator
{
    public class CutterEqpEmulator : IEqpEmulator
    {
        private EqpInfo _Eqp;
        public CutterEqpEmulator(string eqpId)
        {
            this._Eqp = new EqpInfo
            {
                EqpId = eqpId,
                EqpType = EqpType.Cutting
            };
        }
        public EqpInfo GetLatestEqpInfo()
        {

            StatusInfo status = null;
            AlarmInfo alarm = null;
            CassetteInfo cassette = null;
            status = GetLatestStatus();
            switch (status.CurStatusType)
            {
                case StatusType.DOWN:
                    alarm = GetLatestAlarmInfo();
                    break;
                case StatusType.PM:
                case StatusType.OFF:
                    cassette = null;
                    break;
                default:
                    cassette = GetLatestCassetteInfo();
                    if (this._Eqp.AlarmInfo != null && this._Eqp.AlarmInfo.AlarmType == AlarmType.ON_GOING)
                    {
                        alarm = GetLatestAlarmInfo();
                    }
                    break;
            }

            this._Eqp = new EqpInfo
            {
                EqpId = this._Eqp.EqpId,
                EqpType = this._Eqp.EqpType,
                EqpStatus = status,
                CassetteInfo = cassette,
                AlarmInfo = alarm,
                Timestamp = DateTime.Now
            };

            return this._Eqp;

        }

        private AlarmInfo GetLatestAlarmInfo()
        {
            string id = "E000";
            string text = "刀具刃長不足";
            bool isNull = this._Eqp.AlarmInfo == null;
            if (isNull)
            {
                return new AlarmInfo
                {
                    AlarmId = id,
                    AlarmType = AlarmType.ON_GOING,
                    AlarmText = text,
                    AlarmTimeFrom = DateTime.Now,
                    AlarmTimeTo = new Nullable<DateTime>()
                };
            }
            else
            {
                bool isOngoing = this._Eqp.AlarmInfo.AlarmType == AlarmType.ON_GOING;
                return new AlarmInfo
                {
                    AlarmId = id,
                    AlarmType = isOngoing ? AlarmType.CLEAR : AlarmType.ON_GOING,
                    AlarmText = text,
                    AlarmTimeFrom = isOngoing ? this._Eqp.AlarmInfo.AlarmTimeFrom : DateTime.Now,
                    AlarmTimeTo = isOngoing ? DateTime.Now : new Nullable<DateTime>()
                };
            }

        }

        private CassetteInfo GetLatestCassetteInfo()
        {

            List<int> waferIds = new List<int>();
            for (int i = 1; i <= 25; i++)
            {
                waferIds.Add(i);
            }
            List<LotInfo> lots = new List<LotInfo>();
            string lotId = GetNewLotId();
            lots.Add(new LotInfo
            {
                LotId = $"{lotId}.{0}",
                WaferIds = waferIds

            });

            return new CassetteInfo
            {
                Lots = lots,
                Timestamp = DateTime.Now
            };

        }

        private StatusInfo GetLatestStatus()
        {
            bool isNull = this._Eqp.EqpStatus == null;
            return new StatusInfo
            {
                CurStatusType = (StatusType)new Random().Next(0, 8),
                CurStatusTimestamp = DateTime.Now,
                PrevStatusType = isNull ? new Nullable<StatusType>() : this._Eqp.EqpStatus.CurStatusType,
                PrevStatusTimestamp = isNull ? new Nullable<DateTime>() : this._Eqp.EqpStatus.CurStatusTimestamp
            };
        }

        private string GetNewLotId()
        {
            List<char> chars = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper().OrderByDescending(o => o).ToList();
            string result = string.Empty;
            foreach (var i in chars)
            {
                result+= i;
            }
            return result;
        }

     
    }
}

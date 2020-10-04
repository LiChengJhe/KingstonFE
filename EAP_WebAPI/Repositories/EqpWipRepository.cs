using EAP_Library.DTO;
using EAP_WebAPI.Contexts;
using EAP_WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Repositories
{
    public class EqpWipRepository : IDisposable
    {
        private KingstonContext _Context;
        private bool disposedValue;

        public EqpWipRepository(KingstonContext context)
        {
            this._Context = context;
        }

        public void Add(EqpWip eqpWip)
        {
            this._Context.EqpWip.Add(eqpWip);
        }
        public void Add(List<EqpWip> eqpWips)
        {
            this._Context.EqpWip.AddRange(eqpWips);
        }
        public IEnumerable<EqpWip> Get(string eqpId)
        {
            return this._Context.EqpWip.Where(o => o.EqpId == eqpId);
        }

        public IEnumerable<EqpWip> GetAll()
        {
            return this._Context.EqpWip.ToList();
        }

        public IEnumerable<EqpWipDailyStatistics> GetDailyStatistics(string eqpId)
        {

            return this._Context.EqpWip
              .Where(o => o.EqpId == eqpId)
             .ToLookup(p => new DateTime(p.Time.Year, p.Time.Month, p.Time.Day), p => p.Id)
             .Select(o => new EqpWipDailyStatistics { EqpId = eqpId, Date = o.Key, WipCount = o.Count() })
             .ToList();
        }

        public EqpWipDailyStatistics GetTodayStatistics(string eqpId)
        {
            DateTime today = DateTime.Now;

            return this.GetDailyStatistics(eqpId).Where(o=>o.Date.ToShortDateString()== today.ToShortDateString()).FirstOrDefault();
        }

        public void SaveChanges()
        {
            this._Context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return this._Context.SaveChangesAsync();
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
                this._Context?.Dispose();
                this._Context = null;
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~EqpWipRepository()
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

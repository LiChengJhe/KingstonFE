using EAP_WebAPI.Contexts;
using EAP_WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Repositories
{
    public class EqpStatusRepository : IDisposable
    {
        private KingstonContext _Context;
        private bool disposedValue;

        public EqpStatusRepository(KingstonContext context)
        {
            this._Context = context;
        }

        public void Add(EqpStatus status)
        {
                this._Context.EqpStatus.Add(status);
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
        ~EqpStatusRepository()
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

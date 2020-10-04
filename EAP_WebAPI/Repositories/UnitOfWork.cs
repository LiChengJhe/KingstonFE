using EAP_WebAPI.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private KingstonContext _Context;
        private EqpInfoRepository _EqpInfoRepository;
        private EqpAlarmRepository _EqpAlarmRepository;
        private EqpStatusRepository _EqpStatusRepository;
        private EqpWipRepository _EqpWipRepository;

        private bool disposedValue;

        public UnitOfWork(KingstonContext context) {
            this._Context = context;
        }


        public EqpInfoRepository EqpInfoRepo
        {
            get
            {

                if (this._EqpInfoRepository == null)
                {
                    this._EqpInfoRepository = new EqpInfoRepository(this._Context);
                }
                return this._EqpInfoRepository;
            }
        }
        public EqpAlarmRepository EqpAlarmRepo
        {
            get
            {

                if (this._EqpAlarmRepository == null)
                {
                    this._EqpAlarmRepository = new EqpAlarmRepository(this._Context);
                }
                return this._EqpAlarmRepository;
            }
        }

        public EqpStatusRepository EqpStatusRepo
        {
            get
            {

                if (this._EqpStatusRepository == null)
                {
                    this._EqpStatusRepository = new EqpStatusRepository(this._Context);
                }
                return this._EqpStatusRepository;
            }
        }

        public EqpWipRepository EqpWipRepo
        {
            get
            {

                if (this._EqpWipRepository == null)
                {
                    this._EqpWipRepository = new EqpWipRepository(this._Context);
                }
                return this._EqpWipRepository;
            }
        }

        public void SaveChanges() {
            this._Context.SaveChanges();
        }

        public  Task SaveChangesAsync()
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

        // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~UnitOfWork()
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

using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EAP_Library.Configs
{
    public class QueueHelper : IDisposable
    {
        private string _ConnectionString;
        private string _QueueName;
        private QueueConfig _QueueConfig;
        private IQueueClient _Client;
        private bool disposedValue;

        public QueueHelper(QueueConfig config)
        {
            this._QueueConfig = config;
            this._Client = new QueueClient(this._QueueConfig.ConnectionString, this._QueueConfig.QueueName);
        }

        public async Task SendMessagesAsync<T>(T messages)
        {
            try
            {
                string body = JsonSerializer.Serialize(messages);
                Message message = new Message(Encoding.UTF8.GetBytes(body));
                await this._Client.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[{DateTime.Now}] Exception: {exception.Message}");
            }
        }

        public void ReceiveMessages<T>(Action<T> messagesHandler, Action<Exception, ExceptionReceivedContext> exceptionHandler = null, int maxConcurrentCalls = 1, bool autoComplete = false)
        {
            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(exceptionReceivedEventArgs => {
                ExceptionReceivedContext context = exceptionReceivedEventArgs.ExceptionReceivedContext;
                if (exceptionHandler != null) exceptionHandler(exceptionReceivedEventArgs.Exception, exceptionReceivedEventArgs.ExceptionReceivedContext);
                return Task.CompletedTask;
            })
            {
                MaxConcurrentCalls = maxConcurrentCalls,
                AutoComplete = autoComplete,
            };

            this._Client.RegisterMessageHandler(async (Message message, CancellationToken token) => {
                if (messagesHandler != null)
                {
                    string body = Encoding.UTF8.GetString(message.Body);
                    messagesHandler(JsonSerializer.Deserialize<T>(body));
                }
                await this._Client.CompleteAsync(message.SystemProperties.LockToken);
            }
            , messageHandlerOptions);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }
                this._Client.CloseAsync();
                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        ~QueueHelper()
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

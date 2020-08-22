using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EAP_Simulation.ServiceBus
{
    public class QueueHelper
    {
        private string _ConnectionString;
        private string _QueueName;
        private IQueueClient _Client;
        public QueueHelper(string connStr, string queueName)
        {
            this._ConnectionString = connStr;
            this._QueueName = queueName;
            this._Client = new QueueClient(this._ConnectionString, this._QueueName);
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

        public void ReceiveMessages<T>(Action<T> messagesHandler, Action<Exception, ExceptionReceivedContext> exceptionHandler=null, int maxConcurrentCalls = 1, bool autoComplete = false)
        {
            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(exceptionReceivedEventArgs => {
                ExceptionReceivedContext context = exceptionReceivedEventArgs.ExceptionReceivedContext;
                if (exceptionHandler != null) exceptionHandler(exceptionReceivedEventArgs.Exception, exceptionReceivedEventArgs.ExceptionReceivedContext);
                return Task.CompletedTask;
            }) {
                MaxConcurrentCalls = maxConcurrentCalls,
                AutoComplete = autoComplete,
            };

            this._Client.RegisterMessageHandler(async (Message message, CancellationToken token) =>{
            if (messagesHandler != null)
            {
                string body = Encoding.UTF8.GetString(message.Body);
                messagesHandler(JsonSerializer.Deserialize<T>(body));
            }
            await this._Client.CompleteAsync(message.SystemProperties.LockToken);}
            , messageHandlerOptions);

        }


    }
}

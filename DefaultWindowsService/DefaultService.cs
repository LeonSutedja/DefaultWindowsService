using Newtonsoft.Json;
using NLog;
using System;
using System.ServiceProcess;

namespace DefaultWindowsService
{
    public class MessageLog
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Received { get; set; }
    }

    partial class DefaultService : ServiceBase
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        System.Timers.Timer timeDelay;
        int count;
        public DefaultService()
        {
            InitializeComponent();
            timeDelay = new System.Timers.Timer();
            timeDelay.Elapsed += new System.Timers.ElapsedEventHandler(WorkProcess);
        }

        public void WorkProcess(object sender, System.Timers.ElapsedEventArgs e)
        {
            string process = "Timer Tick " + count;
            var message = new MessageLog()
            {
                Message = process,
                ErrorMessage = string.Empty,
                Received = DateTime.Now
            };
            LogService(message);
            count++;
        }

        protected override void OnStart(string[] args)
        {
            var message = new MessageLog()
            {
                Message = "Service Started",
                ErrorMessage = string.Empty,
                Received = DateTime.Now
            };
            LogService(message);
            timeDelay.Enabled = true;
        }

        protected override void OnStop()
        {
            var message = new MessageLog()
            {
                Message = "Service Stopped",
                ErrorMessage = string.Empty,
                Received = DateTime.Now
            };
            LogService(message);
            timeDelay.Enabled = false;
        }

        private void LogService(MessageLog messageLog)
        {
            var serializedMessage = JsonConvert.SerializeObject(messageLog);
            _logger.Debug(serializedMessage);
        }     
    }
}

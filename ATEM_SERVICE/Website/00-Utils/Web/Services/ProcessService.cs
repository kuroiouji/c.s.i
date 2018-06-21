using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Web.Services
{
    internal class ProcessTaskService: IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts =
                                                       new CancellationTokenSource();

        public ProcessTaskService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Utils.LogUtil.WriteLog("Prcess Task Service Start.");


            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Utils.LogUtil.WriteLog("Prcess Task Service Stop.");

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                                                                cancellationToken));
            }
        }

        public void Dispose()
        {
            Utils.LogUtil.WriteLog("Prcess Task Service Dispose.");
            _stoppingCts.Cancel();
        }

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (Utils.CommonUtil.IsNullOrEmpty(Web.Constants.PROCESS_TIME))
                return;

            try
            {
                CultureInfo info = new CultureInfo("en-US");

                DateTime nowDate = DateTime.Now;
                DateTime processDate;

                if (DateTime.TryParseExact(
                                string.Format(info, "{0:yyyyMMdd} {1}:00", nowDate, Web.Constants.PROCESS_TIME),
                                "yyyyMMdd HH:mm:ss", info, DateTimeStyles.None, out processDate))
                {
                    while (true)
                    {
                        nowDate = DateTime.Now;

                        TimeSpan diff = processDate.Subtract(nowDate);
                        if (diff.TotalSeconds < 0)
                        {
                            processDate = processDate.AddDays(1);
                            diff = processDate.Subtract(nowDate);
                        }

                        while (diff.TotalSeconds > 0)
                        {
                            //Utils.LogUtil.WriteLog(string.Format(info,
                            //    "Process HQ2B จะเริ่มทำงานเวลา {0:dd/MM/yyyy} {1} ({2:N0} นาที)", processDate, Web.Constants.PROCESS_TIME, diff.TotalMinutes));

                            int waiting = 60; //  -- Minute(s)
                            if (diff.TotalMinutes < waiting)
                                waiting = (int)Math.Floor(diff.TotalMinutes);
                            if (waiting == 0)
                                break;

                            await Task.Delay((int)TimeSpan.FromMinutes(waiting).TotalMilliseconds);

                            nowDate = DateTime.Now;
                            diff = processDate.Subtract(nowDate);
                        }

                        nowDate = DateTime.Now;
                        if (diff.TotalMilliseconds > 0)
                            await Task.Delay((int)diff.TotalMilliseconds);

                        nowDate = DateTime.Now;
                        Utils.LogUtil.WriteLog(string.Format(info, "เรียก Process HQ2B สำหรับส่งข้อมูลวันที่ {0:dd/MM/yyyy}", nowDate));

                        try
                        {
                            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                            var exepath = System.IO.Path.GetDirectoryName(location);

                            System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
                            proc.FileName = System.IO.Path.Combine(exepath, "exe", "DBInterfaceApp", "DBInterfaceApp.exe");
                            proc.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                                                            Utils.Constants.TEMP_PATH,
                                                            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"),
                                                            "HQ2B",
                                                            "G",
                                                            Utils.ConvertUtil.ConvertToString(nowDate, "yyyyMMdd"),
                                                            "A");
                            proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            proc.CreateNoWindow = true;
                            proc.UseShellExecute = false;

                            System.Diagnostics.Process.Start(proc);
                        }
                        catch (Exception ex)
                        {
                            Utils.LogUtil.WriteLog(ex);
                        }

                        processDate = processDate.AddDays(1);
                    }
                }
            }
            catch(Exception ex)
            {
                Utils.LogUtil.WriteLog(ex);
            }
        }
    }
}

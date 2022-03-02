using AspnetMVCTimer.Managers;
using AspnetMVCTimer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AspnetMVCTimer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static long TimerInterval = SystemVariableManager.TIMER_INTERVAL;
        private System.Timers.Timer _timer;
        public System.Timers.Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new System.Timers.Timer();
                    _timer.AutoReset = false;
                    _timer.Enabled = false;
                    _timer.Interval = TimerInterval;
                    _timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckIntervalElapsed);
                }
                return _timer;
            }
        }

        private void CheckIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Timer.Enabled = false;
                if (TimerJobThread == null || !TimerJobThread.IsAlive)
                {
                    TimerJobThread = new Thread(DoTimerJob);
                    TimerJobThread.IsBackground = true;
                    TimerJobThread.Name = "DoTimerJob";
                    TimerJobThread.Start();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Timer.Interval = TimerInterval;
                Timer.Enabled = true;
            }
        }

        private void DoTimerJob()
        {
            try
            {
                //yapılması gereken işlem...
                Record myRecord = new Record()
                {
                    Message = $"Bu kaydı ekliyorum..." + $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"
                };
                TimerTestDBEntities myContext = new TimerTestDBEntities();
                myContext.Records.Add(myRecord);
                myContext.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        private Thread TimerJobThread = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Eventı proje ilk ayağa kalktığında çağıracağız
            CheckIntervalElapsed(null, null);
        }

        protected void Application_Error()
        {
            //Application istenmeyyen ya da handle edilemeyen bir hataya düşerse bu metot çalışır.
        }
    }
}

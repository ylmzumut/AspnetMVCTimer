using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AspnetMVCTimer.Managers
{
    public class SystemVariableManager
    {
        public static long TIMER_INTERVAL
        {
            get
            {
                try
                {
                    var timerIntervalValue = ConfigurationManager.AppSettings["TIMER_INTERVAL"].ToString();
                    if (string.IsNullOrEmpty(timerIntervalValue))
                    {
                        return 6000;
                    }
                    return Convert.ToInt64(timerIntervalValue);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "Web.Config dosyasında TIMER_INTERVAL keyi olmalıdır!");
                }
            }
        }
    }
}
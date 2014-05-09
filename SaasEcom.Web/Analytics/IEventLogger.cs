using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaasEcom.Web.Analytics
{
    public interface IEventLogger
    {
        void LogEvent<T>(T logEvent);
    }
}
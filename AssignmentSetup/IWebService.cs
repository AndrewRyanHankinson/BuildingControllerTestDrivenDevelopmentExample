using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public class WebService : IWebService
    {
        public void LogFireAlarm(string logDetails)
        {

        }

        public void LogStateChange(string logDetails)
        {

        }

        public void LogEngineerRequired(string logDetails)
        {

        }

    }
    public interface IWebService
    {
        void LogFireAlarm(string logDetails);
        void LogStateChange(string logDetails);
        void LogEngineerRequired(string logDetails);
    }

}

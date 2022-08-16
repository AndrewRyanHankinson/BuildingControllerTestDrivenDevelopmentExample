using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public class FireAlarmManager : IFireAlarmManager
    {
        bool engineerRequired;

        public string GetStatus()
        {
            string status = " ";
            return status;
        }

        public void SetAlarm(bool isActive)
        {
            
        }
    }

    public interface IFireAlarmManager
    {
        string GetStatus();
        void SetAlarm(bool isActive);
    }
}

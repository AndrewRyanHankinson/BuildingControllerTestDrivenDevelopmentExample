using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public class DoorManager : IDoorManager
    {
        bool engineerRequired;

        public string GetStatus()
        {
            string status = " ";
            return status;
        }

        public bool LockDoor(int doorID)
        {
            return true;
        }

        public bool OpenDoor(int doorID)
        {
            return true;
        }

        public bool OpenAllDoors()
        {
            return true;
        }

        public bool LockAllDoors()
        {
            return true;
        }
    }

    public interface IDoorManager
    {
        string GetStatus();
        bool LockDoor(int doorID);
        bool OpenDoor(int doorID);
        bool OpenAllDoors();
        bool LockAllDoors();
    }

}

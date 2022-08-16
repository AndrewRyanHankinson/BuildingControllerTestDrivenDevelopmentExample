using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public class LightManager : ILightManager
    {
        bool engineerRequired;
        public string GetStatus()
        {
            string status = " ";
            return status;
        }

        public void SetAllLights(bool isOn)
        {
           
        }
    }

    public interface ILightManager
    {
        string GetStatus();
        void SetAllLights(bool isOn);
    }

    
}

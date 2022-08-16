using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public class BuildingController
    {
        string buildingID; //Variable used to hold the building ID
        public string currentState; //Holds the value of state the building is current in
        string previousState; //Used for the fire alarm and fire drill state as they can only transition back to the previosu state they were in

        ILightManager LightManager;
        IDoorManager DoorManager;
        IFireAlarmManager FireAlarmManager;
        IWebService WebService;
        IEmailService EmailService;

        public BuildingController(string id)
        {
            SetBuildingID(id.ToLower()); //Calls the function and converts the id that has been input into lowercase
            currentState = "out of hours"; //Sets the default current state to "out of hours" because from here any state change is valid 
        }

        //Constructor that is used to test valid transitions between states, it is possible to input both the current state and the possibly new state
        public BuildingController(string id, string startState)  
        {
            currentState = startState.ToLower(); //Sets the current state to the one of the valid start states that has been given and sets it to lowercase

            //Checks for the valid current state options
            if (currentState == "closed" || currentState == "out of hours" || currentState == "open")
            {
                SetBuildingID(id.ToLower()); //Calls the function and converts the id that has been input into lowercase
            }
            else
            {
                //Brings up an error exception if the startstate is not valid
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'"); 
            }
        }

        //Constructor that allows the test cases to pass in both ID and start state so that valid state starts and possible state changes can be tested
        public BuildingController(string id, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            SetBuildingID(id.ToLower()); //Calls the function and converts the id that has been input into lowercase
            currentState = "out of hours"; //Sets the default current state to "out of hours" because from here any state change is valid 
            LightManager = iLightManager;
            FireAlarmManager = iFireAlarmManager;
            DoorManager = iDoorManager;
            WebService = iWebService;
            EmailService = iEmailService;
        }

        public string GetStatusReport()
        {
            //Combines the 3 manager status so that it can be displayed as 1 string
            return LightManager.GetStatus()+ "," + DoorManager.GetStatus() + "," + FireAlarmManager.GetStatus();
        }

        public void SetBuildingID(string id)
        {
            //Stores the input into the buildingID variable and converts the string into lower case
            buildingID = id.ToLower();
        }

        public string GetBuildingID()
        {
            return buildingID; //Returns the buildingID when the GetBuilding function is called
        }

        public bool SetCurrentState(string state)
        {
            if (currentState == "closed")
            {
                //if the current state and state are both "closed" the SetCurrentState will return true and no change will be made to the state
                if (state == "closed")
                {
                    return true;
                }
                else if (state == "out of hours")
                {
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    return true;
                }
                else if (state == "fire drill")
                {
                    //Save the current state before it is overwritten as once the building is in fire drill state it can only go back to the previous state
                    previousState = currentState; 
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    return true;
                }
                else if (state == "fire alarm")
                {
                    //Save the current state before it is overwritten as once the building is in fire alarm state it can only go back to the previous state
                    previousState = currentState;
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    FireAlarmManager.SetAlarm(true); //Sets of the fire alarm
                    DoorManager.OpenAllDoors(); //Opens all of the doors
                    LightManager.SetAllLights(true); //Turns on all of the lights
                    WebService.LogFireAlarm("fire alarm"); //Creates a log
                    return true;
                }
                else
                {
                    return false;
                }
            } 
            else if (currentState == "out of hours")
            {
                if (state == "closed")
                {
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    DoorManager.LockAllDoors(); //Locks all doors
                    LightManager.SetAllLights(false); //Turns off all lights
                    return true;
                }
                //if the current state and state are both "out of hours" the SetCurrentState will return true and no change will be made to the state
                else if (state == "out of hours")
                {
                    return true;
                }
                else if (state == "open")
                {
                    if (DoorManager.OpenAllDoors() == true)
                    {
                        currentState = state; //Overwrites the current state with the new state as it is valid transition 
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else if (state == "fire drill")
                {
                    //Save the current state before it is overwritten as once the building is in fire drill state it can only go back to the previous state
                    previousState = currentState;
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    return true;
                }
                else if (state == "fire alarm")
                {
                    //Save the current state before it is overwritten as once the building is in fire alarm state it can only go back to the previous state
                    previousState = currentState;
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    FireAlarmManager.SetAlarm(true); //Sets of the fire alarm
                    DoorManager.OpenAllDoors(); //Opens all of the doors
                    LightManager.SetAllLights(true); //Turns on all of the lights
                    WebService.LogFireAlarm("fire alarm"); //Creates a log
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentState == "open")
            {
                if (state == "out of hours")
                {
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    return true;
                }
                //if the current state and state are both "open" the SetCurrentState will return true and no change will be made to the state
                else if (state == "open")
                {
                    return true;
                }
                else if (state == "fire drill")
                {
                    //Save the current state before it is overwritten as once the building is in fire drill state it can only go back to the previous state
                    previousState = currentState;
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    return true;
                }
                else if (state == "fire alarm")
                {
                    //Save the current state before it is overwritten as once the building is in fire alarm state it can only go back to the previous state
                    previousState = currentState;
                    currentState = state; //Overwrites the current state with the new state as it is valid transition 
                    FireAlarmManager.SetAlarm(true); //Sets of the fire alarm
                    DoorManager.OpenAllDoors(); //Opens all of the doors
                    LightManager.SetAllLights(true); //Turns on all of the lights
                    WebService.LogFireAlarm("fire alarm"); //Creates a log
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentState == "fire drill" || currentState == "fire alarm")
            {
                if (state == previousState)
                {
                    if (previousState == "open")
                    {
                        if (DoorManager.OpenAllDoors() == true)
                        {
                            currentState = state; //Overwrites the current state with the new state as it is valid transition 
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (previousState == "closed")
                    {
                        currentState = state; //Overwrites the current state with the new state as it is valid transition 
                        DoorManager.LockAllDoors(); //Locks all the doors
                        LightManager.SetAllLights(false); //Turns off all the lights
                        return true;
                    }
                    else
                    {
                        currentState = state; //Overwrites the current state with the new state as it is valid transition 
                        return true;
                    }
                }
                //if the current state and state are both the same the SetCurrentState will return true and no change will be made to the state
                if (state == currentState)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string GetCurrentState()
        {
            return currentState;
        }
    }
}

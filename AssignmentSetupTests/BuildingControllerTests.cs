using AssignmentSetup;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetupTests
{
    [TestFixture]
    public class BuildingControllerTests
    {
        //L1R1 L1R2 Test checking that an ID is returned and equals the input
        [TestCase("001")]
        [TestCase("ct001")]
        [TestCase("CT12da")]
        public void Constructor_WhenSet_InitializesBuildingID(string id)
        {
            //Arrange
            BuildingController bc;
            //Act
            bc = new BuildingController(id); //Creates a buildingcontroller object using the id given
            string result = bc.GetBuildingID(); //Retrieves the build id
            //Assert
            Assert.AreEqual(result, id.ToLower()); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L1R3 Check to make sure the constructor converts the ID to lower case
        [TestCase("VF2SD", "vf2sd")]
        [TestCase("5654A", "5654a")]
        [TestCase("HELLO", "hello")]
        //Test cases to check if upper case is converted to lower case
        public void Constructor_WhenIDSet_ConvertToLowerCase(string id, string lowerID)
        {
            //Arrange
            BuildingController bc;
            //Act
            bc = new BuildingController(id); //Creates a buildingcontroller object using the id given
            string result = bc.GetBuildingID();//Retrieves the build id
            //Assert
            Assert.AreEqual(result, lowerID); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L1R4 The SetBuildingID returns with the lowercase ID of the input
        [TestCase("CT001", "ct001")]
        [TestCase("mP078", "mp078")]
        [TestCase("sdaSAD", "sdasad")]
        //Test cases to check if upper case is converted to lower case
        public void SetBuildingID_WhenSet_ConvertToLowerCase(string id, string lowerID)
        {
            //Arrange
            BuildingController bc;
            //Act
            bc = new BuildingController(id); //Creates a buildingcontroller object
            bc.SetBuildingID(id); //Uses the input given to set the building
            string result = bc.GetBuildingID(); // Retrieves the build id
            //Assert
            Assert.AreEqual(result, lowerID); // Compares the two strings to see if they match,  if they do pass the test
        }

        [Test]//L1R5 L1R6 Check to see that the function returns the value "out of hours"
        public void Constructor_IsOutOfHours_ReturnTrue()
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001");
            //Act
            string result = bc.GetCurrentState(); // Retrieves the currentState and stores it
            //Assert
            Assert.AreEqual(result, "out of hours"); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L1R5 L1R6 Check to see that no other result are returned from GetCurrentState
        [TestCase ("closed")]
        [TestCase("open")]
        [TestCase("OUT OF HOURS")]
        public void GetCurrentState_IsNotOutOfHours_ReturnFalse(string state)
        {
            //Arrange
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            string result = bc.GetCurrentState(); // Retrieves the currentState and stores it
            //Assert
            Assert.AreNotEqual(result, state); // Compares the two strings to see if they match,  if they don't pass the test
        }

        //L1R7 Return true if the state is valid
        [TestCase("closed")]
        [TestCase("out of hours")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        //Test cases for all valid states
        public void SetCurrentState_IsValid_ReturnTrue(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(true);
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsTrue(result); //Checks to see if the result is true, if it is pass the test
        }

        //L1R7 Tests to see if the set state matches the input
        [TestCase("closed")]
        [TestCase("out of hours")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")] 
        //Test cases of all valid states
        public void SetCurrentState_IsValid_AreEqual(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(true);
            bc.SetCurrentState(state);
            string result = bc.GetCurrentState();
            //Assert
            Assert.AreEqual(state, result); // Compares the two strings to see if they match,  if they don't pass the test
        }

        //L1R7 Returns false if the state is invalid
        [TestCase("Closed")]
        [TestCase("out of hrs")]
        [TestCase("231DFS")]
        [TestCase("firedrill")]
        [TestCase("FIRE ALARM")]
        public void SetCurrentState_StateIsInvalid_ReturnFalse(string state)
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001"); //Creates a buildingcontroller object
            //Act
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsFalse(result); //Checks to see if the result is false, if it is pass the test
        }

        //L1R7 Tests to see if the set state doesnt match the input as it is invalid and the state will not change
        [TestCase("Closed")]
        [TestCase("out of hrs")]
        [TestCase("231DFS")]
        [TestCase("firedrill")]
        [TestCase("FIRE ALARM")]
        public void SetCurrentState_StateIsInValid_AreNotEqual(string state)
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001"); //Creates a buildingcontroller object
            //Act
            bc.SetCurrentState(state);
            string result = bc.GetCurrentState();
            //Assert
            Assert.AreNotEqual(result, state); // Compares the two strings to see if they match,  if they don't pass the test
        }

        //L2R1 Only allow specific state changes, from start state to current state
        [TestCase("closed", "out of hours")]
        [TestCase("out of hours", "closed")]
        [TestCase("out of hours", "open")]
        [TestCase("open", "out of hours")]
        //Testcases of valid state transitions
        public void SetCurrentState_StateChangeIsValid_ReturnTrue(string startState, string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            bc.currentState = startState;
            dm.OpenAllDoors().Returns(true);
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsTrue(result); //Checks to see if the result is true, if it is pass the test
        }

        //L2R1 Return false when an invalid state change is attempted
        [TestCase("closed", "open")]
        [TestCase("open", "closed")]
        //Test cases of invalid state transitions
        public void SetCurrentState_IsInvalid_ReturnFalse(string startState, string state)
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001", startState); //Creates a buildingcontroller object
            //Act
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsFalse(result); //Checks to see if the result is false, if it is pass the test
        }

        //L2R2 If current state and start state are the same then return true
        [TestCase("closed", "closed")]
        [TestCase("out of hours", "out of hours")]
        [TestCase("open", "open")]
        //Test cases that uses the same start state as state to transition into
        public void SetCurrentState_IsSame_ReturnTrue(string startState, string state)
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001", startState); //Creates a buildingcontroller object
            //Act
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsTrue(result); //Checks to see if the result is true, if it is pass the test
        }

        //L2R3 Test for storing current state as lower case
        [TestCase("CLOSED")]
        [TestCase("OUT OF HOURS")]
        [TestCase("OPEN")]
        public void Constructor_CurrentStateIsLower_ReturnEqual(string startState)
        {
            //Arrange
            BuildingController bc = new BuildingController("CT001", startState); //Creates a buildingcontroller object
            //Act
            string result = bc.currentState;
            //Assert
            Assert.AreEqual(startState.ToLower(), result); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L2R3 Test to check that an exception is thrown if an invalid start state is entered
        [TestCase("fire drill", "closed")]
        [TestCase("fire alarm", "out of hours")]
        [TestCase("CLOSE", "open")]
        public void StateChange_Invalid_GetException(string startState, string state)
        {
            //Arrange
            string id = "CT001";
            //Act

            //Assert
            Assert.That(() => new BuildingController(id, startState), Throws.ArgumentException); //Checks to see that an exact exception is thrown, if it is pass the test 
        }

        //L2R3 Test to check that an exact exception is thrown
        [TestCase("fire drill", "closed")]
        [TestCase("fire alarm", "out of hours")]
        [TestCase("CLOSE", "open")]
        public void Constructor_StateChangeInvalid_GetExactException(string startState, string state)
        {
            //Arrange
            string argumentAcception = "Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'";
            //Act
            var ex = Assert.Throws<ArgumentException>(() => new BuildingController("CT001", startState)); 
            //Assert
            Assert.That(ex.Message, Is.EqualTo(argumentAcception));
            //If the argument thrown matches the string its being compared to pass the test
        }

        //L3R1 L3R2 Test to make sure the lights getstatus returns values matching the input
        [TestCase("Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("Lights,OK,OK,OK,OK,OK,FAULT,OK,OK,OK,OK")]
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT")]
        public void ILightManager_GetStatus_IsEqual(string lightsResult)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            lm.GetStatus().Returns(lightsResult);
            //Act
            string result = lm.GetStatus(); //Returns the status string of lights manager and stores it
            //Assert
            Assert.AreEqual(lightsResult, result); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L3R1 L3R2 Test to make sure the fire alarm getstatus returns values matching the input
        [TestCase("FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("FireAlarm,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT")]
        public void IFireAlarmManager_GetStatus_IsEqual(string fireAlarmResult)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            fa.GetStatus().Returns(fireAlarmResult);
            //Act
            string result = fa.GetStatus(); //Returns the status string of fire alarm manager and stores it
            //Assert
            Assert.AreEqual(fireAlarmResult, result); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L3R1 L3R2 Test to make sure the door getstatus returns values matching the input
        [TestCase("Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT")]
        [TestCase("Doors,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT")]
        public void IDoorManager_GetStatus_IsEqual(string doorsResult)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            dm.GetStatus().Returns(doorsResult);
            //Act
            string result = dm.GetStatus(); //Returns the status string of door manager and stores it
            //Assert
            Assert.AreEqual(doorsResult, result); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L3R3 Tests to see that the GetStatusReport combines all 3 reports
        [TestCase("Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("Lights,OK,OK,OK,OK,OK,FAULT,OK,OK,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "Doors,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "FireAlarm,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT")]
        public void GetStatusReport_CombinesAllStatus_IsEqual(string lightsResult, string doorsResult, string fireAlarmResult)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            lm.GetStatus().Returns(lightsResult);
            dm.GetStatus().Returns(doorsResult);
            fa.GetStatus().Returns(fireAlarmResult);
          
            string expectedResult = lightsResult + "," + doorsResult + "," + fireAlarmResult;
            //Act
            string result = bc.GetStatusReport();
            //Assert
            Assert.AreEqual(expectedResult, result); // Compares the two strings to see if they match,  if they do pass the test
        }

        //L3R3
        [TestCase("Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT")]
        [TestCase("Lights,OK,OK,OK,OK,OK,FAULT,OK,OK,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK")]
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "Doors,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "FireAlarm,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT")]
        public void BuildingController_GetStatusReport_IsNotEqual(string lightsResult, string doorsResult, string fireAlarmResult)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            lm.GetStatus().Returns(lightsResult);
            dm.GetStatus().Returns(doorsResult);
            fa.GetStatus().Returns(fireAlarmResult);

            string expectedResult = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            //Act
            string result = bc.GetStatusReport();
            //Assert
            Assert.AreNotEqual(expectedResult, result); // Compares the two strings to see if they match,  if they don't pass the test
        }

        //L3R4 When moving to the open state if the OpenAllDoors method returns false so should the SetCurrentState
        [TestCase("open")]
        public void SetCurrentState_OpenAllDoors_ReturnFalse(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(false);
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsFalse(result); //Checks to see if the result is false, if it is pass the test
        }

        //L3R4 When moving to the open state if the OpenAllDoors method returns false the state should not change
        [TestCase("open")]
        public void SetCurrentState_OpenAllDoors_ReturnNotEqual(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(false); //Fault in opening the doors
            bc.SetCurrentState(state);
            string result = bc.GetCurrentState();
            //Assert
            Assert.AreNotEqual(state, result); // Compares the two strings to see if they match,  if they don't pass the test
        }

        //L3R5 When state changes to "open" and OpenAllDoors returns true meaning no faults, SetCurrentState returns true
        [TestCase("open")]
        public void SetCurrentState_OpenAllDoors_ReturnTrue(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(true); //No fault in opening the doors
            bool result = bc.SetCurrentState(state);
            //Assert
            Assert.IsTrue(result); //Checks to see if the result is true, if it is pass the test
        }

        //L3R5
        [TestCase("open")]
        public void SetCurrentState_ChangesToOpen_ReturnOpen(string state)
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            dm.OpenAllDoors().Returns(true); //No fault in opening the doors
            bc.SetCurrentState(state);
            string result = bc.GetCurrentState();
            //Assert
            Assert.AreEqual(result, state); // Compares the two strings to see if they match, if they do pass the test
        }

        //L4R1
        [Test]
        public void SetCurrentState_ToClosed_LockDoorsIsTrue()
        {
            //Arrange
            //Creates a mock interface for all 5 dependices
            ILightManager lm = Substitute.For<ILightManager>();
            IFireAlarmManager fa = Substitute.For<IFireAlarmManager>();
            IDoorManager dm = Substitute.For<IDoorManager>();
            IWebService ws = Substitute.For<IWebService>();
            IEmailService es = Substitute.For<IEmailService>();

            BuildingController bc = new BuildingController("CT001", lm, fa, dm, ws, es); //Creates a buildingcontroller object
            //Act
            bc.SetCurrentState("closed");
            dm.LockAllDoors().Returns(true); //No fault in locking the doors
            bool result = dm.LockAllDoors();
            //Assert
            Assert.IsTrue(result); //Checks to see if the result is true, if it is pass the test
        }
    }
}

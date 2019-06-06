# Quest-Exploration
This repository contains the Unity project and C++ projects necessary to run a virtual reality navigation simulation using the Oculus Quest and Motu soundboard connected to 4 vibrotactors. A significant amount of additional setup is necessary to succesfully run the test. For this, see the Wiki.

As for the files:
# Command Prompt Applications 
contains the executables necessary to run the project. adb.exe is only necessary if the adb folder has not been added to path (instructions on how to do this can be found in the wiki), but proximityTest and beginTest are both necessary. proximityTest contains the code which interfaces with the Motu soundboard, and beginTest is a one line command prompt application which feeds the ouput of adb logcat into proximityTest (which is the mechanism by which the computer receives virtual position data). Thus, when ready to begin the test (after having followed all the wiki instructions) open beginTest.bat within the same folder as proximityTest.exe and adb.exe (if the latter is not in PATH).

# Unity 
contains two VR projects which can be opened using Unity Hub. These both use the Oculus Integration, meaning this plugin should not have to be redownloaded. These projects should be ready to build after opening (though the UI and temp files are missing, which Unity will readd automatically upon opening. 

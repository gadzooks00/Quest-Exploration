#include <TactorFX/TactorFX.hpp>
#include <MEL/Communications/MelShare.hpp>  // for plotting
#include <MEL/Devices/Windows/Keyboard.hpp> // for keyboard
#include <MEL/Utility/System.hpp>           // for sleep()
#include <MEL/Math/Random.hpp>              // for random
#include <MEL/Core/Console.hpp>             // for print
#include <MEL/Core/Clock.hpp>
#include <array>
#include <iostream>
#include <cstdio>
#include <memory>
#include <stdexcept>
#include <string>
#include <fstream>

typedef mel::Keyboard KB;
using mel::Key;
using mel::MelShare;
using mel::print;
using mel::random;
using mel::sleep;

#define NUM_CH 6

using namespace tfx;
using namespace std;

string filterLog(string logLine);
float sgn(float sgnNumber);

int main(int argc, char const *argv[])
{
    tfx::initialize(NUM_CH);
    DeviceInfo info = tfx::getCurrentDevice();
    mel::print(info.name);
    mel::print(info.index);
    mel::print(info.maxChannels);

    mel::Clock kbClock;
    string input_line;
    const float dur = 0.1f;
    const float freq = 175; // 175 for Evan's tactors
    const float a_time = 1.0f;
    const float s_time = 1.0f;
    const float r_time = 1.0f;
    int cycleTime = 500;
    const int horizLookTime = 100;
    float tactorValues[] = {0.0, 0.0, 0.0, 0.0};
    bool enableTactor[] = {true, true, true, true};
    bool horizFound = false, vertFound = false;
    ofstream outFile;
    outFile.open("dataOut.txt", ios::out);
    while (!KB::is_key_pressed(Key::Escape))
    {
        getline(cin, input_line);
        if (input_line.find("!!!") != string::npos)
        {
            input_line.erase(0, 3);
            for (int i = 0; i < 4; i++)
            {
                tactorValues[i] = 0;
            }
            mel::print(input_line);
            outFile << input_line + "\n";
        }
        input_line = filterLog(input_line);
        if (input_line.length() > 0)
        {
            mel::print(input_line);
            outFile << input_line + "\n";
            for (int i = 0; i < 4; i++)
            {
                size_t commaPos = input_line.find(",");
                tactorValues[i] = stof(input_line.substr(0, commaPos));
                input_line.erase(0, commaPos + 1);
            }
        }
        if (tactorValues[0] == 100 && tactorValues[2] == 100 && !horizFound)
        {
            kbClock.restart(); 
            float amp = 0.4f;
            auto osc = std::make_shared<SquareWave>(freq, amp);
            auto cue = std::make_shared<Cue>(osc, dur);
            while (kbClock.get_elapsed_time() < mel::milliseconds(horizLookTime))
            {
                tfx::playCue(0, cue);
                tfx::playCue(2, cue);
            }
            horizFound = true;
            enableTactor[0] = enableTactor[2] = false;
        }
        if(tactorValues[1] == 100 && tactorValues[3] == 100 && !vertFound)
        {
            kbClock.restart();
            float amp = 0.4f;
            auto osc = std::make_shared<SquareWave>(freq, amp);
            auto cue = std::make_shared<Cue>(osc, dur);
            while (kbClock.get_elapsed_time() < mel::milliseconds(horizLookTime))
            {
                tfx::playCue(1, cue);
                tfx::playCue(3, cue);
            }
            vertFound = true;
            enableTactor[1] = enableTactor[3] = false;
        }
        if(tactorValues[0] != 100 || tactorValues[2] != 100)
        {
            horizFound = false;
        }
        if(tactorValues[1] != 100 || tactorValues[3] != 100)
        {
            vertFound = false;
        }
        if (kbClock.get_elapsed_time() > mel::milliseconds(cycleTime))
        {
            float avg = 0;
            for (int i = 0; i < 4; i++)
            {
                enableTactor[i] = tactorValues[i] != 100;
                if (enableTactor[i])
                {
                    float amp = 0.04f * sgn(tactorValues[i]);
                    auto osc = std::make_shared<SquareWave>(freq, amp);
                    auto cue = std::make_shared<Cue>(osc, dur);
                    tfx::playCue(i, cue);
                    avg += tactorValues[i];
                }
            }
            avg /= 4;
            kbClock.restart();
            cycleTime = (int)(10.0 / avg);
        }
    }
    tfx::finalize();
    outFile.close();
    return 0;
}
string filterLog(string logLine)
{
    size_t asteriskPos = logLine.find('*');
    if (asteriskPos != string::npos)
    {
        logLine.erase(0, asteriskPos + 3);
        return logLine;
    }
    return "";
}
float sgn(float sgnNumber)
{
    return (((sgnNumber) < 0) ? -1 : ((sgnNumber) > 0));
}
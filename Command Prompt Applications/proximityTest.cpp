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

int main(int argc, char const *argv[])
{
    tfx::initialize(NUM_CH);
    DeviceInfo info = tfx::getCurrentDevice();
    mel::print(info.name);
    mel::print(info.index);
    mel::print(info.maxChannels);

    mel::Clock kbClock;
    string input_line;

    int cycleTime = 100;
    float tactorValues[] = {0.0, 0.0, 0.0, 0.0};
    while (!KB::is_key_pressed(Key::Escape))
    {
        getline(cin, input_line);
        input_line = filterLog(input_line);
        if(input_line.find("!!!") != string::npos)
        {
            for(int i = 0; i < 4; i++)
            {
                tactorValues[i] = 0;
            }
            mel::print(input_line);
        }
        if (input_line.length() > 0)
        {
            mel::print(input_line);
            for (int i = 0; i < 4; i++)
            {
                size_t commaPos = input_line.find(",");
                tactorValues[i] = stof(input_line.substr(0, commaPos));
                input_line.erase(0, commaPos + 1);
            }
        }
        if (kbClock.get_elapsed_time() > mel::milliseconds(cycleTime))
        {
            for (int i = 0; i < 4; i++)
            {
                float freq = 175; // 175 for Evan's tactors
                float amp = 0.05f * tactorValues[i];
                float dur = 0.1f;
                auto osc = std::make_shared<SquareWave>(freq, amp);
                auto cue = std::make_shared<Cue>(osc, dur);
                tfx::playCue(i, cue);
            }
            kbClock.restart();
        }
    }
    tfx::finalize();
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
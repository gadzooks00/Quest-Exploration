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

    mel::Clock vClock, hClock;
    string input_line;
    const float dur = 0.1f;
    const float freq = 175; // 175 for Evan's tactors
    int vCycle = 500, hCycle = 250;
    float tactorValues[] = {0.0, 0.0, 0.0, 0.0};
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
        if(tactorValues[0] == tactorValues[1] == tactorValues[2] == tactorValues[3])
        {
            for(int i = 0; i < 4; i++)
            {
                auto osc = std::make_shared<SquareWave>(freq, 0.1f);
                auto cue = std::make_shared<Cue>(osc, dur);
                tfx::playCue(0, cue);
            }
        }
        if (vClock.get_elapsed_time() > mel::milliseconds(vCycle))
        {

            float amp = 0.025f * sgn(tactorValues[0]);

            auto osc = std::make_shared<SquareWave>(freq, amp);
            auto cue = std::make_shared<Cue>(osc, dur);
            tfx::playCue(0, cue);

            amp = 0.025f * sgn(tactorValues[2]);
            osc = std::make_shared<SquareWave>(freq, amp);
            cue = std::make_shared<Cue>(osc, dur);
            tfx::playCue(2, cue);

            vClock.restart();
            vCycle = (int)(100.0 * (0.5 / (tactorValues[0] + tactorValues[2])));
        }
        if (hClock.get_elapsed_time() > mel::milliseconds(hCycle))
        {
            float amp = 0.025f * sgn(tactorValues[1]);
            auto osc = std::make_shared<SquareWave>(freq, amp);
            auto cue = std::make_shared<Cue>(osc, dur);
            tfx::playCue(1, cue);

            amp = 0.025f * sgn(tactorValues[3]);
            osc = std::make_shared<SquareWave>(freq, amp);
            cue = std::make_shared<Cue>(osc, dur);
            tfx::playCue(3, cue);

            hClock.restart();
            hCycle = (int)(100.0 * (0.5 / (tactorValues[1] + tactorValues[3])));
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
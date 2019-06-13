
static class Options
{
    public const bool enableGuideArrow = true; //set to false if visual cue is not desired
    public const bool mobileGuideArrow = true; //have guide arrow point in the direction of the cube
    public const bool guideArrowProportionalSize = true;// have guide arrow change size in response to proximity
    public const bool enableActiveButton = true; //set to false if only passive vibrotactor guidance is desired
    public const bool twoChannelMode = true; //set to false if one channel mode is desired. This will guide the user horizontally,
                                             //then vertically. Two channel mode will turn on the horizontal tactors if button
                                             //three is pressed, and the vertical of button two is. (Both buttons can be pressed.)
    public const bool enableVibrotactorFeedback = true; //set to false if vibrotactor feedback is not desired
    public const bool enableWrongCube = false; //set to true to end the test if the user selects the wrong cube
    public const float correctDistance = 0.3f; //distance at which active mode will give different signal
}
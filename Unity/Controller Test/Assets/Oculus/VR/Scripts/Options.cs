
static class Options
{
    public const bool enableGuideArrow = false; //set to false if visual cue is not desired
    public const bool enableActiveButton = true; //set to false if only passive vibrotactor guidance is desired
    public const bool enableVibrotactorFeedback = true; //set to false if vibrotactor feedback is not desired
    public const bool enableWrongCube = true; //set to true to end the test if the user selects the wrong cube
    public const float correctDistance = 0.3f; //distance at which active mode will give different signal
}
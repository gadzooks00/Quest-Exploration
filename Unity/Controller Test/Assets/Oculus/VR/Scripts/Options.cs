
static class Options
{
    public const bool enableGuideArrow = true; //set to false if visual cue is not desired
    public const bool enableActiveButton = true; //set to false if only passive vibrotactor guidance is desired
    public const bool enableVibrotactorFeedback = true; //set to false if vibrotactor feedback is not desired
    public const bool enableWrongCube = false; //set to true to end the test if the user selects the wrong cube
    public const float correctDistance = 0.1f; //distance at which active mode will give different signal
}
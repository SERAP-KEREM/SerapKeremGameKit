namespace SerapKeremGameKit._Levels
{
    public static class StarEvaluator
    {
        public static int EvaluateStars(LevelConfig config, float completionTimeSec)
        {
            if (config == null || config.TimeThresholdsSec == null || config.TimeThresholdsSec.Length < 3)
                return 0;

            float t = completionTimeSec;
            if (t <= config.TimeThresholdsSec[0]) return 3;
            if (t <= config.TimeThresholdsSec[1]) return 2;
            if (t <= config.TimeThresholdsSec[2]) return 1;
            return 0;
        }
    }
}




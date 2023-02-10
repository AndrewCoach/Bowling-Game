var bowling = new BowlingGame();
int[] bowlingRuns = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
foreach (int i in bowlingRuns)
{
    Console.WriteLine($"[Played: {i}");
    bowling.Play(i);
    Console.WriteLine($"[Score:  {bowling.GetScore()}]");
}


public class BowlingGame
{
    public List<FrameData> Frames = new List<FrameData>();
    private int MoveCount = 1;
    private int FrameCount = 1;

    public void Play(int pins)
    {
        // Special case for last Frame's optional third game and the last turn == 21
        if (FrameCount == 11 && MoveCount == 21)
        {
            var lastFrame = Frames[FrameCount - 2];
            if (lastFrame.Spare || lastFrame.Strike)
                lastFrame.OptionalThirdGame = pins;
            return;
        }

        // If this is an odd game, we must create new frame
        bool isFirstMoveInFrame = MoveCount % 2 == 1;
        if (isFirstMoveInFrame)
        {
            Frames.Add(new FrameData
            {
                FrameNumber = FrameCount,
                ScoreFirstGame = pins,
            });

            // What if we had a strike on the first game in frame
            if(pins == 10)
            {
                // We go directly for next frame;
                FrameCount++;
                MoveCount ++;
            }

        }
        else // We adjust the existing frame and assign to second game
        {
            Frames[FrameCount - 1].ScoreSecondGame = pins;
            // Increment frame count for next frame
            FrameCount++;
        }

        // Increment moveCount
        MoveCount++;
    }

    public int GetScore()
    {
        int score = 0;
        bool spareCarry = false;
        bool strikeCarry = false;
        foreach (var frame in Frames)
        {
            // Assign score
            score += frame.ScorePerFrame;

            // If Spare Happened
            if (spareCarry)
            {
                // We have a spare bonus due from previous turn, but spare can only happen as n.2 in a frame
                score += frame.ScoreFirstGame; // Add this turn's score again
                spareCarry = false;
            }

            // set spare carry for next turn
            spareCarry = frame.Spare;

            // If Strike Happened
            if (strikeCarry)
            {
                // We take this frame score and add it as a bonus
                score += frame.ScorePerFrame; // Add this turn's score again
                strikeCarry = false;
            }

            // set strike carry for next frame
            strikeCarry = frame.Strike;

        }
        return score;
    }


    public class FrameData
    {
        public int FrameNumber { get; set; }
        public int ScoreFirstGame { get; set; } = 0;
        public int ScoreSecondGame { get; set; } = 0;
        public int OptionalThirdGame { get; set; } = 0;
        public bool Spare => ScoreFirstGame + ScoreSecondGame == 10 && ScoreFirstGame != 10;
        public bool Strike => ScoreFirstGame == 10;
        public int ScorePerFrame => ScoreFirstGame + ScoreSecondGame + OptionalThirdGame;
    }
}
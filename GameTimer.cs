﻿using System;

namespace Galactic_Vanguard
{
    public class GameTimer
    {
        private int framesPassed;
        private int secondsPassed;
        private int minutesPassed;

        public GameTimer()
        {
            framesPassed = 0;
            secondsPassed = 0;
            minutesPassed = 0;
        }

        public void Update()
        {
            framesPassed += 1;

            if (framesPassed % 120 == 0)
            {
                secondsPassed += 1;

                if(secondsPassed % 120 == 0)
                {
                    minutesPassed += 1;
                }
            }
        }

        public Tuple<int,int,int> GetTime()
        {
             return new Tuple<int,int,int>(framesPassed, secondsPassed, minutesPassed);
        }

        public int GetFramesPassed()
        {
            return framesPassed;
        }
    }
}

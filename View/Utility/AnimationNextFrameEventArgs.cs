using System;

namespace View.Utility
{
    class AnimationNextFrameEventArgs : EventArgs
    {

        public AnimationNextFrameEventArgs(int currentIteration)
        {
            CurrentIteration = currentIteration;
        }
        public int CurrentIteration { get; set; }
    }
}

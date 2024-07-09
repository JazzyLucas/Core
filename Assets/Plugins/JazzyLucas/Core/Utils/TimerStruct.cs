using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    public enum State
    {
        INACTIVE,
        SIGNALLING,
        ACTIVE,
    }
    /// <summary>
    /// A deterministic approach to a timer system.
    /// </summary>
    public struct TimerStruct
    {
        public State State { get; private set; }
        public float TimeLeft { get; private set; }
        public float TimeLeftPercent => TimeLeft / _length;
        public float TimeLeftOverallPercent => (TimeLeft + _length * intervalsLeft) / _length * (_intervals+1);
        public string TimeLeftFormatted => TimeLeft < 0 ? "0" : TimeLeft.ToString("G2");
        public string TimeLeftPercentFormatted => TimeLeftPercent < 0 ? "0" : TimeLeftPercent.ToString("G2");
        public string TimeLeftOverallPercentFormatted => TimeLeftOverallPercent < 0 ? "0" : TimeLeftOverallPercent.ToString("G2");
        private float intervalsLeft;
        private readonly float _length;
        private readonly float _intervals;
        
        public TimerStruct(float length, float repeatedTimes = 0)
        {
            State = State.ACTIVE;
            _length = length;
            TimeLeft = length;
            _intervals = repeatedTimes < 0 ? Mathf.Infinity : repeatedTimes;
            intervalsLeft = _intervals;
        }
        public static TimerStruct Process(TimerStruct timer, float deltaTime, out State state)
        {
            timer.TimeLeft -= timer.TimeLeft > 0 ? deltaTime : 0;
            if (timer.TimeLeft <= 0)
            {
                if (timer.intervalsLeft > 0)
                {
                    timer.TimeLeft = timer._length;
                    timer.intervalsLeft--;
                    timer.State = State.SIGNALLING;
                }
                else
                    timer.State = State.INACTIVE;
            }
            else
                timer.State = State.ACTIVE;
            state = timer.State;
            return timer;
        }
    }
}
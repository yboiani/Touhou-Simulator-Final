using System;
using UnityEngine;

namespace WanderTimer
{
    [Serializable]
    public class TimerUtility
    {
        [SerializeField] private float timerDuration;
        private float timerCurrent;
        public float NormalizedTime => NormalizeTime();
        
        public TimerUtility(float timerDuration)
        {
            this.timerDuration = timerDuration;
            timerCurrent = 0f;
        }

        private float NormalizeTime()
        {
            return timerCurrent / timerDuration;
        }

        public float GetCurrent()
        {
            return timerCurrent;
        }

        public void ForceDone()
        {
            timerCurrent += timerDuration;
        }
        
        public bool TimerDone()
        {
            if (timerCurrent >= timerDuration)
            {
                return true;
            }
            return false;
        }

        public bool ControlUpdate(float updateAmount)
        {
            timerCurrent = Mathf.Clamp(timerCurrent + updateAmount,0f,timerDuration);
            return TimerDone();
        }
        
        public void Update(float updateAmount)
        {
            timerCurrent = Mathf.Clamp(timerCurrent + updateAmount,0f,timerDuration);
        }

        public void Restart()
        {
            timerCurrent = 0;
        }
        
        public void SkipTime(float normalizedTime)
        {
            timerCurrent = timerDuration * normalizedTime;
        }
        
        public void Restart(float newDuration)
        {
            timerDuration = newDuration;
            timerCurrent = 0f;
        }
        
    }
}
using System;
using UnityEngine;
using World.Environment.Lightning;

namespace World.Environment
{
    public class TimeHandler : MonoBehaviour
    {
        public Sun sun;
        [SerializeField]
        DateTime date;
        
        [Range(1900, 2100)]
        public int year = 2022;

        [Range(1, 12)]
        public int month = 5;
        [Range(1, 31)]
        public int day = 5;
        
        [SerializeField]
        [Range(0, 24)]
        private int hour;

        [SerializeField]
        [Range(0, 60)]
        private int minutes;

        [SerializeField]
        private float timeSpeed = 1;

        [SerializeField]
        private int frameSteps = 1;
        private int frameStep;

        public bool realTime = false;
        
        private DateTime localTime;

        public DateTime LocalTime => localTime;
        private void OnValidate()
        {
            try
            {
                var d = new DateTime(year,month,day,hour,minutes,0);
                localTime = d;
                //Debug.Log(d);
                sun.SetPosition();
            }
            catch(ArgumentOutOfRangeException e)
            {
                Debug.LogWarning("bad date"+e.Message);
            }
        }
        
        public void SetTime(int hour, int minutes) 
        {
            this.hour = hour;
            this.minutes = minutes;            
            OnValidate();
        }

        public void SetDate(DateTime dateTime)
        {
            hour = dateTime.Hour;
            minutes = dateTime.Minute;
            date = dateTime.Date;
            OnValidate();
        }

        public void SetUpdateSteps(int i) 
        {
            frameSteps = i;
        }

        public void SetTimeSpeed(float speed) 
        {
            timeSpeed = speed;
        }

        private void Start()
        {
            if (!realTime)
            {
                localTime = new DateTime(year,month,day,hour,minutes,0);
                hour = localTime.Hour;
                minutes = localTime.Minute;
                date = localTime.Date;
            }
            else
            {
                localTime = DateTime.Now;
                hour = localTime.Hour;
                minutes = localTime.Minute;
                date = localTime.Date;
            }
        }
        
        private void Update()
        {
            localTime = localTime.AddSeconds(timeSpeed * Time.deltaTime);
            if (frameStep==0) 
            {
                hour = localTime.Hour;
                minutes = localTime.Minute;
                date = localTime.Date;
                sun.SetPosition();
            }
            frameStep = (frameStep + 1) % frameSteps;
        }
        
        public enum TimeStates
        {
            
        }
    }
}

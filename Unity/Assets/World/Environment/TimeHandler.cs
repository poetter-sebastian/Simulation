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
        
        public TimeEvents currentState;
        
        public static TimeHandler Instance;
        
        public event EventHandler TimeChangedToDawn;
        public event EventHandler TimeChangedToNoon;
        public event EventHandler TimeChangedToAfternoon;
        public event EventHandler TimeChangedToDusk;
        public event EventHandler TimeChangedToNight;
        public event EventHandler TimeChangedToMidnight;
        public event EventHandler TimeChangedToAfternight;
        
        public DateTime LocalTime => localTime;
        
        private void OnValidate()
        {
            try
            {
                var d = new DateTime(year,month,day,hour,minutes,0);
                localTime = d;
                //Debug.Log(d);
                sun.SetPosition();
                CalcStateFromTime(hour);
            }
            catch(ArgumentOutOfRangeException e)
            {
                Debug.LogWarning("bad date"+e.Message);
            }
        }

        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("World instance already set!");
            }
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
            TimeChangedToDawn += OnDawn;
            TimeChangedToNoon += OnNoon;
            TimeChangedToAfternoon += OnAfternoon;
            TimeChangedToDusk += OnDusk;
            TimeChangedToNight += OnNight;
            TimeChangedToMidnight += OnMidnight;
            TimeChangedToAfternight += OnAfternight;
        }
        
        private void Update()
        {
            localTime = localTime.AddSeconds(timeSpeed * Time.deltaTime);
            if (frameStep==0) 
            {
                //set time
                hour = localTime.Hour;
                minutes = localTime.Minute;
                date = localTime.Date;
                //set sun
                sun.SetPosition();
                //set state
                CalcStateFromTime(hour);
            }
            frameStep = (frameStep + 1) % frameSteps;
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

        private void OnDawn(object sender, EventArgs e)
        {
            Debug.Log("Its dawn!");
        }
        private void OnNoon(object sender, EventArgs e)
        {
            Debug.Log("Its noon!");
        }
        private void OnAfternoon(object sender, EventArgs e)
        {
            Debug.Log("Its after noon!");
        }
        private void OnDusk(object sender, EventArgs e)
        {
            Debug.Log("Its dusk!");
        }
        private void OnNight(object sender, EventArgs e)
        {
            Debug.Log("Its night!");
        }
        private void OnMidnight(object sender, EventArgs e)
        {
            Debug.Log("Its midnight!");
        }
        
        private void OnAfternight(object sender, EventArgs e)
        {
            Debug.Log("Its after night!");
        }       
        
        [Serializable]
        public enum Seasons
        {
            //humid continental climate
            Spring,
            Summer,
            Autumn,
            Winter,
            //equatorial climate
            WetSeason,
            DrySeason,
        }
        
        [Serializable]
        public enum TimeEvents
        {
            Dawn,
            Noon,
            Afternoon, 
            Dusk,
            Night,
            Midnight,
            Afternight,
        }
        
        public void CalcStateFromTime(int time)
        {
            var oldState = currentState;
            currentState = (time) switch
            {
                (>= 6 and < 11) => TimeEvents.Dawn,
                (>= 11 and < 13) => TimeEvents.Noon,
                (>= 13 and < 17) => TimeEvents.Afternoon,
                (>= 17 and < 21) => TimeEvents.Dusk,
                (>= 21 and < 23) => TimeEvents.Night,
                (>= 23) => TimeEvents.Midnight,
                (>= 1 and < 6) => TimeEvents.Afternight,
                _ => TimeEvents.Afternight,
            };
            if (oldState != currentState)
            {
                CallEventFromStatus(currentState)?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public EventHandler CallEventFromStatus(TimeEvents events)
        {
            return events switch
            {
                TimeEvents.Dawn => TimeChangedToDawn,
                TimeEvents.Noon => TimeChangedToNoon,
                TimeEvents.Afternoon => TimeChangedToAfternoon,
                TimeEvents.Dusk => TimeChangedToDusk,
                TimeEvents.Night => TimeChangedToNight,
                TimeEvents.Midnight => TimeChangedToMidnight,
                TimeEvents.Afternight => TimeChangedToAfternight,
                _ => null,
            };
        }
        
        public TimeEvents CalcStateFromLong(float lon)
        {
            return (lon, currentState) switch
            {
                (> -0, TimeEvents.Midnight) => TimeEvents.Dawn,
                (> 50, TimeEvents.Dawn) => TimeEvents.Noon,
                (< 20, TimeEvents.Noon) => TimeEvents.Dusk,
                (< 0, TimeEvents.Dusk) => TimeEvents.Night,
                (< -50, TimeEvents.Night) => TimeEvents.Midnight,
                _ => currentState
            };
        }
    }
}

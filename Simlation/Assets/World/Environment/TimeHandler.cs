using System;
using NaughtyAttributes;
using Player.GUI;
using UnityEngine;
using Utility;
using World.Environment.Lightning;

namespace World.Environment
{
    public class TimeHandler : MonoBehaviour, ILog
    {
        public static TimeHandler Instance;
        
        public Sun sun;
        [SerializeField]
        public DateTime date;
        
        [Range(1900, 2100)]
        public int year = 2022;

        [Range(1, 12)]
        public int month = 5;
        [Range(1, 31)]
        public int day = 5;
        
        [SerializeField]
        [Range(0, 24)]
        public int hour;

        [SerializeField]
        [Range(0, 60)]
        public int minutes;

        [SerializeField]
        [OnValueChanged("SetTimeSpeed")]
        [Range(1, 1200)]
        public float timeSpeed = 1;

        [Range(1, 64)]
        [SerializeField]
        public int frameSteps = 1;

        public bool realTime = false;
        
        public GUIController ui;
        
        public TimeEvents currentState;
        
        public DateTime LocalTime => localTime;
        
        public float TimeSpeed => timeSpeed;
        
        //Events
        public event EventHandler TimeChangedToDawn;
        public event EventHandler TimeChangedToNoon;
        public event EventHandler TimeChangedToAfternoon;
        public event EventHandler TimeChangedToDusk;
        public event EventHandler TimeChangedToNight;
        public event EventHandler TimeChangedToMidnight;
        public event EventHandler TimeChangedToAfternight;
        public event EventHandler<HourElapsedEventArgs> TimeHourElapsed;

        private DateTime localTime;
        private int frameStep;
        
        private void OnValidate()
        {
            try
            {
                var d = new DateTime(year,month,day,hour,minutes,0);
                localTime = d;
                sun.SetPosition();
                CallEventsFromTime(hour, hour);
            }
            catch(ArgumentOutOfRangeException e)
            {
                ILog.LE(LN, "bad date"+e.Message);
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
                ILog.L(LN, "TimeHandler instance already set!", LogType.Exception);
            }
        }

        private void Start()
        {
            //Settings
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
            //Events
            TimeChangedToDawn += OnDawn;
            TimeChangedToNoon += OnNoon;
            TimeChangedToAfternoon += OnAfternoon;
            TimeChangedToDusk += OnDusk;
            TimeChangedToNight += OnNight;
            TimeChangedToMidnight += OnMidnight;
            TimeChangedToAfternight += OnAfternight;
            
            //init call of time event 
            CallEventsFromTime((hour + 23) % 24, hour);
            
            //init event call
            CallEventFromStatus(currentState)?.Invoke(this, EventArgs.Empty);
            
            //set light source
            sun.SetLightSource(currentState)?.Invoke();
        }
        
        private void Update()
        {
            localTime = localTime.AddSeconds(timeSpeed * Time.deltaTime);
            if (frameStep == 0) 
            {
                //set time
                var oldHour = hour;
                hour = localTime.Hour;
                minutes = localTime.Minute;
                date = localTime.Date;
                //set sun
                sun.SetPosition();
                //set state
                CallEventsFromTime(oldHour, hour);
                ui.guiResourcesController.OnTimeChange(new GenEventArgs<string>(localTime.ToString("f")));
            }
            frameStep = (frameStep + 1) % frameSteps;
        }
        
        public string LN()
        {
            return "Time handler";
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

        public void SetTimeSpeed(float speed = -1) 
        {
            timeSpeed = (speed < 1)? timeSpeed : speed;
        }

        private void OnDawn(object sender, EventArgs e)
        {
            ILog.L(LN, "Its dawn!");
        }
        private void OnNoon(object sender, EventArgs e)
        {
            ILog.L(LN, "Its noon!");
        }
        private void OnAfternoon(object sender, EventArgs e)
        {
            ILog.L(LN, "Its after noon!");
        }
        private void OnDusk(object sender, EventArgs e)
        {
            ILog.L(LN, "Its dusk!");
        }
        private void OnNight(object sender, EventArgs e)
        {
            ILog.L(LN, "Its night!");
        }
        private void OnMidnight(object sender, EventArgs e)
        {
            ILog.L(LN, "Its midnight!");
        }
        
        private void OnAfternight(object sender, EventArgs e)
        {
            ILog.L(LN, "Its after night!");
        }

        public void CallEventsFromTime(int oldHourTime, int hourTime)
        {
            var oldState = currentState;
            currentState = (hourTime) switch
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
            if (hourTime > oldHourTime)
            {
                TimeHourElapsed?.Invoke(this, new HourElapsedEventArgs(hourTime));
            }
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
        
        [Serializable]
        public enum Months
        {
            January = 1, //to start at 1 for dateTime
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December,
        }
    }
    
    public class HourElapsedEventArgs : EventArgs
    {
        public int Hour { get; }
        public bool OverNoon { get; }
        
        public HourElapsedEventArgs(int overNoon)
        {
            Hour = overNoon;
            OverNoon = Hour >= 12;
        }
    }
}

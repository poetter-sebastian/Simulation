using System;
using UnityEngine;
using UnityEngine.Rendering;
using Utility;
using static World.Environment.TimeHandler;

namespace World.Environment.Lightning
{
    [ExecuteInEditMode]
    public class Sun : MonoBehaviour, ILog
    {
        [SerializeField]
        private float longitude;

        [SerializeField]
        private float latitude;

        public TimeHandler timeHandler;
        public Light sun;
        public Light moon;

        private void Start()
        {
            timeHandler.TimeChangedToDawn += OnDawn;
            timeHandler.TimeChangedToNight += OnNight;
        }

        /// <summary>
        /// Sets the location on earth
        /// </summary>
        /// <param name="lon">Longitude of the location</param>
        /// <param name="lat">Latitude of the location</param>
        public void SetLocation(float lon, float lat)
        {
            longitude = lon;
            latitude = lat;
        }
        
        /// <summary>
        /// Sets the position of the sun
        /// </summary>
        public void SetPosition()
        {
            var angles = new Vector3();
            SunPosition.CalculateSunPosition(timeHandler.LocalTime, latitude, longitude, out var azi, out var alt);
            angles.x = (float)alt * Mathf.Rad2Deg;
            angles.y = (float)azi * Mathf.Rad2Deg;
            
            //ILog.L(LN, angles);
            sun.transform.localRotation = Quaternion.Euler(angles);
            angles.x = (angles.x + 180) % 360;
            angles.y = (angles.y + 180) % 360;
            moon.transform.localRotation = Quaternion.Euler(angles);
            //light.intensity = Mathf.InverseLerp(-12, 0, angles.x);
        }

        public void EnableSun()
        {
            sun.gameObject.SetActive(true);
            moon.gameObject.SetActive(false);
        }

        public void EnableMoon()
        {
            sun.gameObject.SetActive(false);
            moon.gameObject.SetActive(true);
        }
        
        public Action SetLightSource(TimeEvents events)
        {
            return events switch
            {
                TimeEvents.Dawn => EnableSun,
                TimeEvents.Noon => EnableSun,
                TimeEvents.Afternoon => EnableSun,
                TimeEvents.Dusk => EnableSun,
                TimeEvents.Night => EnableMoon,
                TimeEvents.Midnight => EnableMoon,
                TimeEvents.Afternight => EnableMoon,
                _ => null,
            };
        }
        
        private void OnDawn(object sender, EventArgs e)
        {
            EnableSun();
        }

        private void OnNight(object sender, EventArgs e)
        {
            EnableMoon();
        }

        public string LN()
        {
            return "Sun";
        }
    }

    /// <summary>
    /// The following source came from this blog:
    /// http://guideving.blogspot.co.uk/2010/08/sun-position-in-c.html
    /// </summary>
    public static class SunPosition
    {
        private const double Deg2Rad = Math.PI / 180.0;
        //private const double Rad2Deg = 180.0 / Math.PI;
        
        /// <summary>
        /// Calculates the sun light. 
        /// CalcSunPosition calculates the suns "position" based on a
        /// given date and time in local time, latitude and longitude
        /// expressed in decimal degrees. It is based on the method
        /// found here:
        /// http://www.astro.uio.no/~bgranslo/aares/calculate.html
        /// The calculation is only satisfiable correct for dates in
        /// the range March 1 1900 to February 28 2100. 
        /// </summary>
        /// <param name="dateTime">Time and date in local time. </param>
        /// <param name="latitude">Latitude expressed in decimal degrees. </param>
        /// <param name="longitude">Longitude expressed in decimal degrees. </param>
        /// <param name="outAzimuth"></param>
        /// <param name="outAltitude"></param>
        public static void CalculateSunPosition(
            DateTime dateTime, double latitude, double longitude, out double outAzimuth, out double outAltitude)
        {
            // Convert to UTC  
            dateTime = dateTime.ToUniversalTime();

            // Number of days from J2000.0.  
            var julianDate = 367 * dateTime.Year -
                (int)((7.0 / 4.0) * (dateTime.Year +
                (int)((dateTime.Month + 9.0) / 12.0))) +
                (int)((275.0 * dateTime.Month) / 9.0) +
                dateTime.Day - 730531.5;

            var julianCenturies = julianDate / 36525.0;

            // Sidereal Time  
            var siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

            var siderealTimeUT = siderealTimeHours +
                (366.2422 / 365.2422) * dateTime.TimeOfDay.TotalHours;

            var siderealTime = siderealTimeUT * 15 + longitude;

            // Refine to number of days (fractional) to specific time.  
            julianDate += dateTime.TimeOfDay.TotalHours / 24.0;
            julianCenturies = julianDate / 36525.0;

            // Solar Coordinates  
            var meanLongitude = CorrectAngle(Deg2Rad *
                (280.466 + 36000.77 * julianCenturies));

            var meanAnomaly = CorrectAngle(Deg2Rad *
                                           (357.529 + 35999.05 * julianCenturies));

            var equationOfCenter = Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
                Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));

            var elipticalLongitude =
                CorrectAngle(meanLongitude + equationOfCenter);

            var obliquity = (23.439 - 0.013 * julianCenturies) * Deg2Rad;

            // Right Ascension  
            var rightAscension = Math.Atan2(Math.Cos(obliquity) * Math.Sin(elipticalLongitude), Math.Cos(elipticalLongitude));

            var declination = Math.Asin(Math.Sin(rightAscension) * Math.Sin(obliquity));

            // Horizontal Coordinates  
            var hourAngle = CorrectAngle(siderealTime * Deg2Rad) - rightAscension;

            if (hourAngle > Math.PI)
            {
                hourAngle -= 2 * Math.PI;
            }

            var altitude = Math.Asin(Math.Sin(latitude * Deg2Rad) *
                Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
                Math.Cos(declination) * Math.Cos(hourAngle));

            // Nominator and denominator for calculating Azimuth  
            // angle. Needed to test which quadrant the angle is in.  
            var aziNom = -Math.Sin(hourAngle);
            var aziDenom =
                Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
                Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

            var azimuth = Math.Atan(aziNom / aziDenom);

            if (aziDenom < 0) // In 2nd or 3rd quadrant  
            {
                azimuth += Math.PI;
            }
            else if (aziNom < 0) // In 4th quadrant  
            {
                azimuth += 2 * Math.PI;
            }
            outAltitude = altitude;
            outAzimuth = azimuth;
        }
        
        /// <summary>
        /// Corrects an angle. 
        /// </summary>
        /// <param name="angleInRadians">An angle expressed in radians. </param>
        /// <returns>An angle in the range 0 to 2*PI. </returns>
        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
            }

            if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians % (2 * Math.PI);
            }

            return angleInRadians;
        }
    }

}
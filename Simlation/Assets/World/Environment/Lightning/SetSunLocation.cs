using System.Collections;
using UnityEngine;
using Utility;

namespace World.Environment.Lightning 
{
  public class SetSunLocation : MonoBehaviour, ILog
  {
    [SerializeField]
    private Sun sun;

    public void Start()
    {      
      StartCoroutine(SetLocation());
    }

    public IEnumerator SetLocation()
    {
      if (!Input.location.isEnabledByUser)
      {
        ILog.L(LN, "location disabled by user");
        yield break;
      }
      Input.location.Start();

      while (Input.location.status == LocationServiceStatus.Initializing)
      {
        yield return new WaitForSeconds(0.5f);
      }

      if (Input.location.status == LocationServiceStatus.Failed)
      {
        ILog.LER(LN, "Unable to determine device location");
        yield break;
      }
      
      if(Input.location.status==LocationServiceStatus.Running)
      {
        var locInfo = Input.location.lastData;
        ILog.L(LN, "long="+locInfo.longitude+" lat=+"+locInfo.latitude);
        sun.SetLocation( locInfo.longitude, locInfo.latitude );
      }
      Input.location.Stop();
    }

    public string LN()
    {
      return "Sun location";
    }
  }
}

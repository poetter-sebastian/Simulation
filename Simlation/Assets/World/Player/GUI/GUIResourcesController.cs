using System.Collections;
using TMPro;
using UnityEngine;
using Utility;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIResourcesController : MonoBehaviour
    {
        public TextMeshProUGUI monValue;
        public TextMeshProUGUI qualValue;
        public TextMeshProUGUI timeDate;

        public void OnMoneyChange(GenEventArgs<string> e)
        {
            monValue.text = "" + e.Value + " ¤";
            StartCoroutine(ChangeTextBack());
        }
        
        public void OnQualityChange(GenEventArgs<string> e)
        {
            qualValue.text = "" + e.Value + " %";
        }
        
        public void OnTimeChange(GenEventArgs<string> e)
        {
            timeDate.text = "" + e.Value + "";
        }

        public void PosValues()
        {
            monValue.color = new Color(0, 0.5f, 0);
        }
        
        public void NegValues()
        {
            monValue.color = new Color(0.5f, 0, 0);
        }

        public void NoMoney()
        {
            monValue.color = new Color(1f, 0, 0);
            StartCoroutine(FadeText());
        }
        
        private IEnumerator ChangeTextBack()
        {
            yield return new WaitForSeconds(0.5f);
            monValue.color = new Color(0, 0, 0);
            yield return null;
        }
        
        private IEnumerator FadeText()
        {
            for (var i = 0; i < 4; i++)
            {
                monValue.color = new Color(1f, 0, 0);
                yield return new WaitForSeconds(0.2f);
                monValue.color = new Color(0, 0, 0);
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using Utility;
using World.Agents;

namespace Player.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GUIViewerController : PopupBehaviour
    {
        public TextMeshProUGUI nameValue;
        public TextMeshProUGUI waterValue;
        public TextMeshProUGUI co2Value;
        public TextMeshProUGUI o2Value;
        public TextMeshProUGUI diseaseValue;

        public event EventHandler<GenEventArgs<TreeAgent>> treeCutClicked;
        public event EventHandler foundIllTree;
        
        private TreeAgent tree;

        public void OpenViewer(TreeAgent treeAgent)
        {
            gameObject.SetActive(true);
            nameValue.text = new LocalizedString("Trees", treeAgent.species).GetLocalizedString();
            waterValue.text = "" + treeAgent.waterConsumption.ToString("0.00") + " "+new LocalizedString("Units", "LitersPerDay").GetLocalizedString();
            co2Value.text = "" + treeAgent.co2Modifier.ToString("0.00") + " "+new LocalizedString("Units", "GramPerDay").GetLocalizedString();
            o2Value.text = "" + treeAgent.o2Modifier.ToString("0.00") + " "+new LocalizedString("Units", "KiloGramPerDay").GetLocalizedString();
            if (treeAgent.diseases.Count > 0)
            {
                diseaseValue.text = new LocalizedString("TreeDiseases", treeAgent.diseases[0].name).GetLocalizedString();
                foundIllTree?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                diseaseValue.text = new LocalizedString("TreeDiseases", "Healthy").GetLocalizedString();
            }
            if (tree != null)
            {
                ChangeMat(tree.gameObject.transform, true);
            }
            tree = treeAgent;
            ChangeMat(treeAgent.gameObject.transform);
        }

        public void ResetColor()
        {
            ChangeMat(tree.gameObject.transform, true);
        }
        
        public void ChangeMat(Transform treeTrans, bool ret = false)
        {
            for(var i = 0; i < treeTrans.childCount;i++)
            {
                foreach (var r in treeTrans.transform.GetChild(i).GetComponents<Renderer>())
                {
                    foreach (var m in r.materials)
                    {
                        m.color = ret ? Color.white : Color.magenta;
                    }
                }
            }
        }

        public void CutTreeButtonPressed()
        {
            treeCutClicked?.Invoke(this, new GenEventArgs<TreeAgent>(tree));
        }
    }
}
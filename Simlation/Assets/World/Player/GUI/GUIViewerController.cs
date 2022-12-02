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
            if (treeAgent.diseases.Length > 0)
            {
                diseaseValue.text = new LocalizedString("TreeDiseases", treeAgent.diseases[0]).GetLocalizedString();
                foundIllTree?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                diseaseValue.text = new LocalizedString("TreeDiseases", "Healthy").GetLocalizedString();
            }
            tree = treeAgent;
        }

        public void CutTreeButtonPressed()
        {
            treeCutClicked?.Invoke(this, new GenEventArgs<TreeAgent>(tree));
        }
    }
}
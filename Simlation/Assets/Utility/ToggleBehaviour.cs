using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviour : ToggleGroup
{
    /// <summary>
    /// Because all the Toggles have registered themselves in the OnEnabled, Start should check to
    /// make sure at least one Toggle is active in groups that do not AllowSwitchOff
    /// </summary>
    protected override void Start()
    {
        EnsureValidState();
    }

    protected override void OnEnable()
    {
        EnsureValidState();
    }
    
    /// <summary>
    /// Ensure that the toggle group still has a valid state. This is only relevant when a ToggleGroup is Started
    /// or a Toggle has been deleted from the group.
    /// </summary>
    public new void EnsureValidState()
    {
        IEnumerable<Toggle> activeToggles = ActiveToggles();

        if (activeToggles.Count() > 1)
        {
            var firstActive = GetFirstActiveToggle();

            foreach (Toggle toggle in activeToggles)
            {
                if (toggle == firstActive)
                {
                    continue;
                }
                toggle.isOn = false;
            }
        }
    }
}

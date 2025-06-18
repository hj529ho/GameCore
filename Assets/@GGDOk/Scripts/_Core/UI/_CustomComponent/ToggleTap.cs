using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UI
{
    public class ToggleTap : MonoBehaviour
    {
        public Toggle tabContent;
        public ToggleGroup toggleGroup;

        [HideInInspector] public int tapCount;
        [HideInInspector] public List<UnityEvent<bool>> eventsList = new ();
        // public ToggleGroup toggleGroup;

        private void Start()
        {
            // toggleGroup.toggles
        }
    }

}

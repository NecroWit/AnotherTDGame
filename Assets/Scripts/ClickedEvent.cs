using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace AnotherDTGame
{
    public class ClickedEvent : MonoBehaviour
    {
        public UnityEvent onClicked;

        private void OnMouseDown()
        {
            onClicked?.Invoke();
        }
    }
}

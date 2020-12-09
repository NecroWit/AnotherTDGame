using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnotherDTGame
{
    public class HealthBar : MonoBehaviour
    {
        private float startScale;

        private void Awake()
        {
            startScale = gameObject.transform.localScale.x;
        }

        public void ChangeHealth(float percentage)
        {
            if (percentage >= 0)
            {
                Vector3 localScale = transform.localScale;

                transform.localScale = new Vector3(
                    localScale.x * percentage,
                    localScale.y,
                    localScale.z);
            }

            Color newColor = new Color(0f, 1f, 0.08f, 0.75f);

            if (percentage > 0.7)
            {
                newColor = new Color(0f, 1f, 0.08f, 0.75f);
            }
            else if (percentage <= 0.7 && percentage > 0.4)
            {
                newColor = Color.yellow;
            }
            else if (percentage <= 0.4)
            {
                newColor = Color.red;
            }

            Renderer barRenderer = GetComponent<Renderer>();
            if (barRenderer != null)
            {
                barRenderer.material.SetColor("_Color", newColor);
            }
        }
    }
}

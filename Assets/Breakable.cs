using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Breakable : MonoBehaviour
    {
        public Color faultColor;

        public SpriteRenderer mySprite;

        public float SecondsToBreak = 1f;
        
        public float SecondsToFix = 0.5f;

        public bool broken { get; private set; } = false;
        
        private Color originalColor;
        
        
        public void Break()
        {
            broken = true;
        }
        
        public bool BeingFixed { get; set; }
        

        private void Start()
        {
            originalColor = mySprite.color;
            BreakPercentage = 0;
        }

        public float BreakPercentage
        {
            get;
            private set;
        }
        
        private void Update()
        {
            if (GameManager.instance.IsGameStarted)
            {
                if (BeingFixed)
                {
                    BreakPercentage -= 1 / SecondsToFix * Time.deltaTime;
                }

                if (broken && !BeingFixed)
                {
                    BreakPercentage += 1 / SecondsToBreak * Time.deltaTime;
                }

                BreakPercentage = Mathf.Clamp01(BreakPercentage);

                if (BreakPercentage <= 0)
                {
                    broken = false;
                }

                if (BreakPercentage >= 1)
                {
                    GameManager.instance.GameOver();
                }

                mySprite.color = Color.Lerp(originalColor, faultColor, BreakPercentage);
            }
        }

        private void OnMouseDown()
        {
            BeingFixed = true;
        }

        private void OnMouseUp()
        {
            BeingFixed = false;
        }
    }
}
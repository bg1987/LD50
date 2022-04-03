using System;
using MoreMountains.Feedbacks;
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

        public BreakAway breakAway;

        public MMF_Player player;
        public MMWiggle wiggle;
        public float wiggleAmpMin = 0.01f;
        public float wiggleAmpMax = 0.2f;
        private float effectsTimer = 1f;
        
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

        public float DifficultyAdjustedSecondsToFix => Mathf.Lerp(SecondsToFix, SecondsToBreak, GameManager.instance.DifficultyModifier);

        private void Update()
        {
            if (GameManager.instance.IsGameStarted)
            {
                if (BeingFixed)
                {
                    BreakPercentage -= 1 / DifficultyAdjustedSecondsToFix * Time.deltaTime;
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

                PlayEffects();

                if (BreakPercentage >= 1)
                {
                    breakAway.Break();
                    GameManager.instance.GameOver();
                }

                mySprite.color = Color.Lerp(originalColor, faultColor, BreakPercentage);
            }
        }

        private void PlayEffects()
        {
            effectsTimer += Time.deltaTime;
            if (effectsTimer > 0.5f && BreakPercentage > 0.5f)
            {
                var wiggleAmp = Mathf.Lerp(wiggleAmpMin, wiggleAmpMax, BreakPercentage);
                wiggle.PositionWiggleProperties.AmplitudeMin = Vector3.one * wiggleAmp;
                player.PlayFeedbacks();
            }

            if (effectsTimer > 0.5f)
            {
                effectsTimer = 0f;
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
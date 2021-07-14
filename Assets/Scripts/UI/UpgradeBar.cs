using System;
using Ship;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UpgradeBar : MonoBehaviour
    {
        private int _prevLevelCost;
        private int _currentLevelCost;

        private LavaThrower _shipLavaThrower;
        
        [SerializeField] private Slider slider = null;

        // Start is called before the first frame update
        private void Start()
        {
            Reassign();

            var spaceShip = GameObject.FindGameObjectWithTag("Space Ship");
            _shipLavaThrower = spaceShip.GetComponent<LavaThrower>();
        }


        // Update is called once per frame
        private void Update()
        {
            if (GameDataUtils.CurrentLevel < 7 && GameDataUtils.CurrentScore > _currentLevelCost)
            {
                GameDataUtils.CurrentScore -= _currentLevelCost;
                _shipLavaThrower.LevelUp();
                Reassign();
            }

            if (GameDataUtils.CurrentLevel > 0 && GameDataUtils.CurrentScore < 0)
            {
                GameDataUtils.CurrentScore += _prevLevelCost;
                _shipLavaThrower.LevelDown();
                Reassign();
            }

            // ReSharper disable once PossibleLossOfFraction
            //https://forum.unity.com/threads/how-to-change-the-colour-of-a-slider-based-on-its-current-value.363686/
            slider.value = GameDataUtils.CurrentScore * 100 / _currentLevelCost;
            slider.image.color = Color.Lerp(Color.red, Color.green, slider.value / 100f);
        }

        private void Reassign()
        {
            _prevLevelCost = (int) (Math.Pow(1.5, GameDataUtils.CurrentLevel - 1) * 20);
            _currentLevelCost = (int) (Math.Pow(1.5, GameDataUtils.CurrentLevel) * 20);
        }
    }
}
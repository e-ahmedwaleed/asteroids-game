using System;
using Ship;
using UnityEngine;
using UnityEngine.UI;

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
            if (Globals.CurrentLevel < 7 && Globals.UpgradeScore > _currentLevelCost)
            {
                Globals.UpgradeScore -= _currentLevelCost;
                _shipLavaThrower.LevelUp();
                Reassign();
            }

            if (Globals.CurrentLevel > 0 && Globals.UpgradeScore < 0)
            {
                Globals.UpgradeScore += _prevLevelCost;
                _shipLavaThrower.LevelDown();
                Reassign();
            }

            // ReSharper disable once PossibleLossOfFraction
            //https://forum.unity.com/threads/how-to-change-the-colour-of-a-slider-based-on-its-current-value.363686/
            slider.value = Globals.UpgradeScore * 100 / _currentLevelCost;
            slider.image.color = Color.Lerp(Color.red, Color.green, slider.value / 100f);
        }

        private void Reassign()
        {
            _prevLevelCost = (int) (Math.Pow(1.5, Globals.CurrentLevel - 1) * 20);
            _currentLevelCost = (int) (Math.Pow(1.5, Globals.CurrentLevel) * 20);
        }
    }
}
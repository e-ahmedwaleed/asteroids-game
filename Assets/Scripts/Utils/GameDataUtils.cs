using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    public static class GameDataUtils
    {
        private static readonly GameDataConfigurations Confg = new GameDataConfigurations();

        public static int CurrentLevel
        {
            get
            {
                var value = PlayerPrefs.GetInt("Current Level", int.MinValue);
                return value != int.MinValue ? value : 2;
            }
            set
            {
                PlayerPrefs.SetInt("Current Level", value);
                PlayerPrefs.Save();
            }
        }

        public static int CurrentScore
        {
            get
            {
                var value = PlayerPrefs.GetInt("Current Score", int.MinValue);
                return value != int.MinValue ? value : 1;
            }
            set
            {
                PlayerPrefs.SetInt("Current Score", value);
                PlayerPrefs.Save();
            }
        }

        public static bool MouseEnabled => Confg._mouseEnabled;

        public static void Initialize()
        {
            Confg.Update();
        }

        private class GameDataConfigurations
        {
            private const string FileName = "GameConfig.ini";
            
            public bool _mouseEnabled;

            public GameDataConfigurations()
            {
                _mouseEnabled = false;
            }

            public void Update()
            {
                StreamReader input = null;
                try
                {
                    input = File.OpenText(Path.Combine(Application.streamingAssetsPath, FileName));

                    _mouseEnabled = bool.Parse(input.ReadLine() ?? throw new Exception("Null Config"));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    input?.Close();
                }
            }
        }
    }
}
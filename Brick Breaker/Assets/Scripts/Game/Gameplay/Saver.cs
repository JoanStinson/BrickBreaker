﻿using System.IO;
using UnityEngine;

namespace JGM.Game
{
    public class Saver : MonoBehaviour
    {
        public static Saver Instance;

        private const string KEY_HAS_SAVE = "HasSave";
        private const string KEY_TRUE = "t";
        private const string KEY_FALSE = "f";

        private void Awake()
        {
            Instance = this;
        }

        // after game over, HasSave will be equal to false
        public void Save(bool gameIsOver)
        {
            if (gameIsOver)
                PlayerPrefs.SetString(KEY_HAS_SAVE, KEY_FALSE);
            else
            {
                SavedGame savedGame = new SavedGame
                {
                    finalScore = BrickSpawner.Instance.m_LevelOfFinalBrick,
                    xPositionOfBallLauncher = BallLauncherView.Instance.transform.position.x,
                    BrickRowYPos = new System.Collections.Generic.List<float>()
                    {

                    }
                };

                string data = JsonUtility.ToJson(savedGame);
                Debug.Log(data);
            }
        }

        public bool HasSave()
        {
            return PlayerPrefs.GetString(KEY_HAS_SAVE, KEY_FALSE) == KEY_FALSE ? false : true;
        }
    }
}
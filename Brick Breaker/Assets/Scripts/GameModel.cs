using UnityEngine;

namespace JGM.Game
{
    public class GameModel
    {
        public int HighScore
        {
            get => PlayerPrefs.GetInt("best_score", 0);
            set
            {
                PlayerPrefs.SetInt("best_score", value);
                PlayerPrefs.Save();
            }
        }
        public int Score { get; set; } = 0;
        public int LevelOfFinalBrick { get; set; } = 1;
        public int ScoreMultiplier { get; set; } = 1;
    }
}
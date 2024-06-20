using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class LeaderboardsView : ScreenView
    {
        [SerializeField] private Button m_playAgainButton;
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private Button m_quitButton;
        [SerializeField] private ScrollRect m_scrollRect;
        [SerializeField] private LeaderboardCellView m_cellPrefab;

        private GameView m_gameView;
        private List<LeaderboardEntry> m_entries;
        private List<LeaderboardCellView> m_cellsPool;
        private int m_totalEntries = 100;
        private int m_visibleCellsCount = 10;
        private float m_cellHeight;

        private int m_firstVisibleIndex = 0;

        public override void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_playAgainButton.onClick.AddListener(OnClickPlayAgainButton);
            m_mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
            m_quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnClickPlayAgainButton()
        {
            m_gameView.OnClickPlayAgainButton();
        }

        private void OnClickMainMenuButton()
        {
            m_gameView.OnClickMainMenuButton();
        }

        private void OnClickQuitButton()
        {
            m_gameView.OnClickQuitButton();
        }

        public override void Show()
        {
            base.Show();
            InitializeLeaderboard();
            UpdateCells();
        }

        private void InitializeLeaderboard()
        {
            m_entries = GenerateRandomEntries(m_totalEntries - 1);
            m_cellsPool = new List<LeaderboardCellView>();
            m_cellHeight = ((RectTransform)m_cellPrefab.transform).rect.height;

            // Calculate user's score
            int userScore = m_gameView.Model.Score * m_gameView.Model.ScoreMultiplier;
            // Create user's entry
            LeaderboardEntry userEntry = new LeaderboardEntry { Name = "User", Score = userScore };
            // Add user's entry to the list
            m_entries.Add(userEntry);

            // Sort entries by score in descending order
            m_entries.Sort((a, b) => b.Score.CompareTo(a.Score));

            // Find the index of the user's entry in the sorted list
            int userIndex = m_entries.IndexOf(userEntry);

            // Adjust user's index so it's within bounds
            userIndex = Mathf.Clamp(userIndex, 0, m_totalEntries - 1);

            // Set initial anchored position to center on the user's cell
            float targetPosY = -userIndex * m_cellHeight;
            m_scrollRect.content.anchoredPosition = new Vector2(m_scrollRect.content.anchoredPosition.x, targetPosY);

            // Create cell prefabs
            for (int i = 0; i < m_visibleCellsCount; i++)
            {
                var cell = Instantiate(m_cellPrefab, m_scrollRect.content);
                m_cellsPool.Add(cell);
            }

            m_scrollRect.onValueChanged.AddListener(OnScrollChanged);
            m_scrollRect.content.sizeDelta = new Vector2(m_scrollRect.content.sizeDelta.x, m_cellHeight * m_totalEntries);
        }

        private List<LeaderboardEntry> GenerateRandomEntries(int count)
        {
            List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
            System.Random rand = new System.Random();

            for (int i = 0; i < count; i++)
            {
                string name = "User" + rand.Next(1000, 9999);
                int score = rand.Next(0, 51); // Random score from 0 to 50
                entries.Add(new LeaderboardEntry { Name = name, Score = score });
            }

            return entries;
        }

        private void OnScrollChanged(Vector2 position)
        {
            int newFirstVisibleIndex = Mathf.FloorToInt(m_scrollRect.content.anchoredPosition.y / m_cellHeight);
            if (newFirstVisibleIndex != m_firstVisibleIndex)
            {
                UpdateCells();
            }
        }

        private void UpdateCells()
        {
            int newFirstVisibleIndex = Mathf.FloorToInt(m_scrollRect.content.anchoredPosition.y / m_cellHeight);

            for (int i = 0; i < m_visibleCellsCount; i++)
            {
                int entryIndex = newFirstVisibleIndex + i;
                if (entryIndex >= 0 && entryIndex < m_entries.Count)
                {
                    // Check if it's the user's cell
                    bool isUserCell = entryIndex == m_entries.Count - 1; // Last entry is the user's cell

                    // Format text with blue color if it's the user's cell
                    string textColor = isUserCell ? "blue" : "black";
                    string text = $"<color={textColor}>Position {entryIndex + 1} - {m_entries[entryIndex].Name} - {m_entries[entryIndex].Score}</color>";
                    m_cellsPool[i].SetText(text);
                    m_cellsPool[i].transform.localPosition = new Vector3(m_cellsPool[i].transform.localPosition.x, -entryIndex * m_cellHeight, 0);
                }
            }

            m_firstVisibleIndex = newFirstVisibleIndex;
        }
    }
}

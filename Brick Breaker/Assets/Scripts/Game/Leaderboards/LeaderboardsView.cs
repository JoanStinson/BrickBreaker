using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            StartCoroutine(DefaultScroll());
        }

        private IEnumerator DefaultScroll()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            m_firstVisibleIndex = 0;
            UpdateCells();
        }

        private void InitializeLeaderboard()
        {
            m_entries = GenerateRandomEntries(m_totalEntries - 1);

            int userScore = m_gameView.Model.Score * m_gameView.Model.ScoreMultiplier;
            LeaderboardEntry userEntry = new LeaderboardEntry { Name = "User", Score = userScore };
            m_entries.Add(userEntry);
            m_entries.Sort((a, b) => b.Score.CompareTo(a.Score));

            int userIndex = m_entries.IndexOf(userEntry);
            userIndex = Mathf.Clamp(userIndex, 0, m_totalEntries - 1);
            m_cellHeight = ((RectTransform)m_cellPrefab.transform).rect.height;
            float targetPosY = Mathf.Max(-userIndex * m_cellHeight + m_scrollRect.viewport.rect.height / 2 - m_cellHeight / 2, 0);
            //m_scrollRect.content.localPosition = new Vector2(m_scrollRect.content.localPosition.x, 21305.89f);

            m_cellsPool = new List<LeaderboardCellView>();
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
                int score = rand.Next(0, 51);
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
                    bool isUserCell = (entryIndex == m_entries.Count - 1);
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

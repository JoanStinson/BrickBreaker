using System;
using System.Collections.Generic;
using UnityEngine;

namespace JGM.Game
{
    public class BrickRowSpawnerView : MonoBehaviour
    {
        public Action OnBrickTouchedFloor { get; set; }
        public Action OnPickupExtraBall { get; set; }

        public static BrickRowSpawnerView Instance;
        public int m_LevelOfFinalBrick;
        public float m_SpawningTopPosition = 3.65f;
        public float m_SpawningRowDistance = 0.64f;

        [SerializeField] private int m_rowsToSpawn = 7;
        [SerializeField] private BrickRowView m_brickRowPrefab;

        private List<BrickRowView> m_brickRowInstances;

        public void Initialize()
        {
            Instance = this;
            m_LevelOfFinalBrick = PlayerPrefs.GetInt("level_of_final_brick", 1);
            m_brickRowInstances = new List<BrickRowView>();
            for (int i = 0; i < m_rowsToSpawn; i++)
            {
                var brickRowInstance = Instantiate(m_brickRowPrefab, transform.parent, false);
                brickRowInstance.Initialize();
                brickRowInstance.OnBrickRowTouchedFloor += OnBrickRowTouchedFloor;
                brickRowInstance.OnPickupBallFromRow += OnPickupRowFromBall;
                m_brickRowInstances.Add(brickRowInstance);
                m_brickRowInstances[m_brickRowInstances.Count - 1].transform.localPosition = new Vector3(0, m_SpawningTopPosition, 0);
                m_brickRowInstances[m_brickRowInstances.Count - 1].gameObject.SetActive(false);
            }
            SpawnBricks();
        }

        private void OnBrickRowTouchedFloor()
        {
            OnBrickTouchedFloor?.Invoke();
        }

        private void OnPickupRowFromBall()
        {
            OnPickupExtraBall?.Invoke();
        }

        public void SpawnBricks()
        {
            foreach (var row in m_brickRowInstances)
            {
                if (!row.gameObject.activeInHierarchy)
                {
                    row.gameObject.SetActive(true);
                    break;
                }
            }

            m_LevelOfFinalBrick++;
        }

        public void MoveDownRows()
        {
            foreach (var row in m_brickRowInstances)
            {
                if (row.gameObject.activeInHierarchy)
                {
                    row.MoveDown(m_SpawningRowDistance);
                }
            }
        }

        public void HideAllRows()
        {
            foreach (var row in m_brickRowInstances)
            {
                row.gameObject.SetActive(false);
            }
        }
    }
}
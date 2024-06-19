using System.Collections.Generic;
using UnityEngine;

namespace JGM.Game
{
    public class BrickRowSpawnerView : MonoBehaviour
    {
        public static BrickRowSpawnerView Instance;
        public int m_LevelOfFinalBrick;
        public float m_SpawningTopPosition = 3.65f;
        public float m_SpawningRowDistance = 0.64f;

        [SerializeField] private int m_rowsToSpawn = 7;
        [SerializeField] private BrickRowView m_brickRowPrefab;

        private List<BrickRowView> m_brickRowInstances;

        private void Awake()
        {
            Instance = this;
            m_LevelOfFinalBrick = PlayerPrefs.GetInt("level_of_final_brick", 1);
            m_brickRowInstances = new List<BrickRowView>();
            for (int i = 0; i < m_rowsToSpawn; i++)
            {
                m_brickRowInstances.Add(Instantiate(m_brickRowPrefab, transform.parent, false));
                m_brickRowInstances[m_brickRowInstances.Count - 1].transform.localPosition = new Vector3(0, m_SpawningTopPosition, 0);
                m_brickRowInstances[m_brickRowInstances.Count - 1].gameObject.SetActive(false);
            }
        }

        public void SpawnNewRows()
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
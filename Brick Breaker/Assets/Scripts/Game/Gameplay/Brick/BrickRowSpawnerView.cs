using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class BrickRowSpawnerView : MonoBehaviour
    {
        public Action OnBrickTouchedFloor { get; set; }
        public Action OnPickupExtraBall { get; set; }

        [Inject] private BrickRowView.Factory m_brickRowFactory;
        [SerializeField] private float m_spawningRowDistance = 0.64f;
        [SerializeField] private float m_spawningTopPosition = 3.65f;
        [SerializeField] private int m_rowsToSpawn = 7;

        private GameModel m_gameModel;
        private List<BrickRowView> m_brickRowInstances;

        public void Initialize(GameModel gameModel)
        {
            m_gameModel = gameModel;
            m_brickRowInstances = new List<BrickRowView>();

            for (int i = 0; i < m_rowsToSpawn; i++)
            {
                var brickRowInstance = m_brickRowFactory.Create();
                brickRowInstance.transform.SetParent(transform.parent, false);
                brickRowInstance.Initialize(m_spawningRowDistance, m_spawningTopPosition, gameModel);
                brickRowInstance.OnBrickRowTouchedFloor += OnBrickRowTouchedFloor;
                brickRowInstance.OnPickupBallFromRow += OnPickupRowFromBall;
                m_brickRowInstances.Add(brickRowInstance);
                m_brickRowInstances[m_brickRowInstances.Count - 1].transform.localPosition = new Vector3(0, m_spawningTopPosition, 0);
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
            foreach (var instance in m_brickRowInstances)
            {
                if (!instance.gameObject.activeInHierarchy)
                {
                    instance.gameObject.SetActive(true);
                    break;
                }
            }

            m_gameModel.LevelOfFinalBrick++;
        }

        public void MoveDownRows()
        {
            foreach (var instance in m_brickRowInstances)
            {
                if (instance.gameObject.activeInHierarchy)
                {
                    instance.MoveDown(m_spawningRowDistance);
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
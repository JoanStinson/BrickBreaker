using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MainScenePrefabInstaller : MonoInstaller
    {
        [SerializeField] private BallView m_ballViewPrefab;
        [SerializeField] private BrickRowView m_brickRowViewPrefab;
        [SerializeField] private LeaderboardCellView m_leaderboardCellView;

        public override void InstallBindings()
        {
            Container.BindFactory<BallView, BallView.Factory>().FromComponentInNewPrefab(m_ballViewPrefab);
            Container.BindFactory<BrickRowView, BrickRowView.Factory>().FromComponentInNewPrefab(m_brickRowViewPrefab);
            Container.BindFactory<LeaderboardCellView, LeaderboardCellView.Factory>().FromComponentInNewPrefab(m_leaderboardCellView);
        }
    }
}

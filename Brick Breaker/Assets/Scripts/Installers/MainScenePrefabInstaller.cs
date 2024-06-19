using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MainScenePrefabInstaller : MonoInstaller
    {
        [SerializeField] private PieceView m_pieceViewPrefab;
        [SerializeField] private BallView m_ballViewPrefab;
        [SerializeField] private BrickRowView m_brickRowViewPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<PieceView, PieceView.Factory>().FromComponentInNewPrefab(m_pieceViewPrefab);
            Container.BindFactory<BallView, BallView.Factory>().FromComponentInNewPrefab(m_ballViewPrefab);
            Container.BindFactory<BrickRowView, BrickRowView.Factory>().FromComponentInNewPrefab(m_brickRowViewPrefab);
        }
    }
}

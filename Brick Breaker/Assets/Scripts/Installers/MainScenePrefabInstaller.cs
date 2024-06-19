using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MainScenePrefabInstaller : MonoInstaller
    {
        [SerializeField] private PieceView m_pieceViewPrefab;
        [SerializeField] private BallView m_ballViewPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<PieceView, PieceView.Factory>().FromComponentInNewPrefab(m_pieceViewPrefab);
            Container.BindFactory<BallView, BallView.Factory>().FromComponentInNewPrefab(m_ballViewPrefab);
        }
    }
}

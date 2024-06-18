using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MainScenePrefabInstaller : MonoInstaller
    {
        [SerializeField] 
        private PieceView m_pieceViewPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<PieceView, PieceView.Factory>().FromComponentInNewPrefab(m_pieceViewPrefab);
        }
    }
}

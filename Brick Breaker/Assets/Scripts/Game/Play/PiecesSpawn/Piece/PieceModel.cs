using UnityEngine;

namespace JGM.Game
{
    public class PieceModel
    {
        public int PlayerId { get; private set; }
        public GameSettings.PieceType PieceType { get; private set; }
        public Sprite Sprite { get; private set; }
        public float EnabledColorAlpha { get; private set; }
        public float DisabledColorAlpha { get; private set; }

        public PieceModel(int playerId, GameSettings.PieceType pieceType, Sprite sprite, float enabledColorAlpha, float disabledColorAlpha)
        {
            PlayerId = playerId;
            PieceType = pieceType;
            Sprite = sprite;
            EnabledColorAlpha = enabledColorAlpha;
            DisabledColorAlpha = disabledColorAlpha;
        }
    }
}

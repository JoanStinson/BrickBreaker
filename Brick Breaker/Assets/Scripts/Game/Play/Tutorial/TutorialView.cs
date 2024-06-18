using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JGM.Game
{
    public class TutorialView : MonoBehaviour
    {
        [Header("Tutorial")]
        [SerializeField] private Transform m_background;
        [SerializeField] private Transform m_leftHand;
        [SerializeField] private Transform m_rightHand;
        [SerializeField] private float m_tutorialDelay = 1;

        [Header("Hierarchy Dependencies")]
        [SerializeField] private Transform m_play;
        [SerializeField] private Transform m_top;
        [SerializeField] private Transform m_board;
        [SerializeField] private Transform m_leftPieces;
        [SerializeField] private Transform m_rightPieces;
        [SerializeField] private Transform m_message;

        public async void ShowTutorial(int playerTurnId)
        {
            await Task.Delay(TimeSpan.FromSeconds(m_tutorialDelay));
            gameObject.SetActive(true);
            m_background.SetParent(m_play);

            if (playerTurnId == 0)
            {
                ShowLeftHandTutorial();
            }
            else
            {
                ShowRightHandTutorial();
            }
        }

        private void ShowLeftHandTutorial()
        {
            m_leftHand.gameObject.SetActive(true);
            m_rightHand.gameObject.SetActive(false);

            m_top.SetSiblingIndex(0);
            m_rightPieces.SetSiblingIndex(1);
            m_background.SetSiblingIndex(2);
            m_board.SetSiblingIndex(3);
            transform.SetSiblingIndex(4);
            m_leftPieces.SetSiblingIndex(5);
            m_message.SetSiblingIndex(6);
        }

        private void ShowRightHandTutorial()
        {
            m_leftHand.gameObject.SetActive(false);
            m_rightHand.gameObject.SetActive(true);

            m_top.SetSiblingIndex(0);
            m_leftPieces.SetSiblingIndex(1);
            m_background.SetSiblingIndex(2);
            m_board.SetSiblingIndex(3);
            transform.SetSiblingIndex(4);
            m_rightPieces.SetSiblingIndex(5);
            m_message.SetSiblingIndex(6);
        }

        public void HideTutorial()
        {
            gameObject.SetActive(false);
            m_background.SetParent(transform);
            m_background.SetAsFirstSibling();
            RestoreHierarchyOrder();
        }

        private void RestoreHierarchyOrder()
        {
            m_top.SetSiblingIndex(0);
            m_board.SetSiblingIndex(1);
            m_leftPieces.SetSiblingIndex(2);
            m_rightPieces.SetSiblingIndex(3);
            m_message.SetSiblingIndex(4);
            transform.SetSiblingIndex(5);
        }
    }
}

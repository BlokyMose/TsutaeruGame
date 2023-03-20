using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsutaeru
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        WordPanel wordPanel;

        [SerializeField]
        HPController hpController;

        [SerializeField]
        DialogueController dialogueController;

        [SerializeField]
        CharactersPanel charactersPanel;

        void Awake()
        {
        }

        public void SetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.InGame:
                    SetInGame();
                    break;
                case GameState.OutGame:
                    SetOutGame();
                    break;
                default:
                    break;
            }
        }

        [Button]
        public void SetInGame()
        {
            wordPanel.SetText("test");
            hpController.RestoreAllHealth();
            hpController.Show();
            dialogueController.HideAll();
            charactersPanel.Shrink();
        }

        [Button]
        public void SetOutGame()
        {
            wordPanel.Hide();
            hpController.Hide();
            charactersPanel.Enlarge();
        }

    }

}
using Encore.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUtility;

namespace Tsutaeru
{
    public class GameController : MonoBehaviour
    {
        [Serializable]
        public class DialogueLine
        {
            [HorizontalGroup(.3f)]
            [SerializeField, LabelWidth(.1f)]
            CharacterName characterName;

            [HorizontalGroup]
            [SerializeField, LabelWidth(.1f)]
            string line;

            [SerializeField]
            Sprite sprite;

            public CharacterName CharacterName { get => characterName; }
            public string Line { get => line; }
            public Sprite Sprite { get => sprite; }
        }

        [Serializable]
        public class Stage
        {
            [SerializeField]
            string stageName;

            [SerializeField]
            string targetWord;

            [SerializeField]
            List<DialogueLine> dialogue = new();

            public string StageName { get => stageName; }
            public string TargetWord { get => targetWord; }
            public List<DialogueLine> Dialogue { get => dialogue; }
        }

        [Title("Data")]
        [SerializeField]
        List<Stage> stages= new();

        [SerializeField]
        Stage gameOverStage;

        [SerializeField]
        CharacterSpriteLib boySprites;

        [SerializeField]
        CharacterSpriteLib girlSprites;

        [Title("Components")]
        [SerializeField]
        TsuController tsu;

        [SerializeField]
        WordPanel wordPanel;

        [SerializeField]
        HPController hpController;

        [SerializeField]
        DialogueController dialogueController;

        [SerializeField]
        CharactersPanel charactersPanel;

        [SerializeField]
        AnoSonoSpawner anoSonoSpawner;

        [SerializeField]
        GameObject gameOverScreen;

        [Title("Buttons")]
        [SerializeField]
        Image nextDialogueBut;

        [SerializeField]
        Image gameOverBut;


        Stage currentStage;
        int currentStageIndex = 0;
        int currentDialogueLineIndex = 0;

        void Awake()
        {
            wordPanel.Init();
            hpController.Init();
            dialogueController.Init();
            charactersPanel.Init();
            anoSonoSpawner.Init();
            tsu.Init();

            nextDialogueBut.AddEventTrigger(NextDialogue);
            gameOverBut.AddEventTrigger(LoadMainMenu);
            gameOverBut.gameObject.SetActive(false);
            gameOverScreen.SetActive(false);

            tsu.OnDamaged.AddListener(ReduceHP);
            hpController.onDie.AddListener(GameOver);

            PlayStage(0);
        }

        public void NextStage()
        {
            currentStageIndex++;
            PlayStage(currentStageIndex);
        }

        public void ReduceHP()
        {
            hpController.ReduceHP();
            dialogueController.SetSpriteGirl(girlSprites.Nervous);
        }

        public void GameOver()
        {
            PlayStage(gameOverStage);
        }

        [Title("Buttons")]
        [Button]
        public void PlayStage(int stageIndex)
        {
            currentStageIndex = stageIndex;
            PlayStage(stages[currentStageIndex]);
        }

        public void PlayStage(Stage stage)
        {
            currentStage = stage;
            currentDialogueLineIndex = 0;
            SetGameState(GameState.OutGame);
            PlayDialogue(currentDialogueLineIndex);
        }

        [Button]
        public void TestGameOver()
        {
            PlayStage(gameOverStage);
        }

        public void NextDialogue()
        {
            currentDialogueLineIndex++;
            PlayDialogue(currentDialogueLineIndex);

            if (currentStage == gameOverStage && currentDialogueLineIndex == gameOverStage.Dialogue.Count - 1)
            {
                gameOverBut.gameObject.SetActive(true);
                nextDialogueBut.gameObject.SetActive(false);
            }
        }

        public void PlayDialogue(int dialogueLineIndex)
        {
            currentDialogueLineIndex = dialogueLineIndex;

            if (currentDialogueLineIndex >= currentStage.Dialogue.Count)
            {
                SetGameState(GameState.InGame);
                dialogueController.SetSpriteBoy(boySprites.Nervous);
                dialogueController.SetSpriteGirl(girlSprites.Normal);
            }
            else
            {
                var line = currentStage.Dialogue[currentDialogueLineIndex];

                if (currentStage.Dialogue[currentDialogueLineIndex].CharacterName == CharacterName.Boy)
                    dialogueController.SetDialogueToBubbleBoy(line);
                else if (currentStage.Dialogue[currentDialogueLineIndex].CharacterName == CharacterName.Girl)
                    dialogueController.SetDialogueToBubbleGirl(line);
            }
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
            tsu.Show();

            wordPanel.SetText(currentStage.TargetWord);
            hpController.RestoreHP();
            hpController.Show();
            dialogueController.HideAll();
            charactersPanel.Shrink();
            anoSonoSpawner.Resume();
        }

        [Button]
        public void SetOutGame()
        {
            tsu.Hide();
            wordPanel.Hide();
            hpController.Hide();
            charactersPanel.Enlarge();
            anoSonoSpawner.Pause();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(Delay(3.2f));
            IEnumerator Delay(float delay)
            {
                Exit();
                gameOverScreen.SetActive(true);
                yield return new WaitForSeconds(delay);
                SceneManager.LoadScene("MainMenu");
            }
        }

        void Exit()
        {
            tsu.gameObject.SetActive(false);
        }

    }

}
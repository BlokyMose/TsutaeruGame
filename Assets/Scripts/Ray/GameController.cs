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
            Emotion emotion;

            public CharacterName CharacterName { get => characterName; }
            public string Line { get => line; }
            public Emotion Emotion { get => emotion; }
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
        string initialHiragana = "";

        [SerializeField]
        int spawnCorrectHiraganaEveryCount = 10;

        [SerializeField]
        List<Stage> stages= new();

        [SerializeField]
        Stage gameOverStage;

        [SerializeField]
        Stage winStage;

        [SerializeField]
        CharacterSpriteLib boySprites;

        [SerializeField]
        CharacterSpriteLib girlSprites;

        [Title("Components")]
        [SerializeField]
        TsuController tsu;

        [SerializeField]
        HiraganaSpawner hiraganaSpawner;

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

        [SerializeField]
        GameObject winScreen;

        [Title("Buttons")]
        [SerializeField]
        Image nextDialogueBut;

        [SerializeField]
        Image gameOverBut;


        Stage currentStage;
        int currentStageIndex = 0;
        int currentDialogueLineIndex = 0;
        string receivedHiraganas = "";
        int correctHiraganaCooldown = 0;

        void Awake()
        {
            wordPanel.Init();
            hpController.Init();
            dialogueController.Init();
            charactersPanel.Init();
            anoSonoSpawner.Init();
            tsu.Init();

            nextDialogueBut.AddEventTrigger(NextDialogue);
            gameOverBut.AddEventTrigger(() => { if (currentStage == gameOverStage) ShowGameOverAndLoadMainMenu(); else ShowWinScreenAndLoadMainMenu(); });
            gameOverBut.gameObject.SetActive(false);
            gameOverScreen.SetActive(false);
            winScreen.SetActive(false);

            tsu.OnHitHiragana += CheckHiragana;
            hpController.onDie.AddListener(GameOver);
            hiraganaSpawner.OnSpawn += CheckSpawnCorrectHiragana;

            dialogueController.SetSpriteBoy(boySprites.Get(Emotion.Normal));
            dialogueController.SetSpriteGirl(girlSprites.Get(Emotion.Normal));

            PlayStage(0);

        }

        void CheckSpawnCorrectHiragana()
        {
            correctHiraganaCooldown++;
            if (correctHiraganaCooldown >= spawnCorrectHiraganaEveryCount)
            {
                correctHiraganaCooldown = 0;
                hiraganaSpawner.Spawn(currentStage.TargetWord[receivedHiraganas.Length].ToString());
            }
        }


        void OnDestroy()
        {
            tsu.OnHitHiragana -= CheckHiragana;
            hiraganaSpawner.OnSpawn -= CheckSpawnCorrectHiragana;
        }

        void CheckHiragana(string hiragana)
        {
            if (currentStage.TargetWord[receivedHiraganas.Length].ToString() == hiragana)
            {
                receivedHiraganas += hiragana;
                wordPanel.SetTextColored(currentStage.TargetWord, receivedHiraganas.Length);
                tsu.AddHiragana(hiragana);

                if (currentStage.TargetWord == receivedHiraganas)
                    NextStage();
            }
            else
            {
                ReduceHP();
            }
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
            if (currentStageIndex == stages.Count)
            {
                Win();
            }
            else
            {
                PlayStage(stages[currentStageIndex]);
            }
        }

        public void Win()
        {
            PlayStage(winStage);
        }

        public void PlayStage(Stage stage)
        {
            hiraganaSpawner.DestroyAllHiraganas();
            tsu.ClearHiraganas();
            correctHiraganaCooldown = 0;
            currentStage = stage; 
            currentDialogueLineIndex = 0;
            receivedHiraganas = initialHiragana;
            SetGameState(GameState.OutGame);
            PlayDialogue(currentDialogueLineIndex);
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
            
            else if (currentStage == winStage && currentDialogueLineIndex == winStage.Dialogue.Count - 1)
            {
                gameOverBut.gameObject.SetActive(true);
                nextDialogueBut.gameObject.SetActive(false);
            }
        }

        public void PlayDialogue(int dialogueLineIndex)
        {
            currentDialogueLineIndex = dialogueLineIndex;

            dialogueController.SetSpriteBoy(boySprites.Normal);
            dialogueController.SetSpriteGirl(girlSprites.Normal);

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
                    dialogueController.SetDialogueToBubbleBoy(line.Line, boySprites.Get(line.Emotion));
                else if (currentStage.Dialogue[currentDialogueLineIndex].CharacterName == CharacterName.Girl)
                    dialogueController.SetDialogueToBubbleGirl(line.Line, girlSprites.Get(line.Emotion));
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
            hiraganaSpawner.IsPaused = false;
            tsu.Show();

            wordPanel.SetTextColored(currentStage.TargetWord,receivedHiraganas.Length);
            hpController.RestoreHP();
            hpController.Show();
            dialogueController.HideAll();
            charactersPanel.Shrink();
            anoSonoSpawner.Resume();
        }

        [Button]
        public void SetOutGame()
        {
            hiraganaSpawner.IsPaused = true;
            tsu.Hide();
            wordPanel.Hide();
            hpController.Hide();
            charactersPanel.Enlarge();
            anoSonoSpawner.Pause();
        }

        public void ShowGameOverAndLoadMainMenu()
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
        
        public void ShowWinScreenAndLoadMainMenu()
        {
            StartCoroutine(Delay(3.2f));
            IEnumerator Delay(float delay)
            {
                Exit();
                winScreen.SetActive(true);
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
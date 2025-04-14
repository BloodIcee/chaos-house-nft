using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChaosHouse
{
    public class LevelController : Singleton<LevelController>
    {
        [field: SerializeField] public bool FreeAllSkins { get; private set; }
        [field: SerializeField] public EGameMode GameMode { get; private set; } = EGameMode.SleepRoyale;
        
        [SerializeField] SurviveConfig surviveConfig;
        [SerializeField] SleepRoyaleConfig sleepRoyaleConfig;
        [SerializeField] SkinConfig skinConfig;
        [SerializeField] NamesConfig namesConfig;
        public bool InGame { get; private set; }
        public SurviveConfig SurviveConfig => surviveConfig;
        public SleepRoyaleConfig SleepRoyaleConfig => sleepRoyaleConfig;
        public Player player { get; private set; }        
        public int LevelNumber { get; private set; } = 1;
        public SkinConfig SkinConfig => skinConfig;
        public NamesConfig NamesConfig => namesConfig;
        public uint Coin { get; private set; }

        public List<Skin> AllSkins = new List<Skin>();

        private Bot[] allBots;

        protected override void Awake()
        {
            base.Awake();
            Coin = VirtualWallet.WalletData.CoinValue;            
        }

        private void Start()
        {
            allBots = FindObjectsOfType<Bot>();
            EventManager.Subscribe(EEventsName.LevelRestart, HandleLevelRestart);
            EventManager.Subscribe(EEventsName.LevelStart, HandleLevelStart);
            EventManager.Subscribe(EEventsName.LoadMenu, HandleLoadMenu);
            EventManager.Subscribe(EEventsName.LevelComplite, HandleLevelComplite);
            player = FindObjectOfType<Player>();
            if (player != null) player.SetName(SaveManager.SavedData.PlayerName);
            SetBotSkin();
        }

        private void SetBotSkin()
        {
            //if (allBots.Length == 0) return;
            //List<Skin> skins = player.characterSkin.Skins;
            //var skinPlayer = player.CurrentSkin.SkinType;
            //skins.Remove(skins.Find(x => x.SkinType == skinPlayer));

            //for (int i = 0; i < allBots.Length; i++)
            //{
            //    var skin = allBots[i].characterSkin.GetRandomSkin(skins);
            //    allBots[i].characterSkin.SetSkin(skin.SkinType);
            //    skins.Remove(skin);
            //}
        }
        private void HandleLevelComplite(object o)
        {
            InGame = false;
            LevelNumber++;
        }
        private void HandleLoadMenu(object o)
        {
            SceneManager.LoadScene(0);
            InGame = false;
        }

        private void HandleLevelStart(object o)
        {
            InGame = true;
        }

        private void HandleLevelRestart(object o)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}

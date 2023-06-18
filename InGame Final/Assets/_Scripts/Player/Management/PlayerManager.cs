using System.Collections.Generic;
using UnityEngine;
using _Scripts.Interaction.Management;
using _Scripts.Player.Unit;
using _Scripts.Player.Structure;
using TMPro;

namespace _Scripts.Player.Management
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        public Transform playerUnits;
        public Transform playerBarracks;
        public Transform playerTowers;
        public Transform enemyUnits;

        public List<PlayerUnit> meleeSoldiers = new List<PlayerUnit>();
        public List<PlayerUnit> rangedSoldiers = new List<PlayerUnit>();
        public List<PlayerTower> towers = new List<PlayerTower>();
        public List<PlayerBarracks> barracks = new List<PlayerBarracks>();
        public List<PlayerWorker> workers = new List<PlayerWorker>();

        private float playerOre = 300;
        private float playerWood = 300;
        public float[] roundTimer = new float[3];
        public TextMeshProUGUI oreText;
        public TextMeshProUGUI woodText;
        public TextMeshProUGUI secondText;
        public TextMeshProUGUI minuteText;

        public float PlayerOre
        {
            get { return playerOre; }
            set
            {
                playerOre = value;
                oreText.text = ((int) playerOre).ToString();
            }
        }
        
        public float PlayerWood
        {
            get { return playerWood; }
            set
            {
                playerWood = value; 
                woodText.text = ((int) playerWood).ToString();
            }
        }
        
        
        void Awake()
        {
            instance = this;
            PlayerWood = playerWood;
            PlayerOre = playerOre;
        }

        void Update()
        {
            InputHandler.instance.HandlePlayerInput();
            roundTimer[0] += Time.deltaTime;
            secondText.text = ((int) roundTimer[0]).ToString();
            roundTimer[2] += Time.deltaTime;

            if (roundTimer[0] > 60)
            {
                roundTimer[0] -= 60;
                roundTimer[1] += 1;
                minuteText.text = ((int) roundTimer[1]).ToString();
            }
        }
    }
}

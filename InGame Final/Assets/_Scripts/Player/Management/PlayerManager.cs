using System.Collections.Generic;
using UnityEngine;
using _Scripts.Interaction.Management;
using _Scripts.Player.Unit;
using _Scripts.Player.Structure;

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

        public float playerOre;
        public float playerWood;
        public float playerGold;
        private float elapsedTime;
        public float[] roundTimer = new float[3];

        void Awake()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            InputHandler.instance.HandlePlayerInput();
            elapsedTime += Time.deltaTime;
            roundTimer[0] += Time.deltaTime;
            roundTimer[2] += Time.deltaTime;

            if (elapsedTime > 1)
            {
                playerOre += 100;
                elapsedTime = 0;
            }

            if (roundTimer[0] > 60)
            {
                roundTimer[0] -= 60;
                roundTimer[1] += 1;
            }
        }
    }
}

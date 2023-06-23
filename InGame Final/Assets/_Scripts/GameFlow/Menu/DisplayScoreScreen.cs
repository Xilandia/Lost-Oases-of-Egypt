using System.Text;
using _Scripts.GameFlow.Objective;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Menu
{
    public class DisplayScoreScreen : MonoBehaviour
    {
        public TextMeshProUGUI totalScoreText;
        public TextMeshProUGUI enemiesKilledText;
        public TextMeshProUGUI resourcesCollectedText;
        public TextMeshProUGUI unitsSurvivedText;
        public TextMeshProUGUI buildingsSurvivedText;
        public TextMeshProUGUI gameTimeText;

        void Awake()
        {
            InputScores();
        }

        private void InputScores()
        {
            totalScoreText.text = "Total Score: " + ScoreHandler.instance.totalScore;
            gameTimeText.text = "Game Time: " + ScoreHandler.instance.gameTime[1] + " : " + ScoreHandler.instance.gameTime[0];
            
            PopulateEnemies();
            PopulateResources();
            PopulateUnits();
            PopulateBuildings();
        }

        private void PopulateEnemies()
        {
            StringBuilder enemiesKilled = new StringBuilder();
            enemiesKilled.AppendLine("Enemies Killed:");
            enemiesKilled.AppendLine("Basic: " + ScoreHandler.instance.totalEnemiesKilled[0]);
            enemiesKilled.AppendLine("Climber: " + ScoreHandler.instance.totalEnemiesKilled[1]);
            enemiesKilled.AppendLine("Fast: " + ScoreHandler.instance.totalEnemiesKilled[2]);
            enemiesKilled.AppendLine("Tank: " + ScoreHandler.instance.totalEnemiesKilled[3]);
            enemiesKilled.AppendLine("Boss: " + ScoreHandler.instance.totalEnemiesKilled[4]);
            enemiesKilled.AppendLine("Subtotal: " + ScoreHandler.instance.subtotals[0]);
            enemiesKilledText.text = enemiesKilled.ToString();
        }
        
        private void PopulateResources()
        {
            StringBuilder resourcesCollected = new StringBuilder();
            resourcesCollected.AppendLine("Resources Collected:");
            resourcesCollected.AppendLine();
            resourcesCollected.AppendLine("Wood: " + ScoreHandler.instance.totalResourcesCollected[0]);
            resourcesCollected.AppendLine("Ore: " + ScoreHandler.instance.totalResourcesCollected[1]);
            resourcesCollected.AppendLine("Statue: " + (ScoreHandler.instance.isStatueBuilt? "Yes" : "No"));
            resourcesCollected.AppendLine();
            resourcesCollected.AppendLine("Subtotal: " + ScoreHandler.instance.subtotals[1]);
            resourcesCollectedText.text = resourcesCollected.ToString();
        }
        
        private void PopulateUnits()
        {
            StringBuilder unitsSurvived = new StringBuilder();
            unitsSurvived.AppendLine("Units Survived:");
            unitsSurvived.AppendLine("Knights: " + ScoreHandler.instance.totalUnitsSurvived[1]);
            unitsSurvived.AppendLine("Archers: " + ScoreHandler.instance.totalUnitsSurvived[2]);
            unitsSurvived.AppendLine("Axemen: " + ScoreHandler.instance.totalUnitsSurvived[0]);
            unitsSurvived.AppendLine("Wizards: " + ScoreHandler.instance.totalUnitsSurvived[3]);
            unitsSurvived.AppendLine("Workers: " + ScoreHandler.instance.totalUnitsSurvived[4]);
            unitsSurvived.AppendLine("Subtotal: " + ScoreHandler.instance.subtotals[2]);
            unitsSurvivedText.text = unitsSurvived.ToString();
        }
        
        private void PopulateBuildings()
        {
            StringBuilder buildingsSurvived = new StringBuilder();
            buildingsSurvived.AppendLine("Buildings Survived:");
            buildingsSurvived.AppendLine();
            buildingsSurvived.AppendLine("Barracks: " + ScoreHandler.instance.totalBuildingsSurvived[2]);
            buildingsSurvived.AppendLine("Small Tower: " + ScoreHandler.instance.totalBuildingsSurvived[0]);
            buildingsSurvived.AppendLine("Big Tower: " + ScoreHandler.instance.totalBuildingsSurvived[1]);
            buildingsSurvived.AppendLine();
            buildingsSurvived.AppendLine("Subtotal: " + ScoreHandler.instance.subtotals[3]);
            buildingsSurvivedText.text = buildingsSurvived.ToString();
        }
        
        public void RestartGame()
        {
            Destroy(ScoreHandler.instance.gameObject);
            SceneManager.LoadScene("Game Map");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
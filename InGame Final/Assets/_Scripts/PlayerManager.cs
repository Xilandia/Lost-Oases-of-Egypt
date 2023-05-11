using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerUnits;
    public Transform playerTrainers;
    public Transform enemyUnits;

    public float playerOre = 0;
    private float elapsedTime = 0;
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

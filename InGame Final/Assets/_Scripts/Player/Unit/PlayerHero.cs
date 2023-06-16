using UnityEngine;

namespace _Scripts.Player.Unit
{
    public class PlayerHero : MonoBehaviour
    {
        [SerializeField] private PlayerUnit heroUnit;

        void OnDisable() // when hero dies
        {
            // lose game
        }
    }
}
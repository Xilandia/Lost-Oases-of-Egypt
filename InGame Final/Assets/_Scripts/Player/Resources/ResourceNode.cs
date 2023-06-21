using UnityEngine;

namespace _Scripts.Player.Resources
{
    public class ResourceNode : MonoBehaviour
    {
        public enum ResourceTypes
        {
            Wood,
            Ore
        }

        public float resourceAmount;
        public float resourceGatherRate;
        public ResourceTypes resourceType;
        public float offset;

        public float GatherResource()
        {
            if (resourceAmount > resourceGatherRate)
            {
                resourceAmount -= resourceGatherRate;
                return resourceGatherRate;
            }
            else
            {
                Destroy(gameObject);
                return resourceAmount;
            }
        }
    }
}
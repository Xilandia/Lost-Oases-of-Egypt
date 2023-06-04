using UnityEngine;

namespace _Scripts.Player.Resources
{
    public class ResourceNode : MonoBehaviour
    {
        public enum ResourceTypes
        {
            Wood,
            Ore,
            Gold
        }

        public float resourceAmount;
        public float resourceGatherRate;
        public ResourceTypes resourceType;

        public float GatherResource()
        {
            if (resourceAmount > resourceGatherRate)
            {
                resourceAmount -= resourceGatherRate;
                return resourceGatherRate;
            }
            else
            {
                return RunDry();
            }
        }

        private float RunDry()
        {
            Destroy(gameObject);
            return resourceAmount;
        }
    }
}
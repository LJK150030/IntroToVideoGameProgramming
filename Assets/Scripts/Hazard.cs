using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Hazard : ScriptableObject
    {
        public string hazard;

        public string Description;

        public string Warning;

        public abstract void HazardEvent(RoomNavigation dungeon, GameController controller);


    }
}

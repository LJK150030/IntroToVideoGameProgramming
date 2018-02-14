using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractableObjects : MonoBehaviour
    {
    
        [HideInInspector] public List<string> NounsInRoom = new List<string>();

        public string GetEvent(Room currentRoom, int i)
        {
            Hazard hazardInRoom = currentRoom.HazardsInRoom[i];

            NounsInRoom.Add(hazardInRoom.hazard);
            return hazardInRoom.Description;
        }

        public int TriggerEvent(Room currentRoom, RoomNavigation dungeon, int i, GameController controller)
        {
            Hazard hazrdInRoom = currentRoom.HazardsInRoom[i];
            hazrdInRoom.HazardEvent(dungeon, controller);

            if (hazrdInRoom.hazard.Equals("Wumpus"))
                return 0;
            if (hazrdInRoom.hazard.Equals("Bottomless Pit"))
                return 1;
            if (hazrdInRoom.hazard.Equals("Superbat"))
                return 2;
            return -1;
        }
    }
}

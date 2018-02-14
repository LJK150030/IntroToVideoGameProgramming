using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/Hazard/Superbat")]
    public class Superbat : Hazard
    {

        public override void HazardEvent(RoomNavigation dungeon, GameController controller)
        {
            dungeon.CurrentRoom = dungeon.Dungeon[Random.Range(0, 20)];
        }
    }
}

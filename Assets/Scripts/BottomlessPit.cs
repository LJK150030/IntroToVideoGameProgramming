using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/Hazard/Bottomless Pit")]
    public class BottomlessPit : Hazard
    {

        public override void HazardEvent(RoomNavigation dungeon, GameController controller)
        {
            controller.PlayerDead = true;
        }
    }
}

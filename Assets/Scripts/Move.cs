using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/InputActions/Move")]
    public class Move : InputAction
    {
        public override void RespondToInput(GameController controller, string[] separatedInputWords)
        {
            controller.RoomNavigation.AttemptToChagneRooms(separatedInputWords[1]);
        }
    }
}

using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/InputActions/Quit")]
    public class Quit : InputAction
    {
        public override void RespondToInput(GameController controller, string[] separatedInputWords)
        {
            //controller.roomNavigation.AttemptToChagneRooms(separatedInputWords[0]);
            //controller.AskToReset();
            Application.Quit();
        }
    }
}

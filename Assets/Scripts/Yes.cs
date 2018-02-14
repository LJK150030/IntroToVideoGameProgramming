using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/InputActions/Yes")]
    public class Yes : InputAction
    {
        public override void RespondToInput(GameController controller, string[] separatedInputWords)
        {
            //controller.AskToReset();
            //Asking to reset with original settings
            if (controller.EndOfGame)
            {
                controller.ResetWithSameSetUp();
            }
        }
    }
}

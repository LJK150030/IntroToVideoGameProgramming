using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/InputActions/No")]
    public class No : InputAction
    {
        public override void RespondToInput(GameController controller, string[] separatedInputWords)
        {
            //Asking to reset with original settings
            if (controller.EndOfGame)
            {
                controller.Reset();
            }
        }
    }
}

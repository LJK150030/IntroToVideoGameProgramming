using UnityEngine;

namespace Assets.Scripts
{
    public abstract class InputAction : ScriptableObject
    {
        public string KeyWork;

        public abstract void RespondToInput(GameController controller, string[] separatedInputWords);
    }
}

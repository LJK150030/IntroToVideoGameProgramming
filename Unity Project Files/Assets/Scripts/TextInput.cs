using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TextInput : MonoBehaviour
    {
        private GameController _controller;
        public InputField InputField;

        void Awake()
        {
            _controller = GetComponent<GameController>();
            //When the player presses enter, whatever is in the text filed will be sent to the log file
            InputField.onEndEdit.AddListener(AcceptStringInput);
        }
    
        //Comparing string from out library
        public void AcceptStringInput(string userInput)
        {
            userInput = userInput.ToLower();
            _controller.LogStringWithReturn(userInput);

            //Take whatever the player writes in, and create a new string from each space
            char[] delimiterCharacters = {' '};
            string[] separatedInputWords = userInput.Split(delimiterCharacters);

            foreach (InputAction inputAction in _controller.InputAction)
            {
                if (inputAction.KeyWork == separatedInputWords[0])
                {
                    inputAction.RespondToInput(_controller, separatedInputWords);
                }
            }

            InputComplete();
        }

        public void InputComplete()
        {
            _controller.DisplayLoggedText();
            InputField.ActivateInputField(); // Reactivate after enter
            InputField.text = null; //empty text feild
        }
    }
}

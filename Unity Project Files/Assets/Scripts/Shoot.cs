using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/InputActions/Shoot")]
    public class Shoot : InputAction
    {
        int _turn;
        //private int count = 0;

        public override void RespondToInput(GameController controller, string[] separatedInputWords)
        {
            _turn = 1;
            int length = separatedInputWords.Length-1;

            if (controller.RoomNavigation.CurrentRoom != null)
            {
                if (_turn <= 5)
                {
                    while ((length - 2) > 0)
                    {
                        if (separatedInputWords[length] == separatedInputWords[length - 2])
                        {
                            controller.LogStringWithReturn("Arrows aren't that crooked - try another room");
                            return;
                        }

                        length--;
                    }

                    if (separatedInputWords.Length > 2)
                        if (("Room " + separatedInputWords[2]) == controller.RoomNavigation.CurrentRoom.RoomName)
                        {
                            controller.LogStringWithReturn("Arrows aren't that crooked - try another room");
                            return;
                        }


                    for (int i = 1; i < separatedInputWords.Length; i++)
                    {
                        var keepGoing = controller.RoomNavigation.AttemptToShootThroughRooms(separatedInputWords[i], _turn, separatedInputWords.Length - 1);
                        _turn++;

                        if(!keepGoing)
                            break;
                    }
                }
                else
                {
                    controller.LogStringWithReturn("Too many rooms");
                }
            }

            controller.DisplayRoomText();
        }
    }
}

using UnityEngine;

//Scripts that are like monobehaviour, but are never attached to game objects
//Can be used to create assets which store data or execute code

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/Room")]
    public class Room : ScriptableObject{
        [TextArea]  //Display a large text box
        public string Description;  //used to describe what the rooom is
        public string RoomName;     //used to check against interaction
        public Exit[] Exits;
        public Hazard[] HazardsInRoom;
    }
}

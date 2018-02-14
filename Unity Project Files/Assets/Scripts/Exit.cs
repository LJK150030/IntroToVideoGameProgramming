

//Want to be able to display the exit and more information in the inspector
namespace Assets.Scripts
{
    [System.Serializable]
//Should just be a data class and not extend Monobehavior
    public class Exit
    {
        public string KeyString;
        public string ExitDescription;
        public Room ValueRoom;

    }
}

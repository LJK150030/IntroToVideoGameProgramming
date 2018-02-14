using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "TextAdventure/Hazard/Wumpus")]
    public class Wumpus : Hazard
    {
        private int _probability;
        private bool _foundNewRoom;
        public Room CurrentRoom;
        private Room _temp;

        public override void HazardEvent(RoomNavigation dungeon, GameController controller)
        {
            controller.PlayerDead = true;
        }

        public void SetLocation(Room current)
        {
            CurrentRoom = current;
        }

        public void Move()
        {
            _probability = Random.Range(1, 101);
            if (_probability > 25)
            {
                while (!_foundNewRoom)
                {
                    _probability = Random.Range(0, 3);
                    _temp = CurrentRoom.Exits[_probability].ValueRoom;
                    if (_temp.HazardsInRoom[0] == null)
                    {
                        _foundNewRoom = true;
                        _temp.HazardsInRoom[0] = this;
                        CurrentRoom.HazardsInRoom[0] = null;
                        CurrentRoom = _temp;
                    }
                }
                _foundNewRoom = false;
            }
        }
    }
}

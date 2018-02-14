using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using Random = UnityEngine.Random;

//Used to Navigate through the rooms of the text adventure
namespace Assets.Scripts
{
    public class RoomNavigation : MonoBehaviour{
        
        public Room[] Dungeon;
        public Room CurrentRoom;
        public Room RoomArrowIsIn;
        public MapGraphics Art;
        //private int timer = 0;


        private GameController _controller;
        private Dictionary<string, Room> _exitDictionary;

        public RoomNavigation(int arrowMaxTurns)
        {
        }

        void Awake()
        {
            _exitDictionary = new Dictionary<string, Room>();
            _controller = GetComponent<GameController>();

            CurrentRoom = Dungeon[Random.Range(0, 20)];
            RoomArrowIsIn = CurrentRoom;

            foreach (Room t in Dungeon)
            {
                t.HazardsInRoom[0] = null;
            }
        }

        public void UnpackExitsInRoom()
        {
            _controller.InteractionDescriptionsInRoom.Add("Tunnels lead to:");

            foreach (Exit t in CurrentRoom.Exits)
            {
                _exitDictionary.Add(t.KeyString, t.ValueRoom);
                _controller.InteractionDescriptionsInRoom.Add(t.ExitDescription);
            }
        }

        public void AttemptToChagneRooms(string directionNoun)
        {
            if (CurrentRoom == null)
                return;

            if (_exitDictionary.ContainsKey(directionNoun))
            {
                CurrentRoom = _exitDictionary[directionNoun];
                RoomArrowIsIn = CurrentRoom;
                //controller.LogStringWithReturn("You head off to room " + directionNoun);
                _controller.DisplayRoomText();
            }
            else
            {
                _controller.LogStringWithReturn("There is no path to " + directionNoun);
            }
        }

        public bool AttemptToShootThroughRooms(string directionNoun, int start, int end)
        {
        
            _controller.WumpusAwake = true;

            if (_exitDictionary.ContainsKey(directionNoun) && _controller.WumpusDead == false)
            {
                if(RoomArrowIsIn.Exits[0].KeyString.Equals(directionNoun) ||
                   RoomArrowIsIn.Exits[1].KeyString.Equals(directionNoun) ||
                   RoomArrowIsIn.Exits[2].KeyString.Equals(directionNoun))
                    RoomArrowIsIn = _exitDictionary[directionNoun];
                else
                {
                    RoomArrowIsIn = RoomArrowIsIn.Exits[Random.Range(0, 3)].ValueRoom;
                }

            }
            string[] arrowRoom = RoomArrowIsIn.RoomName.Split(' ');
            //print(roomArrowIsIn.roomName);
            Art.ShootArrow(int.Parse(arrowRoom[1]) - 1);

            if (RoomArrowIsIn == CurrentRoom)
            {
                _controller.LogStringWithReturn("Ouch! Arrow got you!");
                RoomArrowIsIn = CurrentRoom;
                return false;
            }

            if (RoomArrowIsIn.HazardsInRoom[0] != null)
            {
                if (RoomArrowIsIn.HazardsInRoom[0].hazard == "Wumpus")
                {
                    _controller.WumpusDead = true;
                    return false;
                }
            }

            if (start >= end)
            {
                _controller.LogStringWithReturn("Missed");
                RoomArrowIsIn = CurrentRoom;
                _controller.NumArrows--;
                return false;
            }

            //controller.DisplayRoomText();
            return true;
        }

        public int CheckSurroundRooms()
        {
            int numBats = 0;
            int numPits = 0;
            int numWump = 0;

            foreach (Exit t in CurrentRoom.Exits)
            {
                if (t.ValueRoom.HazardsInRoom[0] == null) continue;
            
                _controller.InteractionDescriptionsInRoom.Add(t.ValueRoom.HazardsInRoom[0].Warning);
                if (t.ValueRoom.HazardsInRoom[0].hazard.Equals("Superbat"))
                    numBats++;
                if (t.ValueRoom.HazardsInRoom[0].hazard.Equals("Bottomless Pit"))
                    numPits++;
                if (t.ValueRoom.HazardsInRoom[0].hazard.Equals("Wumpus"))
                    numWump++;
            }

            if (numBats == 0 && numPits == 0 && numWump != 0)
                return 001;
            if(numBats == 0 && numPits != 0 && numWump == 0)
                return 010;
            if (numBats != 0 && numPits == 0 && numWump == 0)
                return 100;
            if (numBats != 0 && numPits != 0 && numWump == 0)
                return 110;
            if (numBats != 0 && numPits == 0 && numWump != 0)
                return 101;
            if (numBats == 0 && numPits != 0 && numWump != 0)
                return 011;
            if (numBats != 0 && numPits != 0 && numWump != 0)
                return 111;
            return 0;
        }

        public void ClearExits()
        {
            if(_exitDictionary.Count != 0)
                _exitDictionary.Clear();
        }
    }
}

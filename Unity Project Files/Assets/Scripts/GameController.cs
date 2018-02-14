using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;


//Manaage the diffrent systems within the game. Such as the room navigation script, text input script, action script, and so on.
namespace Assets.Scripts
{
    public class GameController : MonoBehaviour{

        //Shows all of the text onto the screen
        public Text DisplayText;
        public InputAction[] InputAction;
        public Hazard[] Hazards;
        public MapGraphics Art;

        //Used to show all of the actions taken thus far for the player
        public List<string> ActionLog = new List<string>();


        //Wanting this variable to be public, but will want to hide it from the insepctor as to not manualy overide the variable.
        [HideInInspector] public RoomNavigation RoomNavigation;
        [HideInInspector] public List<string> InteractionDescriptionsInRoom = new List<string>();
        [HideInInspector] public InteractableObjects InteractableObjects;
        [HideInInspector] public bool EndOfGame;
        [HideInInspector] public bool TriggeredHazard;
        [HideInInspector] public Room StartinRoom;
        [HideInInspector] public bool WumpusDead;
        [HideInInspector] public bool WumpusAwake;
        [HideInInspector] public int NumArrows;
        [HideInInspector] public bool PlayerDead;

        private int[] _setUp;
        private int _numHazards;
        private int _wumpus;
        private int _hazardBit;
        private int _hazardSelect;
        //private bool pause = true;


        // Use this for initialization
        public void Awake ()
        {
            RoomNavigation = GetComponent<RoomNavigation>();
            InteractableObjects = GetComponent<InteractableObjects>();

            EndOfGame = false;
            TriggeredHazard = false;
            PlayerDead = false;
            WumpusDead = false;
            WumpusAwake = false;
            StartinRoom = RoomNavigation.CurrentRoom;
            _setUp = new int[Hazards.Length];
            NumArrows = 5;

            _numHazards = 0;

            while (_numHazards < Hazards.Length)
            {
                int choosenRoom = Random.Range(0, 20);

                if (RoomNavigation.Dungeon[choosenRoom].HazardsInRoom[0] == null && RoomNavigation.Dungeon[choosenRoom].RoomName != StartinRoom.RoomName)
                {
                    RoomNavigation.Dungeon[choosenRoom].HazardsInRoom[0] = Hazards[_numHazards];
                    _setUp[_numHazards] = choosenRoom;
                    _numHazards++;
                }
            }

            for (int i = 0; i < Hazards.Length; i++)
            {
                if (Hazards[i].GetType() == typeof(Wumpus))
                    _wumpus = i;
            }

            ((Wumpus) Hazards[_wumpus]).SetLocation(RoomNavigation.Dungeon[_setUp[_wumpus]]);
        }

        public void Start()
        {
            LogStringWithReturn("Hunt the Wumpus!");
            LogStringWithReturn("Find the Wumpus within the Dodecahedral Dungeon ");
            LogStringWithReturn("by shooting him with your crooked arrows.");
            LogStringWithReturn("To move, type 'move *number*'");
            LogStringWithReturn("To shoot, type 'shoot *number* (up to 5 times)'");
            LogStringWithReturn("to reset, type 'quit'");
            DisplayRoomText();
            DisplayLoggedText();
        }

        //Used to display the logged text
        public void DisplayLoggedText()
        {
            //used to join everything in the list as a string, seperate by a new line (\n), and pass the actionlog as an array
            string logAsText = string.Join("\n", ActionLog.ToArray());

            DisplayText.text = logAsText;
        }

        //Display the room as text
        public void DisplayRoomText()
        {
            //for (int i = 0; i < roomname.Length; i++)
            //{
            //    print(roomname[i]);
            //}

            if (!PlayerDead && !WumpusDead)
            {
                ClrearCollectionsForNewRoom();
                if(WumpusAwake)
                    ((Wumpus) Hazards[_wumpus]).Move();
                UnpackRoom();
                string joinedInteractionDescriptions = string.Join("\n", InteractionDescriptionsInRoom.ToArray());

                var roomname = RoomNavigation.CurrentRoom.RoomName.Split(' ');

                string combinedText;
                if (TriggeredHazard)
                {
                    TriggeredHazard = false;
                    combinedText = joinedInteractionDescriptions;
                    LogStringWithReturn(combinedText);

                    if (RoomNavigation.CurrentRoom.HazardsInRoom[0] != null)
                    {
                        if (RoomNavigation.CurrentRoom.HazardsInRoom[0].GetType() == typeof(Superbat))
                        {
                            //int frames = 0;
                            HazardTriggered(RoomNavigation.CurrentRoom);
                            Art.HazardSign(int.Parse(roomname[1]) - 1, _hazardSelect);

                            Invoke("DisplayRoomText", 2);
                            return;
                        }

                        if (RoomNavigation.CurrentRoom.HazardsInRoom[0].GetType() == typeof(BottomlessPit) ||
                            RoomNavigation.CurrentRoom.HazardsInRoom[0].GetType() == typeof(Wumpus))
                        {
                            PlayerDead = true;
                            HazardTriggered(RoomNavigation.CurrentRoom);
                            Art.HazardSign(int.Parse(roomname[1]) - 1, _hazardSelect);
                        }
                    }
                }
                else
                {
                    combinedText = RoomNavigation.CurrentRoom.Description + "\n" + joinedInteractionDescriptions;
                    LogStringWithReturn(combinedText);
                }

                roomname = RoomNavigation.CurrentRoom.RoomName.Split(' ');
                Art.UpdateGraphics(int.Parse(roomname[1]) - 1);
                Art.HazardWarning(_hazardBit);
            }

            if (NumArrows < 0)
            {
                PlayerDead = true;
                LogStringWithReturn("You ran out of Arrows!");
            }

            if (PlayerDead)
            {
                AskToReset();
            }

            if (WumpusDead)
            {
                LogStringWithReturn("Aha! You got the Wumpus!");
                LogStringWithReturn("Hee hee hee - the Wumpus'll getcha next time!!");
                AskToReset();
            }
            LogStringWithReturn("------------------------------------------------");
        }

        void ClrearCollectionsForNewRoom()
        {
            InteractionDescriptionsInRoom.Clear();
            RoomNavigation.ClearExits();
        }

        private void UnpackRoom()
        {
            PrepareHazards(RoomNavigation.CurrentRoom);
            if (!TriggeredHazard)
            {
                _hazardBit = RoomNavigation.CheckSurroundRooms();
                RoomNavigation.UnpackExitsInRoom();
            }
        }

        private void PrepareHazards(Room currentRoom)
        {
            for (int i = 0; i < currentRoom.HazardsInRoom.Length; i++)
            {
                if (currentRoom.HazardsInRoom[i] != null)
                {
                    string description = InteractableObjects.GetEvent(currentRoom, i);

                    InteractionDescriptionsInRoom.Add(description);

                    TriggeredHazard = true;
                }
            }
        }

        private void HazardTriggered(Room currentRoom)
        {
            for (int i = 0; i < currentRoom.HazardsInRoom.Length; i++)
            {
                _hazardSelect = InteractableObjects.TriggerEvent(currentRoom, RoomNavigation, i, this);
            }
        }

        //Used to add strings to the action log
        public void LogStringWithReturn(string stringToAdd)
        {
            ActionLog.Add(stringToAdd + "\n");
        }

        public void AskToReset()
        {
            LogStringWithReturn("Re-play using same set-up?");
            RoomNavigation.CurrentRoom = null;
            EndOfGame = true;
        }

        public void ResetWithSameSetUp()
        {
            PlayerDead = false;
            WumpusDead = false;
            WumpusAwake = false;
            NumArrows = 5;
            Art.Reset();

            foreach (Room t in RoomNavigation.Dungeon)
            {
                t.HazardsInRoom[0] = null;
            }

            for (int i = 0; i < _setUp.Length; i++)
            {
                RoomNavigation.Dungeon[_setUp[i]].HazardsInRoom[0] = Hazards[i];
            }


            RoomNavigation.CurrentRoom = StartinRoom;
            DisplayRoomText();
        }

        public void Reset()
        {
            PlayerDead = false;
            WumpusDead = false;
            WumpusAwake = false;
            StartinRoom = RoomNavigation.Dungeon[Random.Range(0, 20)];
            NumArrows = 5;
            Art.Reset();

            foreach (Room t in RoomNavigation.Dungeon)
            {
                t.HazardsInRoom[0] = null;
            }

            _numHazards = 0;

            while (_numHazards < Hazards.Length)
            {
                int choosenRoom = Random.Range(0, 20);
                if (RoomNavigation.Dungeon[choosenRoom].HazardsInRoom[0] == null && !RoomNavigation.Dungeon[choosenRoom].Equals(StartinRoom))
                {
                    RoomNavigation.Dungeon[choosenRoom].HazardsInRoom[0] = Hazards[_numHazards];
                    _setUp[_numHazards] = choosenRoom;
                    _numHazards++;
                }
            }

            RoomNavigation.CurrentRoom = StartinRoom;
            DisplayRoomText();
        }
    }
}

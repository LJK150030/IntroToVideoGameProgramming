using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MapGraphics : MonoBehaviour
    {

        public RawImage[] RoomImages;
        public Dictionary<string, RawImage> PathImages;
        public Transform[] Hazards;
        public RawImage[] HazardsImages;
        int _currentRoom = -1;
        readonly int[] _neighbors = new int[3];


        void Awake()
        {
            //Get the room game object where all of the sprites are held
            //Array used to store image.
            //RoomGameObject = gameObject.transform.GetChild(1).gameObject;
            RoomImages = new RawImage[20];
            RoomImages = gameObject.transform.GetChild(1).GetComponentsInChildren<RawImage>();
            PathImages = new Dictionary<string, RawImage>();
            Hazards = new Transform[3];
            HazardsImages = new RawImage[3];

            //Get the Path game object
            //PathGameObject = gameObject.transform.GetChild(0).gameObject;
            for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
            {
                //print(i);
                //print(gameObject.transform.GetChild(0).GetChild(i).name);
                PathImages.Add(gameObject.transform.GetChild(0).GetChild(i).name, gameObject.transform.GetChild(0).GetChild(i).gameObject.GetComponent<RawImage>());
            }

            Hazards[0] = gameObject.transform.GetChild(2).GetChild(0);
            Hazards[1] = gameObject.transform.GetChild(2).GetChild(1);
            Hazards[2] = gameObject.transform.GetChild(2).GetChild(2);

            HazardsImages[0] = Hazards[0].GetComponent<RawImage>();
            HazardsImages[1] = Hazards[1].GetComponent<RawImage>();
            HazardsImages[2] = Hazards[2].GetComponent<RawImage>();

            //Make everything dark
            foreach (RawImage t in RoomImages)
            {
                t.color = Color.clear;
            }

            foreach (KeyValuePair<string, RawImage> images in PathImages)
            {
                images.Value.color = Color.clear;
            }

            Hazards[0].position = new Vector3(-100, 0, 0);
            Hazards[1].position = new Vector3(-100, 0, 0);
            Hazards[2].position = new Vector3(-100, 0, 0);
        }

        public void UpdateGraphics(int roomIndex)
        {
            int count = 0;

            if (_currentRoom != -1)
                Clear();

            _currentRoom = roomIndex;

            foreach (KeyValuePair<string, RawImage> images in PathImages)
            {
                string[] temp = images.Key.Split('-');

                if((int.Parse(temp[0]) - 1) == _currentRoom || (int.Parse(temp[1]) - 1) == _currentRoom)
                {
                    images.Value.color = Color.white;

                    if ((int.Parse(temp[0]) - 1) == _currentRoom)
                        _neighbors[count] = int.Parse(temp[1]) - 1;
                    else
                        _neighbors[count] = int.Parse(temp[0]) - 1;

                    count++;
                }
            }

            RoomImages[_currentRoom].color = Color.white;
            RoomImages[_neighbors[0]].color = Color.white;
            RoomImages[_neighbors[1]].color = Color.white;
            RoomImages[_neighbors[2]].color = Color.white;
        }

        public void HazardWarning(int binary)
        {
            //print(binary);

            if(binary == 001)
                RoomImages[_currentRoom].color = Color.red;
            if(binary == 010)
                RoomImages[_currentRoom].color = Color.blue;
            if(binary == 100)
                RoomImages[_currentRoom].color = Color.green;
            if (binary == 110)
                RoomImages[_currentRoom].color = Color.cyan;
            if(binary == 101)
                RoomImages[_currentRoom].color = Color.yellow;
            if(binary == 011)
                RoomImages[_currentRoom].color = Color.magenta;
            if (binary == 111)
                RoomImages[_currentRoom].color = Color.gray;
        }

        public void HazardSign(int roomIndex, int hazardIndex)
        {
            Hazards[hazardIndex].position = new Vector3(gameObject.transform.GetChild(1).GetChild(roomIndex).position.x,
                gameObject.transform.GetChild(1).GetChild(roomIndex).position.y,
                gameObject.transform.GetChild(1).GetChild(roomIndex).position.z);

            if (hazardIndex == 0)
            {
                HazardsImages[hazardIndex].color = Color.red;
                RoomImages[_currentRoom].color = Color.red;
            }
            if (hazardIndex == 1)
            {
                HazardsImages[hazardIndex].color = Color.blue;
                RoomImages[_currentRoom].color = Color.blue;
            }
            if (hazardIndex == 2)
            {
                RoomImages[_currentRoom].color = Color.white;
                HazardsImages[hazardIndex].color = Color.green;
                //RoomImages[currentRoom].color = Color.green;
                Invoke("reset", 2);
            }
        }

        public void Reset()
        {
            Hazards[0].position = new Vector3(-100, 0, 0);
            Hazards[1].position = new Vector3(-100, 0, 0);
            Hazards[2].position = new Vector3(-100, 0, 0);
        }

        public void ShootArrow(int roomIndex)
        {
            //print(roomIndex);
            RoomImages[roomIndex].color = Color.grey;
        }

        public void Clear()
        {
            foreach (RawImage room in RoomImages)
            {
                room.color = Color.clear;
            }
        

            foreach (KeyValuePair<string, RawImage> images in PathImages)
            {
                string[] temp = images.Key.Split('-');

                if ((int.Parse(temp[0]) - 1) == _currentRoom || (int.Parse(temp[1]) - 1) == _currentRoom)
                {
                    images.Value.color = Color.clear;
                }
            }
        }

    }
}

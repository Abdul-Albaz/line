using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLevelManger : MonoBehaviour
{
        public List<Color> Colours = new List<Color>();

        public void InitialiseColours()
        {
            Colours.Add(new Color(4, 26, 77));
            Colours.Add(new Color(11, 86, 200));
            Colours.Add(new Color(221, 44, 55));
        }

        public Color PickRandomLevelColour()
        {
            if (Colours.Count < 1)
            {
                Debug.LogError($"We need at least one colour in order to pick a random colour");
            }

            int randomInt = Random.Range(0, Colours.Count);
            return Colours[randomInt];
        }

    }



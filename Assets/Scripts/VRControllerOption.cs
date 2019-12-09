using DilmerGames.Enums;
using UnityEngine;

namespace DilmerGames
{
    public class VRControllerOption : MonoBehaviour
    {
        public bool IsFocused = false;

        public bool IsSelected = false;

        public VRControllerOptionType VRControllerOptionType = VRControllerOptionType.NoSet;
    }
}
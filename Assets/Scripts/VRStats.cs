using DilmerGames.Core.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace DilmerGames
{
    public class VRStats : Singleton<VRStats>
    {
        [SerializeField]
        public Text firstText;

        [SerializeField]
        public Text secondText;

        [SerializeField]
        public Text thirdText;

        [SerializeField]
        public Text fourthText;
    }
}
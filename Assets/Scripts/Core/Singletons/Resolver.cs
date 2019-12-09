using UnityEngine;

namespace DilmerGames.Core.Singletons
{
    public class Resolver : Singleton<Resolver>
    {
        public OVRPlayerController VRPlayer { get; private set; }

        public VRDraw VRDrawLeftHand { get; private set; }

        public VRDraw VRDrawRightHand { get; private set; }

        void Awake()
        {
            VRPlayer = GameObject.Find("VRPlayerController").GetComponent<OVRPlayerController>();
            VRDrawLeftHand =  GameObject.Find("VRDrawLeft").GetComponent<VRDraw>();
            VRDrawRightHand = GameObject.Find("VRDrawRight").GetComponent<VRDraw>();
        }
    }
}
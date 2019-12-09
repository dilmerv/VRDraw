using UnityEngine;
using DG.Tweening;

namespace DilmerGames
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private LoopType loopType = LoopType.Yoyo;

        [SerializeField]
        private Vector3 rotationVector = new Vector3(0, 180,0);
        
        [SerializeField]
        private float rotationDuration = 1.0f;

        void Awake() => GetComponent<RectTransform>().DOLocalRotate(rotationVector, rotationDuration).SetLoops(-1, loopType);
    }
}
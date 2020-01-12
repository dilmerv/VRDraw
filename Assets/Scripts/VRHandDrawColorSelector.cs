using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DilmerGames
{
    [System.Serializable]
    public class VRHandDrawColorChanged : UnityEvent<Color> {}

    public class VRHandDrawColorSelector : MonoBehaviour
    {   
        [SerializeField]
        private TextMeshPro selectedText;

        [SerializeField]
        private VRHandDrawColorChanged OnColorLeftHandSelected;

        [SerializeField]
        private VRHandDrawColorChanged OnColorRightHandSelected;

        private Material material;
        
        private void Awake() 
        {
            material = GetComponent<Renderer>().material;  
            Debug.Log("Color" + material.color.ToString() );
            VRLogInfo.Instance.LogInfo("ovrHand.name: " + material.color.ToString()); 
             
        }

        public string ColorName
        {
            get
            {
                return material?.color.ToString();
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            selectedText.text = $"COLOR SELECTED: {gameObject.name}";
            Transform handTransform = other.transform.parent.parent.parent;
            
            VRLogInfo.Instance.LogInfo("handTransform.name: " + handTransform.name);

            if(handTransform != null && handTransform.name.Contains("Left"))
            {
                OnColorLeftHandSelected?.Invoke(material.color);
            }
            if(handTransform != null && handTransform.name.Contains("Right"))
            {
                OnColorRightHandSelected?.Invoke(material.color);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using DilmerGames.Events;

namespace DilmerGames
{
    [RequireComponent(typeof(RawImage))]
    public class VRDrawColor : VRControllerOption
    {   
        [SerializeField]
        private VRDrawColorChanged colorChanged;

        [SerializeField]
        private Image selectedImage;

        private RawImage rawImageColor;


        public string ColorName
        {
            get
            {
                return rawImageColor?.color.ToString();
            }
        }

        public void ColorSelect()  
        {
            selectedImage.enabled = true;
            colorChanged?.Invoke(rawImageColor.color);
        }

        void Awake() 
        { 
            selectedImage.enabled = false;
            rawImageColor = GetComponent<RawImage>();
        }

        void Update(){
            if(!IsSelected)
                selectedImage.enabled = false;
        }
    }
}
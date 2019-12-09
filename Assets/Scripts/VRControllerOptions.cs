using System.Collections;
using System.Linq;
using DilmerGames.Core.Singletons;
using DilmerGames.Enums;
using DilmerGames.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DilmerGames
{
    public class VRControllerOptions : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI lineWidthSliderLabel;

        [SerializeField]
        private TextMeshProUGUI minLineDistanceSliderLabel;

        [SerializeField]
        private Slider lineWidthSlider;

        [SerializeField]
        private Slider minLineDistanceSlider;

        [SerializeField]
        private float lineWidthSliderMultiplier = 0.001f;

        [SerializeField]
        private TextMeshProUGUI colorLabel;
        
        private CanvasGroup canvasGroup;

        [SerializeField]
        private bool isScreenHidden = true;

        [SerializeField]
        private ControlHand controlHand = ControlHand.NoSet;

        [SerializeField]
        private VRControllerSliderChanged OnLineChanged;

        [SerializeField]
        private VRControllerSliderChanged OnMinDistanceLineChanged;

        [SerializeField, Range(0, 5.0f)]
        private float fadeSpeed = 0.3f;

        private VRControllerOption[] options;

        private int currentOption = 0;

        public VRControllerOption selectedOption;

        [SerializeField]
        private bool leftKey = false;

        [SerializeField]
        private bool rightKey = false;

        public bool IsScreenHidden
        {
            get 
            {
                return isScreenHidden;
            }
        }
        
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            lineWidthSlider.onValueChanged.AddListener(LineWidthValueChanged);
            minLineDistanceSlider.onValueChanged.AddListener(MinDistanceLineWidthValueChanged);
            canvasGroup.alpha = 0;
            options = GetComponentsInChildren<VRControllerOption>()
                .Where(v => v != this).ToArray();
            selectedOption = options[currentOption];
        }

        void Update()
        {
            // left hand
            if(controlHand == ControlHand.Left)
            {
                // toggle screen control
                if(OVRInput.GetDown(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.X))
                    ToggleScreen();

                // increment slider value
                if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value += Time.deltaTime * lineWidthSliderMultiplier;

                // drecrement slider value
                if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;

                // focus option left direction
                if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) || Input.GetKeyDown(KeyCode.UpArrow))
                    FocusOption(false);

                // focus option right direction
                if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) || Input.GetKeyDown(KeyCode.DownArrow))
                    FocusOption(true);

                // select option
                if((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.L)) && ShoudActivateColor())
                    SelectOption(true);

                // unselect option
                if((OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyUp(KeyCode.L)))
                    SelectOption(false);
            }
            // right hand
            else if(controlHand == ControlHand.Right)
            {
                // toggle screen control
                if(OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A))
                    ToggleScreen();

                // increment slider value
                if((OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value += Time.deltaTime * lineWidthSliderMultiplier;

                // decrement slider value
                if((OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow)) && ShoudActivateSlider())
                    
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;

                // focus option left direction
                if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKeyDown(KeyCode.UpArrow))
                    FocusOption(false);

                // focus option right direction
                if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKeyDown(KeyCode.DownArrow))
                    FocusOption(true);

                // select option
                if((OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.R)) && ShoudActivateColor())
                    SelectOption(true);

                // unselect option
                if((OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyUp(KeyCode.R)))
                    SelectOption(false);
            }
        }
        
        bool ShoudActivateSlider() => selectedOption.VRControllerOptionType == VRControllerOptionType.Slider && !IsScreenHidden;

        bool ShoudActivateColor() => selectedOption.VRControllerOptionType == VRControllerOptionType.Color && !IsScreenHidden;

        void SelectOption(bool active)
        {
            selectedOption = options[currentOption];
            if(selectedOption.IsFocused)
            {
                selectedOption.IsSelected = active;
                if(active)
                {
                    VRDrawColor drawColor = (VRDrawColor)selectedOption;
                    drawColor.ColorSelect();
                }
            }
        }

        void FocusOption(bool rightDirection)
        {
            if(rightDirection)
            {
                if(currentOption >= options.Length - 1)
                    currentOption = 0; 
                else 
                    currentOption++;
            }
            else 
            {
                if(currentOption == 0)
                    currentOption = options.Length - 1; 
                else 
                    currentOption--;
            }

            selectedOption = options[currentOption];

            foreach(VRControllerOption option in options)
            {
                CanvasGroup optionCanvasGroup = option.GetComponent<CanvasGroup>();
                if(optionCanvasGroup == null) continue;

                if(option == selectedOption)
                {
                    optionCanvasGroup.alpha = 1.0f;
                    option.IsFocused = true;  
                    if(option is VRDrawColor)
                        colorLabel.text = ((VRDrawColor)option).ColorName;
                }    
                else
                {
                    option.IsFocused = false;
                    option.IsSelected = false;
                    optionCanvasGroup.alpha = 0.3f;
                }
            }
        }

        void LineWidthValueChanged(float newValue)
        {
            if(!IsScreenHidden) lineWidthSliderLabel.text = $"LINE WIDTH: {newValue.ToString("0.000")}";
            OnLineChanged?.Invoke(newValue);
        }

        void MinDistanceLineWidthValueChanged(float newValue)
        {
            if(!IsScreenHidden) minLineDistanceSliderLabel.text = $"NEW POINT DISTANCE: {newValue.ToString("0.000")}";
            OnMinDistanceLineChanged?.Invoke(newValue);
        }

        void ToggleScreen(bool noFade = false)
        {
            isScreenHidden = !isScreenHidden;

            if(isScreenHidden)
            {
                StartCoroutine(FadeAlpha(1,0, fadeSpeed));
            }
            else
            {
                StartCoroutine(FadeAlpha(0,1, fadeSpeed));
            }

            Resolver.Instance.VRPlayer.SetHaltUpdateMovement(!isScreenHidden);
        }

        private IEnumerator FadeAlpha(float from, float to, float duration) 
        {
            float elaspedTime = 0f;

            while (elaspedTime <= duration) 
            {
                elaspedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }
}
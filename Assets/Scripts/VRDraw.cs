using UnityEngine;
using System.Collections.Generic;
using DilmerGames.Enums;
using DilmerGames.Core.Utilities;

namespace DilmerGames
{
    public class VRDraw : MonoBehaviour
    {   
        [SerializeField]
        private ControlHand controlHand = ControlHand.NoSet;

        [SerializeField]
        private GameObject objectToTrackMovement;

        private Vector3 prevPointDistance = Vector3.zero;

        [SerializeField, Range(0, 1.0f)]
        private float minDistanceBeforeNewPoint = 0.2f;

        [SerializeField, Range(0, 1.0f)]
        private float minDrawingPressure = 0.8f;

        [SerializeField, Range(0, 1.0f)]
        private float lineDefaultWidth = 0.010f;

        private int positionCount = 0; // 2 by default

        private List<LineRenderer> lines = new List<LineRenderer>();

        private LineRenderer currentLineRender;

        [SerializeField]
        private Color defaultColor = Color.white;


        [SerializeField]
        private bool leftKey = false;

        [SerializeField]
        private bool rightKey = false;

        [SerializeField]
        private VRControllerOptions vrControllerOptions;
        
        public VRControllerOptions VRControllerOptions => vrControllerOptions;
        
        void Awake() => AddNewLineRenderer();

        void AddNewLineRenderer()
        {
            positionCount = 0;
            GameObject go = new GameObject($"LineRenderer_{controlHand.ToString()}_{lines.Count}");
            go.transform.parent = objectToTrackMovement.transform.parent;
            go.transform.position = objectToTrackMovement.transform.position;
            LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
            goLineRenderer.startWidth = lineDefaultWidth;
            goLineRenderer.endWidth = lineDefaultWidth;
            goLineRenderer.useWorldSpace = true;
            goLineRenderer.material = MaterialUtils.CreateMaterial(defaultColor, $"Material_{controlHand.ToString()}_{lines.Count}");
            goLineRenderer.positionCount = 1;
            goLineRenderer.numCapVertices = 90;
            goLineRenderer.SetPosition(0, objectToTrackMovement.transform.position);

            // send position
            TCPControllerClient.Instance.AddNewLine(objectToTrackMovement.transform.position);

            currentLineRender = goLineRenderer;
            lines.Add(goLineRenderer);
        }

        void Update()
        {
            if(!vrControllerOptions.IsScreenHidden) return;

    #if !UNITY_EDITOR
            // primary left controller
            if(controlHand == ControlHand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > minDrawingPressure)
            {
                VRStats.Instance.firstText.text = $"Axis1D.PrimaryIndexTrigger: {OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)}";
                UpdateLine();
            }
            else if(controlHand == ControlHand.Left && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                VRStats.Instance.secondText.text = $"Button.PrimaryIndexTrigger: {Time.deltaTime}";
                AddNewLineRenderer();
            }

            // secondary right controller
            if(controlHand == ControlHand.Right && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > minDrawingPressure)
            {
                VRStats.Instance.firstText.text = $"Axis1D.SecondaryIndexTrigger: {OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)}";
                UpdateLine();
            }
            else if(controlHand == ControlHand.Right && OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                VRStats.Instance.secondText.text = $"Button.SecondaryIndexTrigger: {Time.deltaTime}";
                AddNewLineRenderer();
            }

    #endif

    #if UNITY_EDITOR

            // primary left key
            if(controlHand == ControlHand.Left && leftKey)
            {
                VRStats.Instance.firstText.text = $"LeftKey: {leftKey}";
                UpdateLine();
            }
            else if(controlHand == ControlHand.Left && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                VRStats.Instance.secondText.text = $"Input.GetKeyDown(KeyCode.LeftArrow) {Input.GetKeyDown(KeyCode.LeftArrow)}";
                AddNewLineRenderer();
            }

            // secondary right key
            if(controlHand == ControlHand.Right && rightKey)
            {
                VRStats.Instance.firstText.text = $"RightKey: {rightKey}";
                UpdateLine();
            }
            else if(controlHand == ControlHand.Right && Input.GetKeyDown(KeyCode.RightArrow))
            {
                VRStats.Instance.secondText.text = $"Input.GetKeyDown(KeyCode.RightArrow): {Input.GetKeyDown(KeyCode.RightArrow)}";
                AddNewLineRenderer();
            }
    #endif
        }

        void UpdateLine()
        {
            if(prevPointDistance == null)
            {
                prevPointDistance = objectToTrackMovement.transform.position;
            }

            if(prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, objectToTrackMovement.transform.position)) >= minDistanceBeforeNewPoint)
            {
                Vector3 dir = (objectToTrackMovement.transform.position - Camera.main.transform.position).normalized;
                prevPointDistance = objectToTrackMovement.transform.position;
                AddPoint(prevPointDistance, dir);
            }
        }

        void AddPoint(Vector3 position, Vector3 direction)
        {
            currentLineRender.SetPosition(positionCount, position);
            positionCount++;
            currentLineRender.positionCount = positionCount + 1;
            currentLineRender.SetPosition(positionCount, position);
            
            // send position
            TCPControllerClient.Instance.UpdateLine(position);
        }

        public void UpdateLineWidth(float newValue)
        {
            currentLineRender.startWidth = newValue;
            currentLineRender.endWidth = newValue;
            lineDefaultWidth = newValue;
        }

        public void UpdateLineColor(Color color)
        {
            // in case we haven't drawn anything
            if(currentLineRender.positionCount == 1)
            {
                currentLineRender.material.color = color;
                currentLineRender.material.EnableKeyword("_EMISSION");
                currentLineRender.material.SetColor("_EmissionColor", color);
            }
            defaultColor = color;
        }

        public void UpdateLineMinDistance(float newValue)
        {
            minDistanceBeforeNewPoint = newValue;
        }
    }
}
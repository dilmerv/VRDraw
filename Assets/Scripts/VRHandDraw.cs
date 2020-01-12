using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace DilmerGames
{
    public enum HandToTrack
    {
        Left,
        Right
    }

    public class VRHandDraw : MonoBehaviour
    {   
        [SerializeField]
        private HandToTrack handToTrack = HandToTrack.Left;

        [SerializeField]
        private GameObject objectToTrackMovement;

        [SerializeField, Range(0, 1.0f)]
        private float minDistanceBeforeNewPoint = 0.2f;

        private Vector3 prevPointDistance = Vector3.zero;

        [SerializeField]
        private float minFingerPinchDownStrength = 0.5f;

        [SerializeField, Range(0, 1.0f)]
        private float lineDefaultWidth = 0.010f;

        private int positionCount = 0;

        private List<LineRenderer> lines = new List<LineRenderer>();

        private LineRenderer currentLineRender;

        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private GameObject editorObjectToTrackMovement;

        [SerializeField]
        private bool allowEditorControls = true;

        [SerializeField]
        private Material defaultLineMaterial;

        private bool IsPinchingReleased = false;

#region Oculus Types

        private OVRHand ovrHand;

        private OVRSkeleton ovrSkeleton;

        private OVRBone boneToTrack;
#endregion

        void Awake() 
        {
            ovrHand = objectToTrackMovement.GetComponent<OVRHand>();
            ovrSkeleton = objectToTrackMovement.GetComponent<OVRSkeleton>();
            
            #if UNITY_EDITOR
            
            // if we allow editor controls use the editor object to track movement because oculus
            // blocks the movement of LeftControllerAnchor and RightControllerAnchor
            if(allowEditorControls)
            {
                objectToTrackMovement = editorObjectToTrackMovement != null ? editorObjectToTrackMovement : objectToTrackMovement;
            }

            #endif
            // get initial bone to track
            boneToTrack = ovrSkeleton.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
                    .SingleOrDefault();

            VRLogInfo.Instance.LogInfo("boneToTrack is null: " + (boneToTrack == null));

            // add initial line rerderer
            AddNewLineRenderer();
        }

        void AddNewLineRenderer()
        {
            positionCount = 0;

            GameObject go = new GameObject($"LineRenderer_{handToTrack.ToString()}_{lines.Count}");
            go.transform.parent = objectToTrackMovement.transform.parent;
            go.transform.position = objectToTrackMovement.transform.position;

            LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
            goLineRenderer.startWidth = lineDefaultWidth;
            goLineRenderer.endWidth = lineDefaultWidth;
            goLineRenderer.useWorldSpace = true;
            goLineRenderer.material = defaultLineMaterial;
            goLineRenderer.positionCount = 1;
            goLineRenderer.numCapVertices = 5;

            currentLineRender = goLineRenderer;

            lines.Add(goLineRenderer);
        }

        void Update()
        {
            if (boneToTrack == null)
            {
                boneToTrack = ovrSkeleton.Bones
                    .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
                    .SingleOrDefault();

                objectToTrackMovement = boneToTrack.Transform.gameObject;
            }

            CheckPinchState();
        }

        private void CheckPinchState()
        {
            bool isIndexFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

            float indexFingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

            if(ovrHand.GetFingerConfidence(OVRHand.HandFinger.Index) != OVRHand.TrackingConfidence.High)
                return;

            // finger pinch down
            if (isIndexFingerPinching && indexFingerPinchStrength >= minFingerPinchDownStrength)
            {
                UpdateLine();
                IsPinchingReleased = true;
                return;
            }

            // finger pinch up
            if (IsPinchingReleased)
            {
                AddNewLineRenderer();
                IsPinchingReleased = false;
            }
        }

        void UpdateLine()
        {
            if(prevPointDistance == null)
            {
                prevPointDistance = objectToTrackMovement.transform.position;
            }

            if(prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, objectToTrackMovement.transform.position)) >= minDistanceBeforeNewPoint)
            {
                prevPointDistance = objectToTrackMovement.transform.position;
                AddPoint(prevPointDistance);
            }
        }

        void AddPoint(Vector3 position)
        {
            currentLineRender.SetPosition(positionCount, position);
            positionCount++;
            currentLineRender.positionCount = positionCount + 1;
            currentLineRender.SetPosition(positionCount, position);
        }

        public void UpdateLineWidth(float newValue)
        {
            currentLineRender.startWidth = newValue;
            currentLineRender.endWidth = newValue;
            lineDefaultWidth = newValue;
        }

        public void UpdateLineColor(Color color)
        {
            VRLogInfo.Instance.LogInfo("Color Update");

            currentLineRender.material.color = color;
            
            currentLineRender.material.EnableKeyword("_EMISSION");
            currentLineRender.material.SetColor("_EmissionColor", color);
            defaultColor = color;
            defaultLineMaterial.color = color;

            VRLogInfo.Instance.LogInfo("Color Updated");
        }

        public void UpdateLineMinDistance(float newValue)
        {
            minDistanceBeforeNewPoint = newValue;
        }
    }
}
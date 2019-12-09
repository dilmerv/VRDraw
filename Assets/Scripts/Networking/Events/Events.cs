using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class ClientPointsChanged : UnityEvent<string, Vector3[]>
{}

[System.Serializable]
public class ClientPointsAndColorChanged : UnityEvent<string, Color, Vector3[]>
{}

[System.Serializable]
public class ClientConnected : UnityEvent<string>
{}

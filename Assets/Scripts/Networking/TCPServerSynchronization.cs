using UnityEngine;

public class TCPServerSynchronization : MonoBehaviour
{
    public void Connected(string clientId)
    {
        Debug.Log($"Connected {clientId}");
    }
    
    public void ReceivePoints(string clientId, Vector3[] points)
    {
        Debug.Log($"ClientId = {clientId} Points = {points}");
    }
    
    public void ReceivePoints(string clientId, Color color, Vector3[] points)
    {
        Debug.Log($"ClientId = {clientId} Color = {color} Points = {points}");
    }    
}

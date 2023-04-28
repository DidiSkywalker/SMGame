
using UnityEngine;

public class GlobalCanvas : MonoBehaviour
{

    public static GameObject CanvasGameObject;
    public static Canvas Canvas;
    
    void Start()
    {
        CanvasGameObject = gameObject;
        Canvas = gameObject.GetComponent<Canvas>();
    }
}

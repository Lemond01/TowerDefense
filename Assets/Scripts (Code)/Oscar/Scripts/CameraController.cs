using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotación")]
    public float speedX = 5f;        // velocidad horizontal
    public float speedY = 3f;        // velocidad vertical (eje X)
    public float minY = 10f;         
    public float maxY = 80f;         

    private float rotX = 45f;        
    private float rotY = 0f;         

    void Start()
    {
        Vector3 e = transform.eulerAngles;
        rotX = e.x; rotY = e.y;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            rotY += Input.GetAxis("Mouse X") * speedX;
            rotX -= Input.GetAxis("Mouse Y") * speedY;
            rotX = Mathf.Clamp(rotX, minY, maxY);
            transform.rotation = Quaternion.Euler(rotX, rotY, 0f);
        }
    }
}
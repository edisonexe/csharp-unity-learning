using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1.25f;
    
    private float _rotationX;
    private float _rotationY;
    
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;
        
        _rotationY += mouseX;
        _rotationX -= mouseY;
        
        _rotationX = Mathf.Clamp(_rotationX, -20f, 20f);
        _rotationY = Mathf.Clamp(_rotationY, -30f, 30f);
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
}

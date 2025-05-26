using UnityEngine;

public class ConstrainMovement : MonoBehaviour 
{
    private float fixedValueY;
    private float fixedValueX = 0f;

    private void Start()
    {
        fixedValueY = transform.position.y;
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.y = fixedValueY;
        transform.position = pos;


        Vector3 currentRotation = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(fixedValueX, currentRotation.y, currentRotation.z);
    }
}

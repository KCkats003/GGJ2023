using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float targetSize;

    public float smoothTime = 0.3F;

    [System.Serializable]
    private class CameraPosition
    {
        public Transform target;
        public float size;
        public CameraPosition(Transform target, float size)
        {
            this.target = target;
            this.size = size;
        }
    }

    [SerializeField]
    private CameraPosition[] cameraPositions;

    // Start is called before the first frame update
    void Start()
    {
        target = transform;
        targetSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp the camera's position to the target's position
        transform.position = Vector3.Lerp(transform.position, target.position, smoothTime * Time.deltaTime);
        // Lerp the camera's size to the target's size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, smoothTime * Time.deltaTime);

    }

    public void UpdateTarget(int stage)
    {
        if (stage >= cameraPositions.Length)
        {
            return;
        }
        target = cameraPositions[stage].target;
        targetSize = cameraPositions[stage].size;
    }



}

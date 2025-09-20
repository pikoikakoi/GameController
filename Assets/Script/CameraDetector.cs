using UnityEngine;

public class CameraDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Plane[] cameraFrustum;
    [SerializeField] private Collider2D _collider;
    private bool hasInCamera = false;

    void Start()
    {
        _camera = Camera.main;

        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        var bounds = _collider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            hasInCamera = true;
        }
        else
        {
            if(hasInCamera)
            {
                Destroy(gameObject);
            }
            
        }
    }
}

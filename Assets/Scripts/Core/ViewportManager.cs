using UnityEngine;

public class ViewportManager : MonoBehaviour
{
    public static ViewportManager Instance;

    private float leftEdgeX;
    private float rightEdgeX;
    private float upEdgeY;
    private float bottomEdgeY;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Camera.main)
        {
            Debug.Log("Cannot found the main camera");
            return;
        }

        Camera mainCamera = Camera.main;
        
        leftEdgeX  = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        rightEdgeX = mainCamera.ViewportToWorldPoint(Vector3.right).x;
        upEdgeY = mainCamera.ViewportToWorldPoint(Vector3.up).y;
        bottomEdgeY = mainCamera.ViewportToWorldPoint(Vector3.zero).y;
    }

    public float GetLeftEdgeX(float threshold = 0.0f)
    {
        return leftEdgeX + threshold;
    }

    public float GetRightEdgeX(float threshold = 0.0f)
    {
        return rightEdgeX + threshold;
    }

    public float UpEdgeY(float threshold = 0.0f)
    {
        return upEdgeY + threshold;
    }

    public float BottomEdgeY(float threshold = 0.0f)
    {
        return bottomEdgeY + threshold;
    }

    public bool IsInsideViewport(Vector3 pos, float threshold = 0.0f)
    {
        if (pos.x <= leftEdgeX   - threshold ||
            pos.x >= rightEdgeX  + threshold ||
            pos.y <= bottomEdgeY - threshold ||
            pos.y >= upEdgeY     + threshold)
        {
            return false;
        }

        return true;
    }

    public void ClampInViewport(ref Vector3 pos, float threshold = 0.0f)
    {
        pos.x = pos.x <= leftEdgeX + threshold ? leftEdgeX + threshold : pos.x;
        pos.x = pos.x >= rightEdgeX - threshold ? rightEdgeX - threshold : pos.x;
        pos.y = pos.y >= upEdgeY - threshold ? upEdgeY - threshold : pos.y;
        pos.y = pos.y <= bottomEdgeY + threshold ? bottomEdgeY + threshold : pos.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

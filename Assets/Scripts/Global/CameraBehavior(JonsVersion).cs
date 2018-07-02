using UnityEngine;
using System.Collections;

public class CameraBehaviorJons: MonoBehaviour {

    #region World Bound support
    private Bounds mWorldBound;  // this is the world bound
    private Vector2 mWorldMin;  // Better support 2D interactions
    private Vector2 mWorldMax;
    private Vector2 mWorldCenter;
    #endregion

    public GameObject mZoomRef = null;
    private Camera mCamera = null;

	private StarBar_interaction star_bar = null;

    // Use this for initialization
    void Start () {
        mCamera = GetComponent<Camera>();

        // World bound support
        mWorldBound = new Bounds(Vector3.zero, Vector3.one);
        UpdateWorldWindowBound();
		this.star_bar = GameObject.Find ("StarBar").GetComponent<StarBar_interaction> ();
    }
	
	// Update is called once per frame
	void Update () {
        const float kZoomRate = 20f;
        const float kMoveRate = 30f;
        if (Input.GetKey(KeyCode.R))
        {
            mCamera.orthographicSize -= (kZoomRate * Time.smoothDeltaTime);
            UpdateWorldWindowBound();
        }
        if (Input.GetKey(KeyCode.T))
        {
            mCamera.orthographicSize += (kZoomRate * Time.smoothDeltaTime);
            UpdateWorldWindowBound();
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position += kMoveRate * Time.smoothDeltaTime * Vector3.up;
            UpdateWorldWindowBound();
        }
        if (Input.GetKey(KeyCode.V))
        {
            transform.position -= kMoveRate * Time.smoothDeltaTime * Vector3.up;
            UpdateWorldWindowBound();
        }
        if (Input.GetKey(KeyCode.C))
        {
            transform.position -= kMoveRate * Time.smoothDeltaTime * Vector3.right;
            UpdateWorldWindowBound();
        }
        if (Input.GetKey(KeyCode.B))
        {
            transform.position += kMoveRate * Time.smoothDeltaTime * Vector3.right;
            UpdateWorldWindowBound();
        }

        if (null != mZoomRef)
        {
            if (Input.GetKey(KeyCode.O))
            {
                Vector3 deltaV = mZoomRef.transform.position - transform.position;
                deltaV.z = 0;
                float deltaSizeY = (kZoomRate * Time.smoothDeltaTime);
                float deltaSizeX = mCamera.aspect * deltaSizeY;
                float oldH = mCamera.orthographicSize;
                float oldW = mCamera.orthographicSize * mCamera.aspect;
                mCamera.orthographicSize -= deltaSizeY;
                deltaV.x *= (mCamera.orthographicSize * mCamera.aspect) / oldW;
                deltaV.y *= mCamera.orthographicSize / oldH;
                Vector3 p = mZoomRef.transform.position - deltaV;
                p.z = transform.position.z;
                transform.position = p;
            }

            // optimizing from above code, recognizing 
            // W = aspect * H ,cancels in the deltaV.y ratio ...
            if (Input.GetKey(KeyCode.P))
            {
                Vector3 deltaV = mZoomRef.transform.position - transform.position;
                deltaV.z = 0;
                float deltaSize = (kZoomRate * Time.smoothDeltaTime);
                float oldH = mCamera.orthographicSize;
                mCamera.orthographicSize += deltaSize;
                deltaV *= mCamera.orthographicSize / oldH;
                Vector3 p = mZoomRef.transform.position - deltaV;
                p.z = transform.position.z;
                transform.position = p;
            }

            if (Input.GetKey(KeyCode.N))
            {
                Vector3 deltaV = mZoomRef.transform.position - mCamera.transform.position;
                deltaV.z = 0;
                float deltaSize = (kZoomRate * Time.smoothDeltaTime);
                deltaV *= (deltaSize / mCamera.orthographicSize);
                mCamera.orthographicSize -= deltaSize;
                mCamera.transform.position += deltaV;
            }
            if (Input.GetKey(KeyCode.M))
            {
                Vector3 deltaV = mZoomRef.transform.position - mCamera.transform.position;
                deltaV.z = 0;
                float deltaSize = (kZoomRate * Time.smoothDeltaTime);
                deltaV *= (deltaSize / mCamera.orthographicSize);
                mCamera.orthographicSize += deltaSize;
                mCamera.transform.position -= deltaV;
            }

        }
		this.star_bar.UpdateStarBarInCamera ();
    }


    void ZoomInTowards(Vector3 pos, float deltaSize)
    {
        Vector3 deltaV = pos - transform.position;
        deltaV.z = 0;
        deltaV.Normalize();
        float deltaSizeY = deltaSize;
        float deltaSizeX = mCamera.aspect * deltaSizeY;
        mCamera.orthographicSize -= deltaSizeY;
        deltaV.x *= deltaSizeX;
        deltaV.y *= deltaSizeY;
        mCamera.transform.position += deltaV;
    }

    /// <summary>
    /// Cameras the contains point: make sure the main mCamera contains this point within 95% of the mCamera boundary
    /// </summary>
    /// <param name='p'>
    /// P.
    /// </param>
    const float kCameraSafeZone = 1f - 0.90f; // 90%
    public void MoveCameraToContainPoint(Vector3 p)
    {
        Vector3 delta = mWorldBound.extents * kCameraSafeZone;
        Vector3 cameraPos = mCamera.transform.position;
        bool cameraMoved = false;
        if ((p.x + delta.x) > mWorldBound.max.x)
        {
            cameraPos.x += p.x + delta.x - mWorldBound.max.x;
            cameraMoved = true;
        }
        else if ((p.x - delta.x) < mWorldBound.min.x)
        {
            cameraPos.x += p.x - delta.x - mWorldBound.min.x;
            cameraMoved = true;
        }
        if ((p.y + delta.y) > mWorldBound.max.y)
        {
            cameraPos.y += p.y + delta.y - mWorldBound.max.y;
            cameraMoved = true;
        }
        else if ((p.y - delta.y) < mWorldBound.min.y)
        {
            cameraPos.y += p.y - delta.y - mWorldBound.min.y;
            cameraMoved = true;
        }
        if (cameraMoved)
        {
            transform.position = cameraPos;
            UpdateWorldWindowBound();
        }
    }

    #region Game Window World size bound support
    public enum WorldBoundStatus
    {
        CollideTop,
        CollideLeft,
        CollideRight,
        CollideBottom,
        Outside,
        Inside
    };

    /// <summary>
    /// This function must be called anytime the MainCamera is moved, or changed in size
    /// </summary>
    public void UpdateWorldWindowBound()
    {
        float maxY = mCamera.orthographicSize;
        float maxX = mCamera.orthographicSize * mCamera.aspect;
        float sizeX = 2 * maxX;
        float sizeY = 2 * maxY;
        float sizeZ = Mathf.Abs(mCamera.farClipPlane - mCamera.nearClipPlane);

        // Looking down the positive z-axis
        Vector3 c = mCamera.transform.position;
        c.z += 0.5f * sizeZ;
        mWorldBound.center = c;
        mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

        mWorldCenter = new Vector2(c.x, c.y);
        mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
        mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);
    }

    public Vector2 WorldCenter { get { return mWorldCenter; } }
    public Vector2 WorldMin { get { return mWorldMin; } }
    public Vector2 WorldMax { get { return mWorldMax; } }

    public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;

        if (mWorldBound.Intersects(objBound))
        {
            if (objBound.max.x > mWorldBound.max.x)
                status = WorldBoundStatus.CollideRight;
            else if (objBound.min.x < mWorldBound.min.x)
                status = WorldBoundStatus.CollideLeft;
            else if (objBound.max.y > mWorldBound.max.y)
                status = WorldBoundStatus.CollideTop;
            else if (objBound.min.y < mWorldBound.min.y)
                status = WorldBoundStatus.CollideBottom;
            else if ((objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
                status = WorldBoundStatus.Outside;
        }
        else {
            status = WorldBoundStatus.Outside;
        }
        return status;
    }

    public WorldBoundStatus ObjectClampToWorldBound(Transform t)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;
        Vector3 p = t.position;

        if (p.x > mWorldBound.max.x)
        {
            status = WorldBoundStatus.CollideRight;
            p.x = mWorldBound.max.x;
        }
        else if (t.position.x < mWorldBound.min.x)
        {
            status = WorldBoundStatus.CollideLeft;
            p.x = mWorldBound.min.x;
        }

        if (p.y > mWorldBound.max.y)
        {
            status = WorldBoundStatus.CollideTop;
            p.y = mWorldBound.max.y;
        }
        else if (p.y < mWorldBound.min.y)
        {
            status = WorldBoundStatus.CollideBottom;
            p.y = mWorldBound.min.y;
        }

        if ((p.z < mWorldBound.min.z) || (p.z > mWorldBound.max.z))
        {
            status = WorldBoundStatus.Outside;
        }

        t.position = p;
        return status;
    }
    #endregion
}

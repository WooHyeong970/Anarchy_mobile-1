using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera   gameCamera;
    float           orthoZoomSpeed = 0.03f;

    private void Start()
    {
        this.gameObject.transform.position = CentralProcessor.Instance.GetPlayer().GetCamPoint().position;
    }

    private void Update()
    {
        if(CentralProcessor.Instance.UI.state == UIManager.State.Idle)
        {
            if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude; 

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                if(gameCamera.orthographic)
                {
                    gameCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    gameCamera.orthographicSize = Mathf.Max(gameCamera.orthographicSize, 0.1f);
                }

                if(gameCamera.orthographicSize < 4)
                    gameCamera.orthographicSize = 4.1f;
                else if(gameCamera.orthographicSize > 8)
                    gameCamera.orthographicSize = 8;
            }
        }
    }
}
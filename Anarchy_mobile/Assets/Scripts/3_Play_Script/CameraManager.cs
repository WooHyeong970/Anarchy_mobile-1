using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera       GameCamera;
    public float        orthoZoomSpeed = 0.03f;

    private void Start()
    {
        this.gameObject.transform.position = CentralProcessor.Instance.player.getCamPoint().position;
    }

    private void Update()
    {
        if(CentralProcessor.Instance.uIManager.state == UIManager.State.Idle)
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

                if(GameCamera.orthographic)
                {
                    GameCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    GameCamera.orthographicSize = Mathf.Max(GameCamera.orthographicSize, 0.1f);
                }

                if(GameCamera.orthographicSize < 4)
                    GameCamera.orthographicSize = 4.1f;
                else if(GameCamera.orthographicSize > 8)
                    GameCamera.orthographicSize = 8;
            }
        }
    }
}
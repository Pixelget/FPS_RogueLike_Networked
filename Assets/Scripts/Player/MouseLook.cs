using System;
using UnityEngine;

[Serializable]
public class MouseLook: MonoBehaviour {
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    public GameObject CameraMount;


    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;


    public void Init(GameObject CamMount)
    {
        CameraMount = CamMount;
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = CameraMount.transform.localRotation;

        //Cursor.visible = false;
    }


    public void LookRotation()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

        if(clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

        if(smooth)
        {
            transform.localRotation = Quaternion.Slerp (transform.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
            CameraMount.transform.localRotation = Quaternion.Slerp (CameraMount.transform.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            transform.localRotation = m_CharacterTargetRot;
            CameraMount.transform.localRotation = m_CameraTargetRot;
        }

        //Cursor.lockState = CursorLockMode.Locked;
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}


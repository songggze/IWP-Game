using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera camera;
    CinemachineCameraOffset vcam;

    [SerializeField] GameObject player;
    PlayerStats playerStats;

    [SerializeField] float moveSpeed = 120;
    [SerializeField] float moveDistance = 2;
    [SerializeField] double shakeLength = 0.1f;

    [SerializeField] bool isShaking;
    [SerializeField] bool moveUpward;

    double shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        vcam = camera.gameObject.AddComponent<CinemachineCameraOffset>();
        playerStats = player.GetComponent<PlayerStats>();

        isShaking = false;
        moveUpward = true;

        shakeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (shakeTimer > 0 && isShaking){

            // Apply shake effect
            if (moveUpward){
                vcam.m_Offset += new Vector3(moveSpeed, moveSpeed, 0) * Time.deltaTime;
            }
            else {
                vcam.m_Offset -= new Vector3(moveSpeed, moveSpeed, 0) * Time.deltaTime;
            }

            // Change direction
            if (vcam.m_Offset.x > moveDistance && vcam.m_Offset.y > moveDistance)
            {
                moveUpward = false;
            }
            else if (vcam.m_Offset.x < -moveDistance && vcam.m_Offset.y < -moveDistance)
            {
                moveUpward = true;
            }

            shakeTimer -= Time.deltaTime;

            // Disable effect
            if (shakeTimer <= 0){

                vcam.m_Offset = Vector3.zero;
                moveUpward = true;
                isShaking = false;
            }
        }

    }

    public void SetShake()
    {
        // Enable shaking when player hurt
        isShaking = true;
        shakeTimer = shakeLength;
    }
}

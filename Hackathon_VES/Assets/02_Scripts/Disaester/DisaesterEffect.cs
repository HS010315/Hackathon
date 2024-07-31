using System.Collections;
using UnityEngine;

public class DisaesterEffect : MonoBehaviour, IEffectable, IDamagable
{
    public GameObject projectilePrefab;   // 발사할 프리팹
    public Transform[] targets;           // 목표 지점 배열
    public Transform launchPoint;         // 발사 지점
    public float launchSpeed = 10f;       // 발사 속도
    public float launchInterval = 1.0f;   // 발사 간격 (초)
    public float initialAngle = 45f;      // 발사 각도 (도)
    public float gravityScale = 2f;       // 중력 배율

    public float shakeMagnitude = 0.1f;
    public float earthquakeDuration = 10.0f;
    public float damageTime = 5.0f;
    public PlayerStateInfo playerStateInfo;
    public CameraShake cameraShake;
    private bool earthquakeActive = false;
    private int disaesterCase = 0;

    void Start()
    {
        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LaunchProjectiles());
        }
    }

    public void DisaesterEvent(DisaesterList disaesterType)
    {
        switch (disaesterType)
        {
            case DisaesterList.Earthquake:
                disaesterCase = 1;
                break;
            case DisaesterList.VolcanicBomb:
                disaesterCase = 2;
                break;
            case DisaesterList.Lava:
                disaesterCase = 3;
                break;
        }
    }

    public void EffectToPlayer(PlayerStateInfo playerStateInfo)
    {
        playerStateInfo.Panic += 20;
    }

    public void DamageToPlayer(PlayerStateInfo playerStateInfo)
    {
        if (disaesterCase == 1 && !earthquakeActive)
        {
            StartCoroutine(EarthquakeCoroutine());
        }
        else if (disaesterCase == 2)
        {
            StartCoroutine(LaunchProjectiles());
        }
        else if (disaesterCase == 3)
        {

        }
    }

    private IEnumerator EarthquakeCoroutine()
    {
        Debug.Log("지진 시작");
        earthquakeActive = true;
        playerStateInfo.WakeUp();
        StartCoroutine(cameraShake.Shake(earthquakeDuration, shakeMagnitude));

        yield return new WaitForSeconds(damageTime);

        if (playerStateInfo.CurrentState != PlayerState.Crouching)
        {
            playerStateInfo.ChangeState(PlayerState.Falling);
            playerStateInfo.Hp -= 20;
        }

        yield return new WaitForSeconds(earthquakeDuration - damageTime);

        earthquakeActive = false;
        playerStateInfo.ChangeState(PlayerState.Idle);
    }

    private IEnumerator LaunchProjectiles()
    {
        foreach (Transform target in targets)
        {
            LaunchProjectile(target);
            yield return new WaitForSeconds(launchInterval);  // 지정된 간격만큼 대기
        }
    }

    void LaunchProjectile(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.useGravity = false;  // 기본 중력 사용 안 함

        Vector3 velocity = GetVelocity(launchPoint.position, target.position, initialAngle);
        rb.velocity = velocity;

        // 인공 중력 적용
        StartCoroutine(ApplyArtificialGravity(rb));
    }

    IEnumerator ApplyArtificialGravity(Rigidbody rb)
    {
        while (rb != null)
        {
            rb.AddForce(Vector3.down * Physics.gravity.magnitude * gravityScale, ForceMode.Acceleration);
            yield return null;
        }
    }

    public Vector3 GetVelocity(Vector3 launch, Vector3 targetPosition, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude * gravityScale;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        Vector3 planarPosition = new Vector3(launch.x, 0, launch.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = launch.y - targetPosition.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPosition.x > launch.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
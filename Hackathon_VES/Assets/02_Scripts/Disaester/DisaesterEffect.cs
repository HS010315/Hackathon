using System.Collections;
using UnityEngine;

public class DisaesterEffect : MonoBehaviour, IEffectable, IDamagable
{
    public GameObject volcanicBombPrefab;
    public Transform launchPoint;
    public Transform[] targetPoints; // 여러 도착 지점
    public int numBombsPerLaunch = 3;
    public int numLaunches = 5;
    public float minLaunchAngle = 30f;
    public float maxLaunchAngle = 60f;
    public float launchPower = 100f;
    public float gravity = -9.8f;

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
            StartCoroutine(LaunchVolcanicBombs());
        }
        else if (disaesterCase == 3)
        {

        }
    }

    private IEnumerator EarthquakeCoroutine()
    {
        Debug.Log("지진 시작");
        earthquakeActive = true;

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
    private IEnumerator LaunchVolcanicBombs()
    {
        Debug.Log("Launching volcanic bombs...");

        Vector3 launchPointPosition = launchPoint.position;

        for (int i = 0; i < numLaunches; i++)
        {
            for (int j = 0; j < numBombsPerLaunch; j++)
            {
                // 랜덤하게 선택된 도착 지점
                Transform targetPoint = targetPoints[Random.Range(0, targetPoints.Length)];

                GameObject bomb = Instantiate(volcanicBombPrefab, launchPointPosition, launchPoint.rotation);
                Rigidbody rb = bomb.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;

                    Vector3 direction = (targetPoint.position - launchPointPosition).normalized;
                    float distance = Vector3.Distance(launchPointPosition, targetPoint.position);

                    float launchAngleRad = Random.Range(minLaunchAngle, maxLaunchAngle) * Mathf.Deg2Rad;
                    float initialVelocityY = Mathf.Sqrt(-2 * gravity * (distance / Mathf.Sin(2 * launchAngleRad)));
                    float timeToTarget = distance / (launchPower * Mathf.Cos(launchAngleRad));
                    float initialVelocityXZ = distance / timeToTarget;

                    Vector3 initialVelocity = new Vector3(direction.x * initialVelocityXZ, initialVelocityY, direction.z * initialVelocityXZ);
                    rb.velocity = initialVelocity;

                    Debug.Log($"Bomb {i * numBombsPerLaunch + j + 1}: Target position: {targetPoint.position}, Initial velocity: {initialVelocity}");
                }
                else
                {
                    Debug.LogWarning("Rigidbody not found on volcanicBombPrefab!");
                }
                yield return new WaitForSeconds(Random.Range(0f, 1f));
            }
            yield return new WaitForSeconds(Random.Range(0f, 1f));
        }

        Debug.Log("Volcanic bomb launching complete.");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Ant : MonoBehaviour
{

    [SerializeField]
    private int ArrivedDistance = 3;

    [SerializeField]
    private float WanderTimer = 10;

    private float Timer;

    // The amount of time to require before we create a new destination
    [SerializeField]
    private float DestinationCreateTime = 2;

    [SerializeField]
    private Vector3 Destination;

    [SerializeField]
    private int Range = 47;

    [SerializeField]
    private int Speed = 5;

    private bool IsNearAntHill = false;

    [SerializeField]
    private float AntennaDistance = 2f;

    [SerializeField]
    private bool IsDebug = false;

    [SerializeField]
    private bool StopMovement = false;

    [SerializeField]
    private float TurningSpeed = 1;

    [SerializeField]
    private Color Color;

    // Start is called before the first frame update
    void Start()
    {
        // this makes the first wander automatically run.
        Timer = WanderTimer;
        Wander();
        LookAt(Destination, true);
        Material Material = GetComponent<Renderer>().material;
        Color = Random.ColorHSV();
        Material.color = Color;
    }

    // Update is called once per frame
    void Update()
    {
        Wander();

        CheckAntennaCollisions();

        if (!AtDestination() && !StopMovement)
        {
            LookAt(Destination);
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    void Wander(bool force = false)
    {
        Timer += Time.deltaTime;

        if ((Timer >= WanderTimer || AtDestination() || force) && Timer >= DestinationCreateTime)
        {

            if (force)
            {
                // we want to get a new destination that is behind us as we have just run into something
                Destination = RandomPointInCircle(transform, Range, 120, 150);
            }
            else
            {
                Destination = new Vector3(
                    Random.Range(transform.position.x - Range, transform.position.x + Range),
                    0.1f,
                    Random.Range(transform.position.z - Range, transform.position.z + Range)
                );
            }
            Timer = 0;
        }
    }

    void LookAt(Vector3 target, bool immediately = false)
    {
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        if (immediately)
        {
            transform.rotation = rotation;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);
        }
    }

    bool AtDestination()
    {
        return (transform.position - Destination).magnitude < ArrivedDistance;
    }

    private void OnDrawGizmos()
    {
        if (IsDebug)
        {
            Vector3 mid = transform.position + transform.up * 0.5f + transform.forward * 0.5f;
            Gizmos.DrawLine(mid, mid + transform.forward);

            //Gizmos.DrawWireSphere(transform.position, FollowDistance);
            //Gizmos.DrawWireSphere(transform.position, Range);
            Gizmos.color = Color;
            Gizmos.DrawSphere(Destination, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.AntHill)
        {
            IsNearAntHill = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.AntHill)
        {
            //IsNearAntHill = false;
        }
    }

    private void CheckAntennaCollisions()
    {
        if (!IsNearAntHill)
        {
            foreach (Vector3 antennaPosition in AntennaPositions())
            {
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, (transform.position - antennaPosition).normalized, out hit, AntennaDistance))
                {
                    if (IsDebug) Debug.DrawRay(transform.position, (transform.position - antennaPosition).normalized * hit.distance, Color.red);

                    Wander(true);
                }
            }
        }
    }

    private IEnumerable<Vector3> AntennaPositions()
    {
        for (int angle = 135; angle <= 225; angle+=10)
        {
            float rad = Mathf.Deg2Rad * angle;
            Vector3 position = transform.right * Mathf.Sin(rad) + transform.forward * Mathf.Cos(rad);
            yield return transform.position + position * AntennaDistance;
        }
    }

    Vector3 RandomPointInCircle(Transform trans, float radius, float angleMin, float angleMax)
    {
        float angle = Random.Range(angleMin, angleMax);
        float rad = angle * Mathf.Deg2Rad;
        Vector3 position = trans.right * Mathf.Sin(rad) + trans.forward * Mathf.Cos(rad);
        return trans.position + position * radius;
    }
}

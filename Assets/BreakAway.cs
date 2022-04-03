using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAway : MonoBehaviour
{
    public float shootAwayForce;
    public float rotationForce;
    
    public void Break()
    {
        var body = gameObject.AddComponent<Rigidbody2D>();
        body.mass = 0.5f;

        var direction = (new Vector2(Random.Range(-1f, 1f), 1)).normalized;
        body.AddForce(direction*shootAwayForce);
        body.AddTorque(rotationForce * Random.value > 0.5 ? 1 : -1, ForceMode2D.Impulse);
    }
}

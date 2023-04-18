using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
    [SerializeField, Range(5, 50)] private float _speed;
    [SerializeField] private LayerMask _obstacle;

    public GameObjectData Data { get; private protected set; }
    public Transform Target { get; private protected set; }
    private Vector2[] directions =
    {
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left,
        (Vector2.up + Vector2.left).normalized,
        (Vector2.up + Vector2.right).normalized,
        (Vector2.down + Vector2.left).normalized,
        (Vector2.down + Vector2.right).normalized
    };

    private Rigidbody2D rb2d;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(Target == null) return;
        Vector3 direction = GoToTarget() + DetectObstacle();
        
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y, 
            Mathf.Atan2((transform.position.y + rb2d.velocity.y) - transform.position.y, (transform.position.x + rb2d.velocity.x) - transform.position.x) * Mathf.Rad2Deg - 90);

        rb2d.velocity = direction;
    }

    private Vector3 GoToTarget() => (Target.position - transform.position).normalized * _speed * Time.fixedDeltaTime;

    private Vector3 DetectObstacle()
    {
        if (Vector3.Distance(transform.position, Target.position) < 2) return Vector2.zero;
        Vector2 direction = new Vector2();
        foreach (Vector2 v2 in directions)
        {
            Vector2 v = transform.TransformDirection(v2);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, v, .5f, _obstacle);
            if (hit.collider != null)
            {
                Vector2 reverse = (Vector2)(transform.position - hit.collider.transform.position).normalized;
                direction += reverse * _speed * Time.fixedDeltaTime;
            }
        }
        return direction;
    }
    public void SetTarget(Transform target) => Target = target;

    public void SetData(GameObjectData data)
    {
        GetComponent<SpriteRenderer>().color = data.GetObjectColor;
        Data = data;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out Planet p))
        {
            p.Capture(this);
        }
    }
}
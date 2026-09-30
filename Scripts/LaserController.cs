using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{


    [SerializeField] private float defDistanceRay = 100f;
    public LineRenderer m_lineRenderer;
    public GameObject prefVfx;
    public GameObject objDestroyVfx;
    private GameObject vfx;
    Transform m_transform;
    // Start is called before the first frame update
    void Awake()
    {
        m_transform = GetComponent<Transform>();
        vfx = Instantiate(prefVfx,transform);
        
    }

    private void Update()
    {
        ShooterLaser();
    }
    void ShooterLaser() 
    {
        if (Physics2D.Raycast(m_transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, transform.right);
            if (_hit.rigidbody.gameObject.tag == "Bullet" || _hit.rigidbody.gameObject.tag == "Buff") 
            {
                Destroy(_hit.transform.gameObject);
                Instantiate(objDestroyVfx, _hit.point, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
                Camera.main.transform.GetComponents<AudioSource>()[11].Play();
            }
            if (_hit.rigidbody.gameObject.tag == "Player")
            {
                _hit.rigidbody.gameObject.GetComponent<PlayerController>().DestroyTank();
            }
            Draw2DRay(m_transform.position, _hit.point);
            vfx.transform.position = _hit.point;
            
        }
        else 
        {
            //Draw2DRay(m_transform.position, transform.right * defDistanceRay);
        }
    }

    private void Draw2DRay(Vector2 startPosition, Vector2 endPosition)
    {
        m_lineRenderer.SetPosition(0, startPosition);
        m_lineRenderer.SetPosition(1, endPosition);

    }
}

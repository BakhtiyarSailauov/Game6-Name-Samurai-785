using UnityEngine;

public class Shablon_Move_Controller2 : MonoBehaviour
{
    Transform Target;
    private float speed = 0.05f;

    private void Update()
    {
        if (Input.touchCount == 1) SetTarget();
        if (Input.GetMouseButtonDown(0)) Move();
        if (Target) Move();
    }


    void SetTarget()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider == null) return;
           // GameObject newTarget = Instantiate(Resources.Load("T_5_arrow_up_"), hit.point, Quaternion.identity) as GameObject;
            Target = hit.transform;
        }
    }
    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
    }
}

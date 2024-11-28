using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCol : MonoBehaviour
{
    [SerializeField] BoxCollider2D col;
    public float rayLength = 10f; // 레이 길이
    public LayerMask collisionLayer;

    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private Vector2[] hitPoints = new Vector2[4]; // 충돌 지점을 저장할 배열
    private void Start()
    {
        
    }

    private void LateUpdate()
    { 
       // CreateRectangleCollider();
       CircleRayCast(); 
    }

    private void CircleRayCast() 
    {
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, 0.1f, Vector2.up, rayLength);
        if (ray.transform.gameObject != this.gameObject) 
        {
            if (ray.transform.gameObject.layer == 7)
            {
            Debug.Log("플레이어 힛");

            }
            else 
            {
            Debug.Log("플레이어 없음");
            }

        }
        
    }

    private void CreateRectangleCollider()
    {
        // 레이캐스트 실행
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], rayLength, collisionLayer);
            Debug.DrawRay(transform.position, directions[i], Color.red);
            if (hit.collider != null)
            {
                hitPoints[i] = hit.point; // 충돌 지점 저장
            }
            else
            {
                if (i == 0)
                {
                    hitPoints[i] = new Vector2(transform.position.x, transform.position.y+rayLength);
                }
                else if (i == 1) 
                {
                    hitPoints[i] = new Vector2(transform.position.x, transform.position.y-rayLength);
                }
                else if (i == 2)
                {
                    hitPoints[i] = new Vector2(transform.position.x-rayLength, transform.position.y);
                }
                else if (i == 3)
                {
                    hitPoints[i] = new Vector2(transform.position.x+rayLength, transform.position.y);
                }

            }
        }

        // 사각형 중심과 크기 계산
        Vector2 center = new Vector2(
            (hitPoints[2].x + hitPoints[3].x) / 2, // 왼쪽과 오른쪽의 중간 x값
            (hitPoints[0].y + hitPoints[1].y) / 2  // 위쪽과 아래쪽의 중간 y값
        );

        Vector2 size = new Vector2(
            Mathf.Abs(hitPoints[3].x - hitPoints[2].x), // 오른쪽 - 왼쪽
            Mathf.Abs(hitPoints[0].y - hitPoints[1].y)  // 위 - 아래
        );

        // 기존 BoxCollider2D가 있다면 제거
        
        
        col.offset = center - (Vector2)transform.position; // 로컬 위치로 오프셋 설정
        col.size = size;

        Debug.Log($"Rectangle Collider created at Center: {center}, Size: {size}");
    }
}

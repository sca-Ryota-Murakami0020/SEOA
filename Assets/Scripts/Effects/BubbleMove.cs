using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMove : MonoBehaviour
{
    [SerializeField, Tooltip("初期の向き")]
    private Vector2 direction;

    [SerializeField, Tooltip("速度")]
    private float speed;

    [SerializeField, Tooltip("上バウンド有効化")]
    private bool can_top_bound = true;
    [SerializeField, Tooltip("下バウンド有効化")]
    private bool can_bottom_bound = true;
    [SerializeField, Tooltip("右バウンド有効化")]
    private bool can_right_bound = true;
    [SerializeField, Tooltip("左バウンド有効化")]
    private bool can_left_bound = true;

    //壁に当たっているかどうか
    private bool is_hit_wall = false;

// Start is called before the first frame update
void Start()
{
        direction = Random.insideUnitCircle.normalized;
}

// Update is called once per frame
void Update()
{
    BoundWall(Camera.main);
    //位置更新
    transform.position += GetVelocity(direction, speed);
    //オブジェクトの向き更新
    transform.rotation = LookDirection(direction);
}

void BoundWall(Camera camera)
{
    is_hit_wall = false;

    Vector3 change_position = transform.position;
    float top = Camera.main.transform.position.y + GetHalfHeight(camera);
    float bottom = Camera.main.transform.position.y - GetHalfHeight(camera);
    float right = Camera.main.transform.position.x + GetHalfWidth(camera);
    float left = Camera.main.transform.position.x - GetHalfWidth(camera);

    //左の壁判定
    if (transform.position.x <= left && can_left_bound)
    {
        direction.x *= -1.0f;
        change_position.x = left + float.Epsilon;
        is_hit_wall = true;
    }
    //右の壁判定
    else if (transform.position.x >= right && can_right_bound)
    {
        direction.x *= -1.0f;
        change_position.x = right - float.Epsilon;
        is_hit_wall = true;
    }
    //上の壁判定
    if (transform.position.y >= top && can_top_bound)
    {
        direction.y *= -1.0f;
        change_position.y = top - float.Epsilon;
        is_hit_wall = true;
    }
    //下の壁判定
    else if (transform.position.y <= bottom && can_bottom_bound)
    {
        direction.y *= -1.0f;
        change_position.y = bottom + float.Epsilon;
        is_hit_wall = true;
    }

    transform.position = change_position;
}

//向きと速度から速度ベクトルを計算する
Vector3 GetVelocity(Vector2 direction, float speed)
{
    return direction * speed * Time.deltaTime;
}

//指定した方向を返す
Quaternion LookDirection(Vector2 direction)
{
    return Quaternion.FromToRotation(Vector3.up, direction);
}

//カメラの横のサイズの半分を取得
float GetHalfWidth(Camera camera)
{
    return camera.orthographicSize * camera.aspect;
}
//カメラの縦のサイズの半分を取得
float GetHalfHeight(Camera camera)
{
    return camera.orthographicSize;
}

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMove : MonoBehaviour
{
    [SerializeField, Tooltip("�����̌���")]
    private Vector2 direction;

    [SerializeField, Tooltip("���x")]
    private float speed;

    [SerializeField, Tooltip("��o�E���h�L����")]
    private bool can_top_bound = true;
    [SerializeField, Tooltip("���o�E���h�L����")]
    private bool can_bottom_bound = true;
    [SerializeField, Tooltip("�E�o�E���h�L����")]
    private bool can_right_bound = true;
    [SerializeField, Tooltip("���o�E���h�L����")]
    private bool can_left_bound = true;

    //�ǂɓ������Ă��邩�ǂ���
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
    //�ʒu�X�V
    transform.position += GetVelocity(direction, speed);
    //�I�u�W�F�N�g�̌����X�V
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

    //���̕ǔ���
    if (transform.position.x <= left && can_left_bound)
    {
        direction.x *= -1.0f;
        change_position.x = left + float.Epsilon;
        is_hit_wall = true;
    }
    //�E�̕ǔ���
    else if (transform.position.x >= right && can_right_bound)
    {
        direction.x *= -1.0f;
        change_position.x = right - float.Epsilon;
        is_hit_wall = true;
    }
    //��̕ǔ���
    if (transform.position.y >= top && can_top_bound)
    {
        direction.y *= -1.0f;
        change_position.y = top - float.Epsilon;
        is_hit_wall = true;
    }
    //���̕ǔ���
    else if (transform.position.y <= bottom && can_bottom_bound)
    {
        direction.y *= -1.0f;
        change_position.y = bottom + float.Epsilon;
        is_hit_wall = true;
    }

    transform.position = change_position;
}

//�����Ƒ��x���瑬�x�x�N�g�����v�Z����
Vector3 GetVelocity(Vector2 direction, float speed)
{
    return direction * speed * Time.deltaTime;
}

//�w�肵��������Ԃ�
Quaternion LookDirection(Vector2 direction)
{
    return Quaternion.FromToRotation(Vector3.up, direction);
}

//�J�����̉��̃T�C�Y�̔������擾
float GetHalfWidth(Camera camera)
{
    return camera.orthographicSize * camera.aspect;
}
//�J�����̏c�̃T�C�Y�̔������擾
float GetHalfHeight(Camera camera)
{
    return camera.orthographicSize;
}

}


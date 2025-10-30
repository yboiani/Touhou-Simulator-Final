using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float percentOfSpeed = 20.0f;

    private Material _material;
    private static readonly int Speed = Shader.PropertyToID("_Speed");

    private float prevSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _material = GetComponent<SpriteRenderer>().material;
        prevSpeed = _player.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_player.Speed - prevSpeed) <= float.Epsilon) return;

        prevSpeed = _player.Speed;
        _material.SetFloat(Speed, prevSpeed * (percentOfSpeed / 100.0f));
    }
}

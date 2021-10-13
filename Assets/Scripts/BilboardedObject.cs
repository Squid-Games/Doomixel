using UnityEngine;

public class BilboardedObject : MonoBehaviour
{
    private GameObject _gameLogic;
    private GameObject _playerObject;

    void Start()
    {
        _gameLogic = GameObject.FindGameObjectWithTag("GameController");
        _playerObject = _gameLogic.GetComponent<GameLogic>().playerObject;
    }

    void Update()
    {
        var playerTransform = _playerObject.transform;
        var direction = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.z - transform.position.z);
        transform.eulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y), transform.localEulerAngles.z);
    }
}

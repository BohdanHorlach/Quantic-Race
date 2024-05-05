using UnityEngine;
using Photon.Pun;


public class MiniMapIconSeter : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;
    

    void Start()
    {
        if(_photonView.IsMine == true)
		_sprite.color = _playerColor;
	else
		_sprite.color = _enemyColor;
    }
}

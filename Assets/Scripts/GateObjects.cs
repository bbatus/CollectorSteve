using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

 public enum PoolType
{
    GreenPool,
    YellowPool,
    RedPool
}

public class GateObjects : MonoBehaviour
{
    public PoolType poolType;
    [SerializeField]

    bool isUpgrade = false;
    [SerializeField] Collider collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            collider.enabled = false;
            other.transform.root.GetComponent<PlayerMove>().BringObjectToPool(this.gameObject, poolType);
            UIManager.instance.levelState = LevelState.Win;
            UIManager.instance.ConfettiSetActive(true);

}
    }
}

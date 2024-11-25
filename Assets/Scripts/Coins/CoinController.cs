using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private CoinConnectionHandler coinConnectionHandler;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform visual;

    [Header("Settings")]
    [SerializeField, Range (1,5)] private int score;

    public int Score => score;

    private void OnEnable()
    {
        coinConnectionHandler.OnCollection += CoinConnectionHandler_OnCollection;
    }
    private void OnDisable()
    {
        coinConnectionHandler.OnCollection -= CoinConnectionHandler_OnCollection;
    }

    public void CollectCoin()
    {
        _collider.enabled = false;
        visual.gameObject.SetActive(false);
    }

    private void CoinConnectionHandler_OnCollection(object sender, CoinConnectionHandler.OnCollectionEventArgs e)
    {
        CollectCoin();
    }
}

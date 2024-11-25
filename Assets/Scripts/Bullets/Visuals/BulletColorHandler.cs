using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColorHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BulletConnectionHandler bulletConnectionHandler;
    [SerializeField] private Renderer bulletRenderer;
    [Space]
    [SerializeField] private List<BulletMaterial> bulletMaterials;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Serializable]
    public class BulletMaterial
    {
        public BulletColor bulletColor;
        public Material material;
    }

    private void OnEnable()
    {
        bulletConnectionHandler.OnConnection += BulletConnectionHandler_OnConnection;
    }

    private void OnDisable()
    {
        bulletConnectionHandler.OnConnection -= BulletConnectionHandler_OnConnection;
    }

    private void SetMaterialByBulletColor(BulletColor bulletColor)
    {
        Material mat = GetMaterialByBulletColor(bulletColor);

        if (!mat)
        {
            if (debug) Debug.Log($"No material was found for BulletColor :{bulletColor}");
        }

        bulletRenderer.material = mat;
    }

    private Material GetMaterialByBulletColor(BulletColor bulletColor)
    {
        foreach (BulletMaterial bulletMaterial in bulletMaterials)
        {
            if (bulletMaterial.bulletColor == bulletColor)
            {
                return bulletMaterial.material;
            }
        }

        return null;
    }


    private void BulletConnectionHandler_OnConnection(object sender, BulletConnectionHandler.OnConnectionEventArgs e)
    {
        SetMaterialByBulletColor(e.bulletInfo.bulletColor);
    }
}

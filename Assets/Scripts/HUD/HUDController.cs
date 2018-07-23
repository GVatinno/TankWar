using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField]
    Text mAimValue = null;
    [SerializeField]
    Text mPowerValue = null;
    [SerializeField]
    Text mTankName = null;

    void Awake () {
        MessageBus.Instance.AimUpdated += OnAimUpdate;
        MessageBus.Instance.PowerUpdated += OnPowerUpdate;
        MessageBus.Instance.StartTankAttack += OnAttack;
        MessageBus.Instance.GameStarted += OnGameStarted;
    }
	
	void Destroy () {
        MessageBus.Instance.AimUpdated -= OnAimUpdate;
        MessageBus.Instance.PowerUpdated -= OnPowerUpdate;
        MessageBus.Instance.StartTankAttack -= OnAttack;
        MessageBus.Instance.GameStarted -= OnGameStarted;
    }

    void OnGameStarted()
    {
        OnTankNameChanged("");
        OnAimUpdate(0.0f);
        OnPowerUpdate(0.0f);
    }

    void OnAttack(Tank tank)
    {
        OnTankNameChanged(tank.mName);
        OnAimUpdate(tank.currentAim);
        OnPowerUpdate(tank.currentPower);
    }

    void OnTankNameChanged(string name)
    {
        mTankName.text = name;
    }

    void OnAimUpdate( float aim )
    {
        mAimValue.text = System.Math.Round(aim,2).ToString();
    }

    void OnPowerUpdate( float power )
    {
        mPowerValue.text = System.Math.Round(power,2).ToString();
    }
}

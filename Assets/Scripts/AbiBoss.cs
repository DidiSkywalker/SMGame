using AbiBossAttacks;
using UnityEngine;

public class AbiBoss : MonoBehaviour
{

    public AbiBossAttack currentAttack;
    private bool _attackActive;

    private void Update()
    {
        if (_attackActive)
        {
            currentAttack.Update();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attackActive)
            {
                _attackActive = false;
                currentAttack.Destroy();
            }
            else
            {
                _attackActive = true;
                currentAttack.Use(gameObject);
            }
        }
    }
}

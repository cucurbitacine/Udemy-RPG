using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    public class WeaponEquipped : MonoBehaviour
    {
        public WeaponModel model;

        [Header("SFX")]
        public SfxEffect sfx;
        
        public void SetModel(WeaponModel weaponModel)
        {
            model = Instantiate(weaponModel);
            model.name = weaponModel.name; // IMPORTANT
        }

        public void Sfx()
        {
            if (sfx)
            {
                sfx.Play();
            }
        }
    }
}
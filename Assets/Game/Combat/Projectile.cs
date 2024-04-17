using UnityEngine;

namespace Game.Combat
{
    public class Projectile : MonoBehaviour
    {
        public float damage = 0f;
        public float speed = 20;
        public float lifetime = 10f;

        [Space]
        public float lifetimeAfterHit = 5f;
        public GameObject[] destroyOnHit = null;
        
        [Space]
        public bool canChaseTarget = false;
        public GameObject impactEffectPrefab;
        
        [Space] public CombatTarget target;
        
        private Coroutine _shooting;
        private float _time;
        private bool _hidden;
        
        public void Shoot(CombatTarget combatTarget, float amountDamage)
        {
            gameObject.SetActive(true);

            damage = amountDamage;
            target = combatTarget;

            transform.LookAt(GetAim(target));
            
            _time = 0f;
        }

        public void Damage(CombatTarget combatTarget, Vector3 point, Vector3 normal)
        {
            if (combatTarget.health.isDied) return;
            
            combatTarget.health.Damage(damage);

            Impact(point, normal);
            
            Hide();
        }
        
        public void Damage(CombatTarget combatTarget)
        {
            Damage(combatTarget, transform.position, -transform.forward);
        }
        
        public void Hide()
        {
            if (_hidden) return;
            _hidden = true;
            _time = 0f;

            if (destroyOnHit != null)
            {
                foreach (var destroy in destroyOnHit)
                {
                    Destroy(destroy);
                }
            }
            
            Destroy(gameObject, lifetimeAfterHit);
        }

        private void Impact(Vector3 point, Vector3 normal)
        {
            if (impactEffectPrefab)
            {
                var rotation = Quaternion.LookRotation(-normal, Vector3.up);
                var impact = Instantiate(impactEffectPrefab, point, rotation);
                if (impact.TryGetComponent<ParticleSystem>(out var effect))
                {
                    if (!effect.main.playOnAwake)
                    {
                        effect.Play();
                    }

                    if (!impact.TryGetComponent<DestroyEffect>(out var destroyer))
                    {
                        destroyer = impact.AddComponent<DestroyEffect>();
                    }
                }
            }
        }
        
        private static Vector3 GetAim(Component target)
        {
            if (target.TryGetComponent<Collider>(out var cld))
            {
                return cld.bounds.center;
            }

            return target.transform.position;
        }
        
        private void Update()
        {
            if (target && _time < lifetime)
            {
                _time += Time.deltaTime;
                
                if (canChaseTarget)
                {
                    transform.LookAt(GetAim(target));
                }
                
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }
            else
            {
                Hide();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target && other.TryGetComponent<CombatTarget>(out var combatTarget) && target == combatTarget)
            {
                Damage(combatTarget);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (target)
            {
                var contact = other.GetContact(0);
                
                if (contact.thisCollider.TryGetComponent<CombatTarget>(out var combatTarget) && target == combatTarget)
                {
                    Damage(combatTarget, contact.point, contact.normal);
                }
            }
        }
    }
}
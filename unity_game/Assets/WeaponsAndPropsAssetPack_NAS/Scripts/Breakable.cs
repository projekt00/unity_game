using System.Collections;
using UnityEngine;

namespace WeaponsAndPropsAssetPack_NAS.Scripts
{
    public class Breakable : MonoBehaviour
    {
        [SerializeField] private Transform wholeObject;
        [SerializeField] private Transform fracturedObject;
        [SerializeField] private bool isCyclic;

        // Core Variables
        private bool isBroken;
        private bool isClean;
        private bool objectReseted = true;
        private Transform fracturedObjectInstance;
        private bool shouldBreak;

        // Variables to showcase in cycle
        private const float timeToCleanUp = 5f;
        private const float timeToStartDestruction = 2f;
        private const float timeToReconstructObject = 2f;
        private const float cycleTime = 0.2f;
        private const float timerTimeUnit = 1f;
        
        private void Start()
        {
            TriggerBreak();
        }

        private void TriggerBreak()
        {
            // Methods For Cyclic Use (I.E Destroy On Loop -  For Showcase)
            if (isCyclic)
            {
                StartCoroutine(CycleDestruction());
            }
            // Methods For Single Use (I.E Destroy Once)
            else
            {
                StartCoroutine(DestroyOnce());
            }
        }

        // Core Methods For Single Use (I.E Destroy Once)
        private IEnumerator DestroyOnce()
        {
            objectReseted = false;
            shouldBreak = true;
            yield return null;
        }

        private void Update()
        {
            if (shouldBreak)
            {
                BreakObject();
            }
        }

        private void BreakObject()
        {
            wholeObject.gameObject.SetActive(false);
            fracturedObjectInstance = Instantiate(fracturedObject);
            fracturedObjectInstance.position = wholeObject.position;
            fracturedObjectInstance.gameObject.SetActive(true);
            isBroken = true;
            shouldBreak = false;
            StartCoroutine(CleanUpCoroutine());
        }

        private void CleanUp()
        {
            isClean = true;
            Destroy(fracturedObjectInstance.gameObject);
        }

        private IEnumerator ResetObject()
        {
            if (isClean)
            {
                yield return new WaitForSeconds(timeToReconstructObject);
                wholeObject.gameObject.SetActive(true);
                isBroken = false;
                isClean = false;
                objectReseted = true;
            }
        }

        private IEnumerator CleanUpCoroutine()
        {
            float timer = 0f;
            while (isBroken && !isClean)
            {
                if (timer >= timeToCleanUp)
                {
                    CleanUp();
                }

                yield return new WaitForSeconds(timerTimeUnit);
                timer += 1f;
            }

            // Methods For Cyclic Use (I.E Destroy On Loop -  For Showcase)
            if (isCyclic)
            {
                yield return ResetObject();
            }

            yield return null;
        }

        // Methods For Cyclic Use (I.E Destroy On Loop -  For Showcase)
        private IEnumerator CycleDestruction()
        {
            while (true)
            {
                if (objectReseted)
                {
                    yield return new WaitForSeconds(timeToStartDestruction);
                    objectReseted = false;
                    shouldBreak = true;
                }
                yield return new WaitForSeconds(cycleTime);
            }
        }
    }
}
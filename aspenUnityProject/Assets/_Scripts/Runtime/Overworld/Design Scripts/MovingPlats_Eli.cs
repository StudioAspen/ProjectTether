using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace _Scripts.Runtime.Overworld
{

    public class MovingPlats_Eli : MonoBehaviour
    {
        [SerializeField] List<Vector3> Destinations;
        int CurrentDest;
        bool ReturnPlatform = false;

        [Header("Platform Settings")]
        [SerializeField] float NormalSpeed = 0.1f;
        [SerializeField] float ReverseSpeed = 0.2f;
        [Tooltip("If true, platform will return to its starting position when reversing instead of looping through previous destinations")]
        [SerializeField] bool ReturnToStart = false;

        void FixedUpdate()
        {
            if (!ReturnPlatform)
            {
                PlatformForward();
            }
            else
            {
                PlatformReverse();
            }
        }
        void PlatformForward()
        {
            if (Destinations.Count == 0) return;
            Vector3 dest = Destinations[CurrentDest];
            Vector3 old = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, dest, NormalSpeed);
            Vector3 movement = transform.position - old;
            Vector3 lastStop = Destinations.Last();

            if (Vector3.Distance(transform.position, dest) < 0.01f)
            {
                CurrentDest++;

                if (CurrentDest >= Destinations.Count)
                {
                    CurrentDest = 0;
                }
            }
            //If plat reaches last stop in array:
            if (Vector3.Distance(transform.position, lastStop) < 0.01f)
            {
                if (!ReturnToStart)
                {
                    CurrentDest--;
                    if (CurrentDest < 0)
                        CurrentDest = Destinations.Count - 1;
                }
                
                ReturnPlatform = true;
            }
        }

        void PlatformReverse()
        {
            if (Destinations.Count == 0) return;
            Vector3 dest = Destinations[CurrentDest];
            Vector3 old = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, dest, ReverseSpeed);
            Vector3 movement = transform.position - old;

            if (Vector3.Distance(transform.position, dest) < 0.01f)
            {
                CurrentDest--;
                if (CurrentDest < 0)
                {
                    CurrentDest = 0;
                    ReturnPlatform = false;
                }
            }
        }
    }
}

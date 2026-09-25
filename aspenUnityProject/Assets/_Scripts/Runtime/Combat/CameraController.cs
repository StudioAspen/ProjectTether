using _Scripts.Runtime.Managers;
using Tether.CharacterSystems;
using TileSystem;
using UnityEngine;

namespace _Scripts.Runtime.Combat {
    public class CameraController : MonoBehaviour
    {
        private Vector3 cameraStartPosition;
        private Quaternion cameraStartRotation;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cameraStartPosition = transform.position;
            cameraStartRotation = transform.rotation;
        }

        void OnEnable()
        {
            CombatManager.examinedTile += GoToTile;
            CombatManager.exitedExamine += ReturnView;
        }

        void OnDisable()
        {
            CombatManager.examinedTile -= GoToTile;
            CombatManager.exitedExamine -= ReturnView;
        }

        // Update is called once per frame
        void Update()
        {

        }

        //maybe change rotation to match bird's-eye-view
        void GoToTile(TileController tileController)
        {
            transform.position = new Vector3(tileController.Position().x + 90, tileController.Position().y + 50,
                tileController.Position().z);
        }

        void ReturnView()
        {
            transform.position = cameraStartPosition;
            transform.rotation = cameraStartRotation;
        }
    }

}
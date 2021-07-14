using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;
using Utils;

namespace Ship
{
    public class LavaThrower : MonoBehaviour
    {
        //keep track of current lava rays
        private List<LavaGun> _guns;

        //keep track of current lara rays weight
        private float[] _gunsWeight;

        //reference to ship script to change lavaWeight Field
        private Ship _shipScript;

        private const float XScaling = 0.025f;
        private const float YScaling = 0.05f;
        private const float WScaling = 0.025f;

        [SerializeField] private GameObject lavaPrefap = null;

        // Start is called before the first frame update
        private void Start()
        {
            _guns = new List<LavaGun>();
            ReDesignThrower(GameDataUtils.CurrentLevel);

            _shipScript = gameObject.GetComponent<Ship>();
        }

        // Update is called once per frame
        private void Update()
        {
            var fire = Math.Max(Input.GetAxis("Fire Lava"), Input.GetAxis("Vertical"));
            if (fire > 0)
            {
                bool newShot = false;
                foreach (var gun in _guns)
                    if (gun.GunDown)
                    {
                        newShot = true;
                        ThrowLava(gun);
                    }
                
                if(newShot)
                    AudioManager.Play(AudioClipName.Shot);
                MakeThemLonger();
            }
            else
            {
                CutThemOut();
            }

            _shipScript.WeaponWeight = _gunsWeight.Sum();
        }

        private void ThrowLava(LavaGun lavaGun)
        {
            //Y-shift gets over written by make it longer
            var gunLavaPosition = GetLavaPosition(0.05f, lavaGun.Dir);
            _gunsWeight[GetDirIndex(lavaGun.Dir)] = 1f;

            if (lavaGun.GunDown)
            {
                lavaGun.Gun = Instantiate(lavaPrefap,
                    gunLavaPosition, Quaternion.identity);
                lavaGun.Gun.transform.localScale = new Vector3(0.5f, 0.05f, 1f);
                lavaGun.GunDown = false;
            }
        }

        private void MakeThemLonger()
        {
            var lavaScales = new Vector2[_guns.Count];
            var lavaPositions = new Vector2[_guns.Count];
            var transforms = new Transform[_guns.Count];

            for (var i = 0; i < _guns.Count; i++)
            {
                if (_guns[i].GunDown || _guns[i].Gun is null) continue;

                transforms[i] = _guns[i].Gun.transform;
                lavaScales[i] = transforms[i].localScale;

                if (lavaScales[i].y < 1)
                {
                    lavaScales[i].x += XScaling / 2;
                    lavaScales[i].y += YScaling / 2;

                    _gunsWeight[GetDirIndex(i)] += WScaling;
                }

                lavaPositions[i] = GetLavaPosition(lavaScales[i].y, _guns[i].Dir);
            }

            for (var i = 0; i < transforms.Length; i++)
            {
                if (_guns[i].GunDown || _guns[i].Gun is null) continue;

                transforms[i].localScale = lavaScales[i];
                transforms[i].position = lavaPositions[i];
            }
        }

        private void CutThemOut()
        {
            var lavaRays = new Rigidbody2D[_guns.Count];

            for (var i = 0; i < _guns.Count; i++)
            {
                if (_guns[i].GunDown || _guns[i].Gun is null) continue;
                lavaRays[i] = _guns[i].Gun.GetComponent<Rigidbody2D>();
            }

            for (var i = 0; i < lavaRays.Length; i++)
            {
                if (_guns[i].GunDown || _guns[i].Gun is null) continue;

                var gunBody = lavaRays[i];
                gunBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                gunBody.AddForce(15f * Vector2.up, ForceMode2D.Impulse);
                _gunsWeight[GetDirIndex(i)] = 1f / _gunsWeight.Length;
                _guns[i].GunDown = true;
                _guns[i].Gun = null;
            }
        }


        private int GetDirIndex(int i)
        {
            return GetDirIndex(_guns[i].Dir);
        }

        private int GetDirIndex(GunDir dir)
        {
            return (int) dir % _guns.Count;
        }

        private Vector3 GetLavaPosition(float yScale, GunDir dir)
        {
            var shipCenter = gameObject.transform.position;
            var yShift = yScale * 3.75f;

            switch (dir)
            {
                case GunDir.Middle:
                    return new Vector3(shipCenter.x, shipCenter.y + yShift + 0.525f, shipCenter.z);
                case GunDir.Level1Right:
                    return new Vector3(shipCenter.x + .35f, shipCenter.y + yShift + 0.05f, shipCenter.z);
                case GunDir.Level1Left:
                    return new Vector3(shipCenter.x - .35f, shipCenter.y + yShift + 0.05f, shipCenter.z);
                case GunDir.Level2Right:
                    return new Vector3(shipCenter.x + .50f, shipCenter.y + yShift - 0.05f, shipCenter.z);
                case GunDir.Level2Left:
                    return new Vector3(shipCenter.x - .50f, shipCenter.y + yShift - 0.05f, shipCenter.z);
                case GunDir.Level3Right:
                    return new Vector3(shipCenter.x + .25f, shipCenter.y + yShift + 0.35f, shipCenter.z);
                case GunDir.Level3Left:
                    return new Vector3(shipCenter.x - .25f, shipCenter.y + yShift + 0.35f, shipCenter.z);
                default:
                    return shipCenter;
            }
        }

        public void HitSomething(GameObject lava)
        {
            foreach (var lavaGun in _guns)
                if (lava == lavaGun.Gun)
                {
                    lavaGun.Gun = null;
                    lavaGun.GunDown = true;
                }
        }

        public void LevelUp()
        {
            GameDataUtils.CurrentLevel++;
            ReDesignThrower(GameDataUtils.CurrentLevel);
        }

        public void LevelDown()
        {
            GameDataUtils.CurrentLevel--;
            ReDesignThrower(GameDataUtils.CurrentLevel);
        }

        private void ReDesignThrower(int currentLevel)
        {
            CutThemOut();
            _guns.Clear();

            if (currentLevel % 2 == 1)
                _guns.Add(new LavaGun());

            if (currentLevel > 1)
            {
                _guns.Add(new LavaGun {Dir = GunDir.Level1Left});
                _guns.Add(new LavaGun {Dir = GunDir.Level1Right});
            }

            if (currentLevel > 3)
            {
                _guns.Add(new LavaGun {Dir = GunDir.Level2Left});
                _guns.Add(new LavaGun {Dir = GunDir.Level2Right});
            }
            
            if (currentLevel > 5)
            {
                _guns.Add(new LavaGun {Dir = GunDir.Level3Left});
                _guns.Add(new LavaGun {Dir = GunDir.Level3Right});
            }

            var gunsCount = _guns.Count;

            _gunsWeight = new float[gunsCount];
            for (var i = 0; i < gunsCount; i++)
                _gunsWeight[i] = 1f / gunsCount;
        }

        private class LavaGun
        {
            public LavaGun()
            {
                Gun = null;
                GunDown = true;
                Dir = GunDir.Middle;
            }

            public GunDir Dir { get; set; }

            public GameObject Gun { get; set; }

            public bool GunDown { get; set; }
        }

        private enum GunDir
        {
            Middle,
            Level1Left,
            Level1Right,
            Level2Left,
            Level2Right,
            Level3Left,
            Level3Right
        }
    }
}
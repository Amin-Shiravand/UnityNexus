using System.Collections;
using System.Collections.Generic;
using Nexus.Singleton;
using UnityEngine;

namespace Nexus.ResourceManager
{
    public class GameResourceManager : MonoBehaviorSingleton<GameResourceManager>
    {
        public uint CountOfAssetLoaded
        {
            get;
            private set;
        }

        public int TotalAvatarsCount
        {
            get;
            private set;
        }

        private Dictionary<int, Object> assetsHashTable = new Dictionary<int, Object>();
        // private Dictionary<int, Object[]> assetPackMap = new Dictionary<int, Object[]>();

        public Texture LoadTexture(string Path)
        {
            return GetFormAssetHashTable<Texture>("Textures/" + Path) as Texture;
        }

        public Material LoadMaterial(string Path)
        {
            return GetFormAssetHashTable<Material>("Materials/" + Path) as Material;
        }

        public GameObject LoadPrefab(string Path)
        {
            return GetFormAssetHashTable<GameObject>("Prefabs/" + Path);
        }

        public Font LoadFont(string Path)
        {
            return GetFormAssetHashTable<Font>("Fonts/" + Path) as Font;
        }

        public AudioClip LoadAudioClip(string Path)
        {
            return GetFormAssetHashTable<AudioClip>("Audios/" + Path) as AudioClip;
        }

        public Sprite LoadSprite(string Path)
        {
            return GetFormAssetHashTable<Sprite>("Arts/" + Path);
        }

        public Sprite LoadAvatarSprite(string Path)
        {
            return GetFormAssetHashTable<Sprite>("Arts/Avatars/" + Path);
        }

        public Sprite[] LoadAllAvatars()
        {
            List<Sprite> avatars = new List<Sprite>();

            for (int i = 0; i < TotalAvatarsCount; ++i)
            {
                Sprite sp = GetFormAssetHashTable<Sprite>("Arts/Avatars/" + i.ToString());
                if (sp != null)
                    avatars.Add(sp);
            }

            return avatars.ToArray();
        }

        //public Sprite[] LoadSpritePack(string Path)
        //{

        //    Object[] obj = GetPackAssetsFromMap(Path);
        //    Sprite[] sprites = ;
        //    return (Sprite[]);
        //}

        public void UnloadPrefab(string Path)
        {
            UnloadAsset("Prefabs/" + Path);
        }

        public void UnloadMaterial(string Path)
        {
            UnloadAsset("Materials/" + Path);
        }

        public void UnloadTexture(string Path)
        {
            UnloadAsset("Textures/" + Path);
        }

        public void UnloadFont(string Path)
        {
            UnloadAsset("Fonts/" + Path);
        }

        public void UnloadAudioClip(string Path)
        {
            UnloadAsset("Audios/" + Path);
        }

        public void UnloadFontSprite(string Path)
        {
            UnloadAsset("Sprites/" + Path);
        }

        public bool UnloadAllAssets()
        {

            var iterator = assetsHashTable.GetEnumerator();

            while (iterator.MoveNext())
            {
                Object current = iterator.Current.Value;

                if (current.GetType() != typeof(GameObject))
                    Resources.UnloadAsset(current);

                if (CountOfAssetLoaded > 0)
                    --CountOfAssetLoaded;
                current = null;
            }
            assetsHashTable.Clear();
            Resources.UnloadUnusedAssets();
            return CountOfAssetLoaded == 0 ? true : false;
        }

        private bool UnloadAsset(string Path)
        {
            bool isUnloaded = false;
            int hashCode = Path.GetHashCode();
            if (assetsHashTable.ContainsKey(hashCode))
            {
                Object unloadObject = assetsHashTable[hashCode];
                if (unloadObject.GetType() != typeof(GameObject))
                    Resources.UnloadAsset(unloadObject);
                assetsHashTable.Remove(hashCode);
                isUnloaded = true;
                if (CountOfAssetLoaded > 0)
                    --CountOfAssetLoaded;
            }
            return isUnloaded;
        }

        //private Object[] GetPackAssetsFromMap(string Path)
        //{
        //    int hashCode = Path.GetHashCode();
        //    if (assetPackMap.ContainsKey(hashCode))
        //        return assetPackMap[hashCode];
        //    else
        //    {
        //        Object[] objs = Resources.LoadAll(Path);
        //        if (objs == null || objs.Length == 0)
        //            return null;

        //        assetPackMap.Add(hashCode, objs);
        //        return objs;
        //    }

        //}

        private T GetFormAssetHashTable<T>(string Path) where T : Object
        {
            System.Type type = typeof(T);
            Object loadedObject = null;

            int hashCode = Path.GetHashCode();
            if (assetsHashTable.ContainsKey(hashCode))
                loadedObject = assetsHashTable[hashCode];
            else
                assetsHashTable.Add(hashCode, loadedObject);

            if (loadedObject == null)
            {
                loadedObject = Resources.Load(Path, type);


                if (loadedObject == null)
                {
                    assetsHashTable.Remove(hashCode);
                    return null;
                }
                assetsHashTable[hashCode] = loadedObject;
            }

            ++CountOfAssetLoaded;
            return (T)loadedObject;
        }

        private void Awake()
        {
            SetAvatarsCount();
        }

        private void SetAvatarsCount()
        {
            UnityEngine.Object[] avatars = Resources.LoadAll("Arts/Avatars");
            TotalAvatarsCount = avatars.Length;
            for (int i = 0; i < avatars.Length; ++i)
                Resources.UnloadAsset(avatars[i]);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
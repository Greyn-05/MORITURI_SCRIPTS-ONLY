using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceManager
{
    // Resources.UnloadUnusedAssets(); // TODO 쓸때 이점은?

    public bool Loaded = false;
    public Dictionary<string, Object> _resources = new();

    public void ResourcesAssign()
    {
        if (Loaded) return;

        ResourcesToDictionary<GameObject>();
        ResourcesToDictionary<Sprite>();
        ResourcesToDictionary<TextAsset>();
        ResourcesToDictionary<AudioClip>();
        ResourcesToDictionary<AudioSource>();
        ResourcesToDictionary<ParticleSystem>();
        ResourcesToDictionary<AudioMixer>();

        // TODO 리소스폴더에서 쓰는 자료형 전부 적으세요. 리소스폴더안에 같은 파일명있어도 1개만 저장되니(추정) 주의

        Loaded = true;

    }

    public void ResourcesToDictionary<T>() where T : Object
    {
        foreach (var obj in Resources.LoadAll<T>(""))
        {
            if (!_resources.ContainsKey(obj.name))
                _resources.Add(obj.name, obj);
        }
    }


    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    // Load 사용 예시
    /// anim.clip = Main.Resource.Load<AnimationClip>("Attack_3Combo_2_Inplace");
    ///  TextAsset csvTest = Main.Resource.Load<TextAsset>("NPCInfoCSV");


    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out var resource)) return resource as T;

        return null;

    }

    public void Unload(string key)
    {
        if (_resources.TryGetValue(key, out var resource))
        {
            _resources.Remove(key);
            Resources.UnloadAsset(resource); // 순서 이게 맞나?
        }
    }

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


    /// 게임오브젝트를 Instantiate하기 : Main.Resource.InstantiatePrefab("파일명"); , Main.Resource.InstantiatePrefab("파일명", 부모transform);
    /// 오브젝트를 풀링해서 쓰기 : Main.Resource.InstantiatePrefab("파일명", 부모transform또는null ,true);
    /// 삭제하기(공통) : Main.Resource.Destroy(this.gameObject); 


    // 풀링true하면 풀링루트 자식으로 들어가는데 어차피 풀들이 딕셔너리에 저장되어있어서 부모가 달라도 풀돌려보내기 가능할듯
    // 리소스로드용, 풀링용 리소스로드 나누고싶은데 둘다 매개변수가 같아서 오버로딩못하고 bool 매개변수를 쓸수밖에없다

    public GameObject InstantiatePrefab(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>(key);

        if (prefab == null)
        {
            // Debug.LogError($"{key}프리팹 불러오기 실패");
            return null;
        }

        if (pooling)
        {
            GameObject poo = Main.Pool.Pop(prefab);
            poo.transform.parent = parent;
            return poo;
        }

        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject InstantiatePrefab(string key, Vector3 _position, Quaternion _rotate, bool pooling = false) // 플레이어매니저용
    {
        GameObject prefab = Load<GameObject>(key);

        if (prefab == null)
        {
            // Debug.LogError($"{key}프리팹 불러오기 실패"); 
            return null;
        }

        if (pooling) return Main.Pool.Pop(prefab);

        GameObject obj = GameObject.Instantiate(prefab, _position, _rotate);
        obj.name = prefab.name;
        return obj;
    }


    public void Destroy(GameObject obj) //프리팹 풀로돌리기 풀없으면 삭제
    {
        if (obj == null) return;
        if (Main.Pool.Push(obj)) return;

        UnityEngine.Object.Destroy(obj);
    }

}
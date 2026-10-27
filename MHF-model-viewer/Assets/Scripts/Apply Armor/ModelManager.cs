using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class ModelManager : MonoBehaviour
{
	private Dictionary<string, AsyncOperationHandle<GameObject>> loadedPrefabs = new();
	private Dictionary<string, int> refCounts = new();

	private static ModelManager instance;
	public static ModelManager Instance => instance;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public async Task<GameObject> LoadModelById(string modelId, Transform parent)
	{
		if (loadedPrefabs.TryGetValue(modelId, out var handle) == false)
		{
			handle = Addressables.LoadAssetAsync<GameObject>(modelId);
			await handle.Task;

			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
				Debug.LogError("Can't load model " + modelId);
				Addressables.Release(handle);
				return null;
			}

			loadedPrefabs[modelId] = handle;
			refCounts[modelId] = 0;
		}
		refCounts[modelId]++;
		return Instantiate(handle.Result, parent);
	}

	public void ReleaseModel(string modelId)
	{
		if (refCounts.ContainsKey(modelId) == false)
			return;
		refCounts[modelId]--;
		if (refCounts[modelId] <= 0)
		{
			Addressables.Release(loadedPrefabs[modelId]);
			loadedPrefabs.Remove(modelId);
			refCounts.Remove(modelId);
		}
	}
}

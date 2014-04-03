using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	GridController grid;
	Terrain terrain;
	public GameObject pr;
	public Texture2D texture;

	// Use this for initialization
	void Start () {
		grid = GameObject.Find("grid").GetComponent<GridController>();
		terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

		float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);

		//нужно сделать приведение размеров картинки к размерам карты
		for (int y = 0; y < terrain.terrainData.heightmapResolution; y++)
		{
			for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
			{
				float height = 1.0f - texture.GetPixel(x, y).grayscale;
				heights[x, y] = 0.01f * height; //это волшебная высота, на нее надо домножать цвет картинки (выяснить, от чего зависит данный, коэф-нт! )
			}
		}
		terrain.terrainData.SetHeights(0, 0, heights);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		Debug.Log("OnClick");
		Vector3 pos;
		if (grid.MousePointToColliderHitPosition(out pos)) {	//TODO вообще-то лучше что-нибудь универсальное клик/тач			
			Vector2 cell = grid.WorldPositionToCell(pos);
			Debug.Log(cell);
			Vector3 cell_pos = grid.CellToWorldPositionOfCenter(cell);

			GameObject.Instantiate(pr, cell_pos, Quaternion.identity);
		}
	}




















	//-------------------------------------------------------------------------------------
	/// <summary>
	/// Загрузка карты высоты с файла формата RAW.
	/// </summary>
	/// <param name="file_name">Имя файла</param>
	/// <param name="use_16bit">Кодировка файла</param>
	//-------------------------------------------------------------------------------------
	/*public void LoadHeightMapRAW(String file_name, Boolean use_16bit)
	{
		if (use_16bit)
		{
			Byte[,] data = DispatcherFiles.ReadRawFile8Bit(file_name);
			
			if (data != null)
			{
				Int32 size_h = data.GetUpperBound(0);
				
				if (size_h != mTerrainData.heightmapResolution)
				{
					Debug.LogError("Ошибка в загрузке карты высот. Загружен размер <" +
					               size_h.ToString() + ">. Требуется размер <" + mTerrainData.heightmapResolution + ">");
					return;
				}
				
				Single[,] heights = mTerrainData.GetHeights(0, 0, mTerrainData.heightmapResolution, mTerrainData.heightmapResolution);
				
				for (Int32 y = 0; y < mTerrainData.heightmapResolution; y++)
				{
					for (Int32 x = 0; x < mTerrainData.heightmapResolution; x++)
					{
						heights[(mTerrainData.heightmapResolution - 1) - y, x] = (((Single)(data[x, y])) / 255.0f);
					}
				}
				
				mTerrainData.SetHeights(0, 0, heights);
			}
		}
		else
		{
			UInt16[,] data = DispatcherFiles.ReadRawFile16Bit(file_name);
			
			if (data != null)
			{
				Int32 size_h = data.GetUpperBound(0);
				
				if (size_h != mTerrainData.heightmapResolution)
				{
					Debug.LogError("Ошибка в загрузке карты высот. Загружен размер <" +
					               size_h.ToString() + ">. Требуется размер <" + mTerrainData.heightmapResolution + ">");
					return;
				}
				
				Single[,] heights = mTerrainData.GetHeights(0, 0, mTerrainData.heightmapResolution, mTerrainData.heightmapResolution);
				
				for (Int32 y = 0; y < mTerrainData.heightmapResolution; y++)
				{
					for (Int32 x = 0; x < mTerrainData.heightmapResolution; x++)
					{
						heights[(mTerrainData.heightmapResolution - 1) - y, x] = (((Single)(data[x, y])) / 65535.0f);
					}
				}
				
				mTerrainData.SetHeights(0, 0, heights);
			}
		}
	}*/
}

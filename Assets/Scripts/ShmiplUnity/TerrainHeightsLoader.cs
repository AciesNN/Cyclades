using UnityEngine;
using System.Collections;

namespace Shmipl.Unity
{
	public class TerrainHeightsLoader {
		static readonly float maxHeight = 0.01f; //это волшебная высота (todo - выяснить, от чего зависит данный, коэф-нт!)

		/*загружает карту высот из градиента серого картинки
		 * внимание, надо сделать текстуру читаемой (для чего поставить тип текстуры advanced)
		*/
		public static void LoadHeighMapFromTexture(Texture2D texture, Terrain terrain) {
			float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
			
			//хитрость с координатами сделана потому, что матрица террейна считается снизу справа, как и картинка, но считается по другим осям
			for (int y = 0; y < terrain.terrainData.heightmapResolution; ++y)
			{				
				for (int x = 0; x < terrain.terrainData.heightmapResolution; ++x)
				{

					if (y < texture.width && x < texture.height) {
						float height = 1.0f - texture.GetPixel(y, x).grayscale;
						heights[x, y] = maxHeight * height;
					} else {
						heights[x, y] = 0.0f;
					}
				}
			}
			terrain.terrainData.SetHeights(0, 0, heights);
		}

	}
}

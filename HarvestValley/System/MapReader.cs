using System.IO;


namespace HarvestValley.System
{
    public class MapReader
    {
        public const int LEVEL_ROW_SIZE = 9;
        public const int LEVEL_COLUMN_SIZE = 15;

        public string[,] LoadLevelData()
        {
            string[,] item = new string[LEVEL_COLUMN_SIZE, LEVEL_ROW_SIZE];
            string[] line = File.ReadAllLines(Path.GetFullPath("Content\\Map.txt"));
            for(int r=0; r < LEVEL_ROW_SIZE; r++)
            {
                for(int c = 0; c < LEVEL_COLUMN_SIZE; c++)
                {
                    item[c, r] = line[r].Substring(c, 1);
                }
            }
            return item;
        }
    }
}

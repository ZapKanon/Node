using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_Data
{
    [System.Serializable]
    public struct BlockData
    {
        public string path;
        public Vector2 gridPosition;
        public Node_Block.Rotations rotation;
    }

    [System.Serializable]
    public class AllBlockData
    {
        public BlockData[] m_BlockData = new BlockData[100];
    }

    public int m_Score;
    public AllBlockData m_AllBlockData = new AllBlockData();

    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(Save_Data a_SaveData);
    void LoadFromSaveData(Save_Data a_SaveData);
}

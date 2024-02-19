using System;

namespace CheckPCBSystem
{

    public class HistoryFileData
    {
        public string FileName;
    }

    public class ResultData : IDisposable
    {
        public int Index;
        public string Data;
        public Tuple<int, int> StartPos;
        public Tuple<int, int> EndPos;
        public float Level;

        public void Dispose()
        {
            this.Index = 0;
            this.Data = string.Empty;
            StartPos = default;
            EndPos = default;
            Level = 0f;
        }

        public override string ToString()
        {
            return $"ID:{Data}{Environment.NewLine}Position:{StartPos},{EndPos}{Environment.NewLine}Level:{Level}";
        }
    }
}

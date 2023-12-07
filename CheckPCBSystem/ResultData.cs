using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPCBSystem
{
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
    }
}

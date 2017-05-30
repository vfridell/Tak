using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TakLib.AI
{
    [Serializable]
    public class AnalysisMemory
    {
        public string AnalysisKey;
        public int AnalysisDepth;
        public int Score;
        public Move BestMove;
    }

    [Serializable]
    public class AnalysisMemoryCollection : Dictionary<Board, AnalysisMemory>
    {
        public static AnalysisMemoryCollection LoadMemories(string key, int depth)
        {
            AnalysisMemoryCollection returnCollection;
            string filename = $"{key}_{depth}.bin";
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists && fileInfo.Length > 0)
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream fstream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    returnCollection = (AnalysisMemoryCollection) formatter.Deserialize(fstream);
                }
            }
            else
            {
                returnCollection = new AnalysisMemoryCollection(key, depth);
            }
            return returnCollection;
        }

        public readonly string Key;
        public readonly int Depth;
        public string Filename => $"{Key}_{Depth}.bin";

        private AnalysisMemoryCollection(SerializationInfo info, StreamingContext context) : base(info,context)
        {
            Key = info.GetString("Key");
            Depth = info.GetInt32("Depth");
        }

        public AnalysisMemoryCollection(string key, int depth)
        {
            Key = key;
            Depth = depth;
        }

        public void Write()
        {
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(Filename, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, this);
            }
        }
    }
}

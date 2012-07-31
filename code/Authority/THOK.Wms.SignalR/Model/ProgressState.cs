using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace THOK.Wms.SignalR.Model
{
    public enum StateType
    {
        Start = 0,
        Processing = 1,
        Prompt = 2,
        Warning = 3,
        Error = 4,
        Info = 5,
        Question = 6,
        Confirm = 7,
        Complete = 8,
        Stop = 9
    }

    [Serializable]
    public class ProgressState
    {
        public StateType State { get; set; }
        public IList<string> Messages = new List<string>();
        public IList<string> Errors = new List<string>();
        public string TotalProgressName { get; set; }
        public int TotalProgressValue { get; set; }
        public string CurrentProgressName { get; set; }
        public int CurrentProgressValue { get; set; }
        public ProgressState Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return (ProgressState)formatter.Deserialize(stream);
        }
    }
}

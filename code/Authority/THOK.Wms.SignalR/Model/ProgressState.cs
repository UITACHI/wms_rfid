using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        Complete = 8
    }

    public class ProgressState
    {
        public StateType State { get; set; }
        public IList<string> Messages { get; set; }
        public IList<string> Errors { get; set; }
        public string TotalProgressName { get; set; }
        public int TotalProgressValue { get; set; }
        public string CurrentProgressName { get; set; }
        public int CurrentProgressValue { get; set; }
    }
}

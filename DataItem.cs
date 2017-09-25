using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH_ZhouBao {
    public class DataItem {
        public string Type { get; set; }
        public string Project { get; set; }
        public string Content { get; set; }
        public string Process { get; set; }

        public int Index { get; set; }

        public bool IsValid {
            get {
                return false;
            }
        }
    }
}

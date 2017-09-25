using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH_ZhouBao {
    class UserInfo {
        public virtual int ID { get; set; }
        public virtual string Mac { get; set; }
        public virtual String Name { get; set; }
    }

    class DefaultUserInfo : UserInfo {
        public override string Name {
            get {
                return "程序猿";
            }

            set {
                base.Name = value;
            }
        }
    }
}

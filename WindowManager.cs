using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DH_ZhouBao {
    class WindowManager {
        private static Dictionary<String, Window> windows = new Dictionary<string, Window>();
        public static Window Create(Type window, params object [] args ) {
            var key = window.FullName;
            var found = windows.ContainsKey(key);
            if(found) {
                return windows [key];
            } else {
                var dialog = Activator.CreateInstance(window,args) as Window;
                windows [key] = dialog;
                dialog.Closed += ( object sender, EventArgs e ) => {
                    windows.Remove(key);
                };
                return dialog;
            }
        }
    }
}

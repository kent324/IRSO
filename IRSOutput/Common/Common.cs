using IRSOutput.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Common {
    public class Common {
        public static string GR(Dictionary<string, string> dic, int _key) {
            string key = string.Format("Text{0}", _key);
            return dic.FirstOrDefault(p => p.Key == key).Value;
        }

        public static Dictionary<string, string> findRecordByNum(Dictionary<string, string> dic, int start, int end) {
            Dictionary<string, string> reDic = new Dictionary<string, string>();
            int tmp = 0;
            foreach (var d in dic) {
                tmp = d.Key.Substring(4, d.Key.Length - 4).ToInt();
                if (tmp >= start && tmp <= end) {
                    reDic.Add(d.Key, d.Value);
                }
            }
            return reDic;
        }
    }
}

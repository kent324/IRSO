using IRSOutput.DAL;
using IRSOutput.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput {

    /// <summary>
    /// 变种的享元模式
    /// </summary>
    public static class DBSourceFactory {
        private static Dictionary<int, List<IRSEmployee>> instance = new Dictionary<int, List<IRSEmployee>>();
        private static void Init(int year) {
            if (!instance.ContainsKey(year)) {
                DateTime dtStart = new DateTime(year + 1, 1, 1).AddDays(-1);
                DateTime dtEnd = new DateTime(year, 1, 1);

                List<IRSEmployee> tmp = IRSQueryDB.loadAllEmployees(string.Empty, dtStart, dtEnd);
                if (year == 2015) {
                    string[] queryTB = "4841".Split(',');
                    tmp = tmp.Where(x => !queryTB.Contains(x.EinfoID.ToString())).ToList();
                }
                instance.Add(year, tmp);
            }
        }
        public static List<IRSEmployee> GetDBSource(int year) {
            Init(year);
            return instance[year];
        }
    }
}
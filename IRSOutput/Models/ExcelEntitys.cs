using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Models {

    /// <summary>
    /// 从Excel数据源导入数据的模型
    /// </summary>
    public class ExcelEntitys {
        public string Name { get; set; }
        public int EinfoID { get; set; }
        public string Status { get; set; }
        public string C1 { get; set; }
        public string D1 { get; set; }
        public string E1 { get; set; }
        public string F1 { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        //public override string ToString() {
        //    return string.Format("INSERT INTO emp_Healthbilling(Einfo_id,Status,[1C],[1D],[1E],[1F],Month,Year,Name) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
        //        , EinfoID, Status, C1, D1, E1, F1, Month, Year, Name.Replace("'", ""));
        //}
    }
}

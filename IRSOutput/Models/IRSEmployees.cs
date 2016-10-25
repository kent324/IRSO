using IRSOutput.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Models {

    /// <summary>
    /// 运行当前运算的员工范围模型
    /// </summary>
    public class IRSEmployee : ExcelEntitys {
        public int DepartId { get; set; }
        public string Department { get; set; }
        public string Name_First { get; set; }
        public string Name_Middle { get; set; }
        public string Name_Last { get; set; }
        public string SSN { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime terminateDate { get; set; }
        public DateTime DOB { get; set; }
        /// <summary>
        /// false : 表示离职
        /// true : 表示在职
        /// </summary>
        public bool empStatus {
            get {
                return this.terminateDate == DateTime.MinValue;
            }
        }
        /// <summary>
        /// 工龄
        /// true: 大于一年
        /// false:小于一年
        /// </summary>
        public bool workYear => HireDate.AddYears(1).Year > ClosingDate.Year ? false : true;
        //public bool workYear {
        //    get {
        //        return HireDate.AddYears(1).Year >= ClosingDate.Year ? false : true;
        //    }
        //}
        public bool isRehire { get; set; }
        /// <summary>
        /// 当前年的截止日期
        /// </summary>
        public DateTime ClosingDate { get; set; }

        public int _month => this.HireDate.Year < int.Parse(Year) ? -1 : this.HireDate.Month;
        /*
        入职时间和离职时间不在同一年会给12,会出现问题 例子:2808
        */
        //public int terMonth => this.terminateDate.Year == int.Parse(Year) ? terminateDate.Month : 12;
        //public int terMonth => terminateDate.Month;
        public int terMonth {
            get {
                if (terminateDate.Year > int.Parse(Year)) {
                    return 12;
                }
                else if (terminateDate.Year >= HireDate.Year) {
                    return terminateDate.Month;
                }
                return 12;
            }
        }
        /// <summary>
        /// 判断是否在今年离职的
        /// </summary>
        public bool terThisYear => terminateDate.Year > Year.ToInt() ? false : true;
    }

    /// <summary>
    /// 雇佣状态
    /// </summary>
    public enum EmploymentStatus {
        OnJob,
        UnJob
    }
}
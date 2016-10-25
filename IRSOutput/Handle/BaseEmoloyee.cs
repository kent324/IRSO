using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Models {
    public class BaseEmoloyee {
        public double hireMonth { get; set; }
        public DateTime dtYear => DateTime.Parse(DateTime.Now.ToString(string.Format("{0}-01-01", Employee.Year))).AddYears(1).AddDays(-1);

        public IRSEmployee Employee;
        public List<HealthbillingKeyValue> EmployeeList = new List<HealthbillingKeyValue>();
        public BaseEmoloyee(IRSEmployee employee) {
            this.Employee = employee;
            hireMonth = dtYear.Subtract(employee.HireDate).Days / 30.5;
            EmployeeList = Context.HealthbillingList.Where(p => p.EinfoID == employee.EinfoID).ToList();
            if (EmployeeList.Count > 1 && EmployeeList.Count < 12) {
                //Healthbilling没有12笔员工的记录则自动填满,以方便后面处理.
                ReadProperty rp = new ReadProperty();
                var tmp = rp.month.Except(EmployeeList.Select(p => p.Month)).ToList();
                var eId = EmployeeList.FirstOrDefault().EinfoID;
                foreach (var e in tmp) {
                    EmployeeList.Add(new HealthbillingKeyValue() {
                        EinfoID = eId,
                        Month = e,
                        Value = 0,
                        TextField14 = string.Format("Text{0}", rp.code14[e - 1].ToString()),
                        TextField15 = string.Format("Text{0}", rp.code15[e - 1].ToString()),
                        TextField16 = string.Format("Text{0}", rp.code16[e - 1].ToString()),
                    });
                }
            }
        }

        public virtual void handler14(Context context) { }
        public virtual void handler15(Context context) { }
        public virtual void handler16(Context context) { }
        /// <summary>
        /// 15列的扩展, 如果15列里为空时以0.00来显示
        /// </summary>
        /// <param name="context"></param>
        public void handlerExtend15(Context context) {
            var inner = context.GetInner();
            var twelveMonth = inner.FirstOrDefault(p => p.Key == "Text27").Key;
            if (twelveMonth == null) {
                var except = EmployeeList.Select(s => s.TextField15).Except<string>(inner.Keys);//找出差集       
                if (except.Count() == 12) {
                    context.SetInner("Text27", "0.00");
                }
                else {
                    foreach (var ex in except) {
                        context.SetInner(ex, "0.00");
                    }
                }
            }
        }
        /// <summary>
        /// 14列的扩展,We need to also enter “1H” on line 14 for the months they are not employed (where 16 = 2A)
        /// </summary>
        /// <param name="context"></param>
        public void handlerExtend14(Context context) {
            var query = (from a in context.GetInner()
                        join b in EmployeeList.Select(s => new { s.TextField14, s.TextField16 })
                        on a.Key equals b.TextField16
                        where a.Value == "2A"
                        select b.TextField14).ToList();
            if (query.Count() > 0) {
                var inner = context.GetInner();               
                foreach (var q in query) {
                    if (!inner.ContainsKey(q)) {
                        context.SetInner(q, "1H");
                    }
                }
            }
        }
    }
}
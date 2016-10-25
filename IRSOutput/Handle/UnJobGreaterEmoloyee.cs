using IRSOutput.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.Models {
    /// <summary>
    /// 在职员工
    /// </summary>
    public class UnJobGreaterEmoloyee : BaseEmoloyee {
        public UnJobGreaterEmoloyee(IRSEmployee emp) :
            base(emp) { }
        public override void handler14(Context context) {
            var query = base.EmployeeList.Where(p => p.Month >= base.Employee._month && p.Month <= base.Employee.terMonth).ToList();

            if (query.Count() == 12) {
                context.SetInner("Text14", context.Code14);
            }
            else {
                context.SetInner(query, context.SetInnerText14);
            }

        }
        public override void handler15(Context context) {
            var query = base.EmployeeList.Where(p => p.Month >= base.Employee._month && p.Month <= base.Employee.terMonth && p.Value > 0).ToList();

            if (query.Count == 12) {
                context.SetInner("Text27", context.Code15);
            }
            else {
                context.SetInner(query, context.SetInnerText15);
            }
        }
        public override void handler16(Context context) {
            var _month = base.Employee._month;
            var employeeList = base.EmployeeList;
            var query = employeeList.Where(p => p.Month >= _month && p.Month <= base.Employee.terMonth).ToList();
            var query1 = employeeList.Where(p => p.Month > base.Employee.terMonth).ToList();            
            //var query3 = employeeList.Where(p => p.Month < _month).ToList();
            var queryValGreaterZroe = query.Where(p => p.Value > 0);

            if (queryValGreaterZroe.Count() == 12) {
                context.SetInner("Text40", Code16.C2.GetDescription());
            }
            else {
                if (base.hireMonth < 13) {
                    context.SetInner("Text41", "2D");
                }
                else if (base.hireMonth > 13 && base.hireMonth < 14) {
                    context.SetInner("Text41", "2D");
                    context.SetInner("Text42", "2D");
                }
                context.SetInner(query, context.SetInnerText16Compare);
                context.SetInner(query1, context.SetInnerText16A2);
                //context.SetInner(query3, context.SetInnerText16A2);
            }
        }
    }
}
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
    public class OnJobGreaterEmoloyee : BaseEmoloyee {
        public OnJobGreaterEmoloyee(IRSEmployee emp) :
            base(emp) { }
        public override void handler14(Context context) {
            context.SetInner("Text14", context.Code14);
        }
        public override void handler15(Context context) {
            //var employeeList = context.HealthbillingList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var emp = base.EmployeeList.Where(p => p.Value > 0);

            if (emp.Count() == 12) {
                context.SetInner("Text27", context.Code15);
            }
            else {
                foreach (var e in emp) {
                    context.SetInner(e.TextField15, context.Code15);
                }
            }
        }
        public override void handler16(Context context) {
            //var employeeList = context.HealthbillingList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var emp = base.EmployeeList.Where(p => p.Value > 0);
            if (emp.Count() == 12) {
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
                foreach (var e in emp) {
                    /*
                    H:5月
                    2A = 1-4
                    2D = 5-7
                    2C = 8-12

                    */
                    //1-4
                    

                    context.SetInner(e.TextField16, Code16.C2.GetDescription());
                }
            }
        }        
    }
}
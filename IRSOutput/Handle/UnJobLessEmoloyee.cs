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
    public class UnJobLessEmoloyee : BaseEmoloyee{
        public UnJobLessEmoloyee(IRSEmployee emp) :
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
            //var employeeList = context.HealthbillingList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var query = base.EmployeeList.Where(p => p.Month >= base.Employee._month && p.Month <= base.Employee.terMonth && p.Value > 0).ToList();
            //var query1 = employeeList.Where(p => p.Month > base.Employee.terMonth).ToList();

            if (query.Count == 12) {
                context.SetInner("Text27", context.Code15);

                //var hasValue = query.Sum(p => p.Value);
                //dic.Add("Text27", hasValue > 0 ? _amount15 : "0");
            }
            else {
                context.SetInner(query, context.SetInnerText15);
                //foreach (var q in query) {
                //    dic.Add(q.TextField, q.Value > 0 ? _amount15 : "0");
                //}
                //foreach (var q in query1) {
                //    dic.Add(q.TextField, "0");
                //}
            }
        }
        public override void handler16(Context context) {
            //var _month = base.Employee.HireDate.Year == base.Employee.terminateDate.Year ? _month : 0;
            var _month = base.Employee._month;
            //var obj = fillTextFieldBindingValue(employee.EinfoID, Year, 41, 52);
            //var employeeList = context.HealthbillingList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var employeeList = base.EmployeeList;
            var query = employeeList.Where(p => p.Month >= _month && p.Month <= base.Employee.terMonth).ToList();
            var query1 = employeeList.Where(p => p.Month > base.Employee.terMonth).ToList();
            var query2 = employeeList.Where(p => p.Month >= _month && p.Month <= _month + 3).ToList();
            var query3 = employeeList.Where(p => p.Month < _month).ToList();
            var queryValGreaterZroe = query.Where(p => p.Value > 0);

            if (queryValGreaterZroe.Count() == 12) {
                ///需修复
                //var hasValue = query.Sum(p => p.Value);
                context.SetInner("Text40", Code16.C2.GetDescription());
            }
            //dic.Add("Text40", hasValue > 0 ? "2C" : "2D");            }
            else {
                var lastValue = query.OrderByDescending(p => p.Month).Take(1).FirstOrDefault();
                if (_month != 0) {
                    foreach (var q in query2) {
                        //Employee cannot be 2D after termination month.
                        context.SetInner(q.TextField16, q.Month <= base.Employee.terMonth ? (q.Value > 0 ? "2C" : "2D") : "2A");
                    }

                    foreach (var q in query) {
                        if (lastValue != null && lastValue.Month != q.Month) {
                            context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "");
                        }
                        else if (IRSQueryDB.TerminationBorder(query, base.Employee)) {
                            context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2D");
                        }
                        /*
                        5803: TD:10/6/2015
                              October cannot be 1E and 2A at the same month. 2A should be blank for October
                        */
                        //else if (base.Employee.terThisYear) {
                        //    context.SetInner(q.TextField16, "2A");
                        //}
                    }


                    //foreach (var q in query2) {
                    //    if (lastValue!=null && lastValue.Month != q.Month) {
                    //        context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2D");
                    //        //dic.Add(q.TextField, q.Value > 0 ? "2C" : "2D");
                    //    }
                    //    else if (IRSQueryDB.TerminationBorder(query, base.Employee)) {
                    //        context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2D");
                    //        //dic.Add(q.TextField, q.Value > 0 ? "2C" : "2D");
                    //    }
                    //}
                }
                //context.SetInner(query, context.SetInnerText16Compare);
                context.SetInner(query1, context.SetInnerText16A2);
                context.SetInner(query3, context.SetInnerText16A2);
                
            }
        }
    }
}

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
    public class OnJobLessEmoloyee : BaseEmoloyee {
        public OnJobLessEmoloyee(IRSEmployee emp) :
            base(emp) { }
        public override void handler14(Context context) {
            //var employeeList = base.EmployeeList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var employeeList = base.EmployeeList;
            if (base.Employee.isRehire) {
                int reHire = 0;
                int end = 0;
                if (IRSQueryDB.getRehireEmployeeDate(base.Employee.EinfoID, base.Employee.Year.ToInt(), ref reHire, ref end)) {
                    if (reHire == end) {                        
                        context.SetInner("Text14", context.Code14);
                    }
                    else {
                        var query = employeeList.Where(p => p.Month >= reHire).ToList();
                        if (query.Count() == 12) {
                            context.SetInner("Text14", context.Code14);
                        }
                        else {
                            foreach (var e in query) {
                                context.SetInner(e.TextField14, context.Code14);
                            }
                        }
                        //context.SetInner(query, context.SetInnerText14);
                        //var query = employeeList.Where(p => p.Month <= end).ToList();
                        //if (reHire > 0) {
                        //    var query2 = base.EmployeeList.Where(p => p.Month >= reHire).ToList();                            
                        //    context.SetInner(query2, context.SetInnerText14);
                        //}                        
                        //context.SetInner(query, context.SetInnerText14);
                    }
                }
            }
            else {
                var query = employeeList.Where(p => p.Month < base.Employee._month).ToList();
                var query1 = employeeList.Where(p => p.Month >= base.Employee._month).ToList();                
                context.SetInner(query1, context.SetInnerText14);
            }

        }
        public override void handler15(Context context) {
            //var employeeList = base.EmployeeList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var employeeList = base.EmployeeList;
            if (base.Employee.isRehire) {
                int reHire = 0;
                int end = 0;
                if (IRSQueryDB.getRehireEmployeeDate(base.Employee.EinfoID, base.Employee.Year.ToInt(), ref reHire, ref end)) {
                    var query = employeeList.Where(p => p.Month <= end).ToList();
                    if (reHire > 0) {
                        var query1 = employeeList.Where(p => p.Month >= reHire && p.Month < reHire + 3).ToList();
                        var query2 = employeeList.Where(p => p.Month >= reHire - 3 && p.Month < reHire).ToList();

                        var query3 = employeeList.Where(p => p.Month >= reHire + 3).ToList();

                        context.SetInner(query1, context.SetInnerText15Compare);
                        context.SetInner(query3, context.SetInnerText15Compare);
                        //foreach (var q in query1) {
                        //    dic.Add(q.TextField, q.Value > 0 ? _amount15 : "0");
                        //    context.SetInner(q.)
                        //}
                        //foreach (var q in query2) {
                        //    var exitst = query.FirstOrDefault(p => p.Month == q.Month);
                        //    if (exitst == null)
                        //        dic.Add(q.TextField, "0");
                        //}
                        //foreach (var q in query3) {
                        //    dic.Add(q.TextField, q.Value > 0 ? _amount15 : "0");
                        //}
                    }
                    context.SetInner(query, context.SetInnerText15Compare);
                    
                    //foreach (var q in query) {
                    //    var exitst = dic.FirstOrDefault(p => p.Key == q.TextField);
                    //    if (exitst.Key == null)
                    //        dic.Add(q.TextField, q.Value > 0 ? _amount15 : "");
                    //}
                }
            }
            else {
                var query = employeeList.Where(p => p.Month < base.Employee._month).ToList();
                var query1 = employeeList.Where(p => p.Month >= base.Employee._month && p.Month < base.Employee._month + 3).ToList();
                var query2 = employeeList.Where(p => p.Month >= base.Employee._month + 3).ToList();

                //Dictionary<string, string> beforeDic = new Dictionary<string, string>();
                //dic.ToList().ForEach(p => beforeDic.Add(p.Key, p.Value));
                var query3 = employeeList.Where(p => p.Month >= base.Employee._month + 3).ToList();

                context.SetInner(query3, context.SetInnerText15Compare);

                //C1095ContentByAlgorithm(employee.EinfoID, Year, "", _amount15, 15, _month + 2, "0", ref dic);

                //if (query.Union(query1).ToList().Count == 12 && beforeDic.Count == dic.Count) {
                //    dic.Add("Text27", "0");
                //}
                //else {
                //    foreach (var q in query) {
                //        dic.Add(q.TextField, "0");
                //    }

                //    foreach (var q in query1) {
                //        dic.Add(q.TextField, "0");
                //    }
                //}
            }
        }
        public override void handler16(Context context) {
            //var employeeList = base.EmployeeList.Where(p => p.EinfoID == base.Employee.EinfoID);
            var employeeList = base.EmployeeList;
            if (base.Employee.isRehire) {
                int reHire = 0;
                int end = 0;
                if (IRSQueryDB.getRehireEmployeeDate(base.Employee.EinfoID, base.Employee.Year.ToInt(), ref reHire, ref end)) {
                    var query = employeeList.Where(p => p.Month <= end).ToList();
                    if (reHire > 0) {
                        //var query1 = month.Where(p => p.Key >= reHire && p.Key < _month + 3).ToList();
                        var query1 = employeeList.Where(p => p.Month >= reHire && p.Month <= reHire + 3).ToList();
                        var query2 = employeeList.Where(p => p.Month > reHire + 3).ToList();
                        //var query2 = obj.Where(p => p.Month >= reHire + 3 && p.Value > 0).ToList();

                        var query3 = employeeList.Except(query).Except(query1).Except(query2);
                        var query4= employeeList.Where(p => p.Month < base.Employee._month).ToList();
                        //var query4 = obj.Where(p => p.Month == reHire + 3).ToList();

                        foreach (var q in query1) {
                            //dic.Add(q.TextField, q.Value > 0 ? "2C" : "2D");
                            context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2D");
                        }
                        foreach (var q in query2) {
                            //dic.Add(q.TextField, "2C");
                            context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "");
                        }
                        foreach (var q in query4) {
                            context.SetInner(q.TextField16, "2A");
                            //dic.Add(q.TextField, "2A");
                        }
                        //foreach (var q in query4) {
                        //    var exitst = dic.FirstOrDefault(p => p.Key == q.TextField).Key;
                        //    if (exitst == null && q.Month == _month + 3)
                        //        dic.Add(q.TextField, q.Value > 0 ? "2C" : "2D");
                        //}
                        foreach (var q in query3) {
                            var exitst = context.GetInner().FirstOrDefault(p => p.Key == q.TextField16).Key;
                            
                            if (exitst == null && q.Month < reHire + 3)
                                context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2A");
                            
                            //dic.Add(q.TextField, "2A");
                        }
                    }
                    foreach (var q in query) {
                        var exitst = context.GetInner().FirstOrDefault(p => p.Key == q.TextField16).Key;

                        if (exitst == null)
                            context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "");
                        //dic.Add(q.TextField, q.Value > 0 ? "2C" : "");
                    }
                }
            }
            else {

                var query = employeeList.Where(p => p.Month < base.Employee._month).ToList();
                var query1 = employeeList.Where(p => p.Month >= base.Employee._month && p.Month < base.Employee. _month + 3).ToList();
                var query2 = employeeList.Where(p => p.Month > base.Employee._month + 2).ToList();

                //var query = month.Where(p => p.Key < _month).ToList();
                //var query1 = month.Where(p => p.Key >= _month && p.Key < _month + 3).ToList();
                //C1095ContentByAlgorithm(employee.EinfoID, Year, "", "2C", 16, _month + 2, string.Empty, ref dic);

                foreach (var q in query) {
                    context.SetInner(q.TextField16, "2A");
                    //dic.Add(q.TextField, "2A");
                }

                foreach (var q in query1) {
                    context.SetInner(q.TextField16, "2D");
                    //dic.Add(q.TextField, "2D");
                }

                foreach (var q in query2) {
                    /*For some New employees:  The 4th month of Line 16 is currently blank depending on what day of the month they are hired.*/
                    if (q.Month == base.Employee._month + 3) {
                        //dic.Add(q.TextField, q.Value > 0 ? "2C" : "2D");
                        context.SetInner(q.TextField16, q.Value > 0 ? "2C" : "2D");
                    }
                    else {
                        //dic.Add(q.TextField, q.Value > 0 ? "2C" : string.Empty);
                        context.SetInner(q.TextField16, q.Value > 0 ? "2C" : string.Empty);
                    }
                }
            }
        }
    }
}
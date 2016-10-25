using IRSOutput.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IRSOutput.DAL {
   
    /// <summary>
    ///ExtendedMethod 的摘要说明
    /// </summary>
    public static class ExtendedMethod {
        /// <summary>
        /// String 转换为Int
        /// </summary>
        /// <param name="s">前提是String已知是Int</param>
        /// <returns></returns>
        public static int ToInt(this String s) {
            int val;
            int.TryParse(s, out val);
            return val;
        }
        /// <summary>
        /// Object 转换为Int
        /// </summary>
        /// <param name="o">前提是Object已知是Int</param>
        /// <returns></returns>
        public static int ToInt(this Object o) {
            int val;
            int.TryParse(o.ToString(), out val);
            return val;
        }

        /// <summary>
        /// Object 转换为Boolean
        /// </summary>
        /// <param name="o">前提是Object已知是Boolean</param>
        /// <returns></returns>
        public static bool ToBoolean(this Object o) {
            return Convert.ToBoolean(o);
        }
        /// <summary>
        /// Object 转换为Int
        /// </summary>
        /// <param name="o">前提是Object已知是Int</param>
        /// <returns></returns>
        public static float ToFloat(this Object o) {
            float val;
            float.TryParse(o.ToString(), out val);
            return val;
        }
        /// <summary>
        /// Linq支持DistinctBy多个字段
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source) {
                if (seenKeys.Add(keySelector(element))) {
                    yield return element;
                }
            }
        }

        public static string GetDescription<TEnum>(this TEnum enumerationValue)
   where TEnum : struct, IComparable, IFormattable, IConvertible {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum) {
                throw new ArgumentException("EnumerationValue必须是一个枚举值", "enumerationValue");
            }

            //使用反射获取该枚举的成员信息
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0) {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0) {
                    //返回枚举值得描述信息
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //如果没有描述特性的值，返回该枚举值得字符串形式
            return enumerationValue.ToString();
        }
    }
}

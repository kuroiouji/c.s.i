using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class CommonUtil
    {
        public static bool IsNullOrEmpty(object obj)
        {
            if (obj != null)
            {
                if (obj is string)
                {
                    string str = (string)obj;
                    if (string.IsNullOrEmpty(str.Trim()) == false
                        || string.IsNullOrWhiteSpace(str.Trim()) == false)
                        return false;
                }
                else if (obj.GetType().IsGenericType)
                {
                    System.Collections.ICollection objL = obj as System.Collections.ICollection;
                    if (objL != null)
                    {
                        if (objL.Count > 0)
                            return false;
                    }
                }
                else if (obj is DBNull)
                {
                    return true;
                }
                else
                    return false;
            }

            return true;
        }
    }
}

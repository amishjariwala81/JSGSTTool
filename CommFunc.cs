using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//Test 1
//Test 2

//AJ Test

// Test New Entry   
// Test 3
// Test 4
namespace EWayBillTool
{
    internal class CommFunc
    {
        public const int VAR_INTEGER = 3;
        public const int VAR_STRING = 8;
        public const int VAR_DATE = 7;
        public const int VAR_UNBDEFINED = 9;

        public enum VariantType
        {            
            Empty = 0,         
            Null = 1,            
            Short = 2,            
            Integer = 3,            
            Single = 4,            
            Double = 5,            
            Currency = 6,            
            Date = 7,            
            String = 8,            
            Object = 9,           
            Error = 10,            
            Boolean = 11,            
            Variant = 12,            
            DataObject = 13,            
            Decimal = 14,          
            Byte = 17,            
            Char = 18,            
            Long = 20,
            UserDefinedType = 36,            
            Array = 8192
        }

        public static VariantType VarType(object VarName)
        {
            if (VarName == null)
            {
                return VariantType.Object;
            }

            return VarTypeFromComType(VarName.GetType());
        }

        internal static VariantType VarTypeFromComType(Type typ)
        {
            if ((object)typ == null)
            {
                return VariantType.Object;
            }

            if (typ.IsArray)
            {
                typ = typ.GetElementType();
                if (typ.IsArray)
                {
                    return (VariantType)8201;
                }

                VariantType variantType = VarTypeFromComType(typ);
                if ((variantType & VariantType.Array) != 0)
                {
                    return (VariantType)8201;
                }

                return variantType | VariantType.Array;
            }

            if (typ.IsEnum)
            {
                typ = Enum.GetUnderlyingType(typ);
            }

            if ((object)typ == null)
            {
                return VariantType.Empty;
            }

            switch (Type.GetTypeCode(typ))
            {
                case TypeCode.String:
                    return VariantType.String;
                case TypeCode.Int32:
                    return VariantType.Integer;
                case TypeCode.Int16:
                    return VariantType.Short;
                case TypeCode.Int64:
                    return VariantType.Long;
                case TypeCode.Single:
                    return VariantType.Single;
                case TypeCode.Double:
                    return VariantType.Double;
                case TypeCode.DateTime:
                    return VariantType.Date;
                case TypeCode.Boolean:
                    return VariantType.Boolean;
                case TypeCode.Decimal:
                    return VariantType.Decimal;
                case TypeCode.Byte:
                    return VariantType.Byte;
                case TypeCode.Char:
                    return VariantType.Char;
                case TypeCode.DBNull:
                    return VariantType.Null;
                default:
                    if ((object)typ == typeof(Missing) || (object)typ == typeof(Exception) || typ.IsSubclassOf(typeof(Exception)))
                    {
                        return VariantType.Error;
                    }

                    if (typ.IsValueType)
                    {
                        return VariantType.UserDefinedType;
                    }
                    return VariantType.Object;
            }
        }

        public string V2C(object cValue)
        {
            object xValue;
            string cReturn = "";
            xValue = cValue;
            if (VarType(cValue) == VariantType.Integer || VarType(cValue) == VariantType.Double || VarType(cValue) == VariantType.Decimal || VarType(cValue) == VariantType.Single || VarType(cValue) == VariantType.Long)
            {
                xValue = string.IsNullOrWhiteSpace(xValue?.ToString().Trim()) ? 0 : xValue;
                cReturn = xValue.ToString().Trim();
            }
            else if (VarType(cValue) == VariantType.Decimal)
            {
                xValue = string.IsNullOrEmpty(xValue?.ToString().Trim()) ? 0 : xValue;
                cReturn = xValue.ToString().Trim();
            }
            else if (VarType(cValue) == VariantType.String)
            {
                xValue = xValue.ToString().Replace("'", "''");                
                cReturn = "'" + (string.IsNullOrWhiteSpace(xValue.ToString()) ? "" : xValue.ToString().Trim()) + "'";
            }
            else if (VarType(cValue) == VariantType.Date)
            {
                cReturn = "'" + DateTime.Now.ToString(xValue.ToString().Trim()) + "'";
            }
            else
            {
                cReturn = "''";
            }
            return cReturn;
        }
        public string[] VToken(string str, long numOfChar)
        {
            string[] sArr;
            long nCount=0;
            sArr = new string[(int)(str.Length / numOfChar)];
            do
            {
                sArr[nCount] = str.Substring(0, (int)numOfChar);
                str = str.Substring((int)numOfChar);
                nCount++;
            }while (str.Length > 0);
            return sArr;
        }
    }
}
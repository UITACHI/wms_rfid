#region Header
//
//MAIN MODULE   : Sabre.Common
//MODULE        :         
//SUB MODULE    :    
//AUTHOR        : AjanthaJa      
//CREATED       : 10/2/2008 8:01:56 PM        
//DESCRIPTION   : Common funtions and procedures 
//MODIFICATION HISTORY:  
//COPYRIGHT : Copyright Sabre Technologies (Pvt) Ltd. All Rights Reserved.
//
#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#endregion

namespace THOK.Common
{
    public class NullHandler
    {
        #region Private Fields

        #endregion

        #region Contructor
        public NullHandler()
        {
        }
        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        #region LeadingZero

        /// <summary>
        /// Get Leading Zero(s) for given number of digit
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="noOfDigits">No Of Digits</param>
        /// <returns>String with appropriate leading zeros</returns>
        public static string LeadingZero(int number, int noOfDigits)
        {
            double tempNo;
            string tempString;

            tempNo = number / Math.Pow(10, (double)noOfDigits);

            tempString = tempNo.ToString();
            if (tempString == "0.1") tempString = "0.10";
            return tempString.Substring(tempString.IndexOf('.') + 1, noOfDigits);
        }

             #endregion

        #region DateTime SetMinimumdate()
        public static DateTime SetMinimumdate()
        {
            return new DateTime(1900, 1, 1, 0, 0, 0);
        }
        #endregion

        #region object ValueOF(string Value)
        public static object ValueOF(string Value)
        {
            if (Value == "" || Value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return Value.TrimEnd();
            }
        }
        #endregion

        #region object ValueOF(bool Value)
        public static object ValueOF(bool Value)
        {
            if (Value.Equals(null))
            {
                return false;
            }
            else
            {
                return Value;
            }
        }
        #endregion

        #region object ValueOF(int Value)
        public static object ValueOF(int Value)
        {
            if (Value.Equals(null))
            {
                return DBNull.Value;
            }
            else
            {
                return Value;
            }
        }
        #endregion

        #region object ValueOF(int? Value)
        public static object ValueOF(int? Value)
        {
            if (Value.Equals(null))
            {
                return DBNull.Value;
            }
            else
            {
                return Value;
            }
        }
        #endregion
        
        #region object ValueOF(double Value)
        public static object ValueOF(double Value)
        {
            if (Value.Equals(null))
            {
                return DBNull.Value;
            }
            else
            {
                return Value;
            }
        }
        #endregion

        #region object ValueOF(DateTime? Value)
        public static object ValueOF(DateTime? Value)
        {
            if (Value.Equals(null))
            {
                return DBNull.Value;
            }
            else
            {
                return Value;
            }
        }
        #endregion

        #region object AvoidNull(object Value)
        public static object AvoidNull(object Value)
        {
            if (Value == null || Value == DBNull.Value) return null;
            else return Value;
        }
        #endregion
        
        #region string AvoidNullStr(object Value)
        /// <summary>
        /// Convert and trim string databse object
        /// </summary>
        /// <param name="Value"></param>
        /// <returns>string value</returns>
        public static string AvoidNullStr(object Value)
        {
            if ((Value == null) || (Value == DBNull.Value)) return String.Empty;
            else return Convert.ToString(Value).Trim();
        }
        #endregion

        #region char AvoidNullChar(object Value)
        /// <summary>
        /// Convert and char database object
        /// </summary>
        /// <param name="Value"></param>
        /// <returns>char value</returns>
        public static char AvoidNullChar(object Value)
        {
            if ((Value == null) || (Value == DBNull.Value)) return Char.MinValue;
            else return ((char)Value);
        }
        #endregion

        #region object AvoidNullByteArray(object Value)
        public static object AvoidNullByteArray(object Value)
        {
            if (Value == null || Value == DBNull.Value) return new byte[0];
            else return Value;
        }
        #endregion

        #region bool AvoidNullBool(object Value)
        public static bool AvoidNullBool(object Value)
        {
            if (Value == null || Value == DBNull.Value) return false;
            else return (bool)Value;
        }
        #endregion

        #region int AvoidNullint(object Value)
        public static int AvoidNullint(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return int.Parse(Value.ToString());
        }
        #endregion

        public static int? AvoidNullIntNullable(object Value)
        {
            if (Value == null || Value == DBNull.Value) return null;
            else return ConvertToIntNullable(Value.ToString());
        }

        public static byte AvoidNullByte(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return ConvertToByte(Value.ToString());
        }

        public static byte? AvoidNullByteNullable(object Value)
        {
            if (Value == null || Value == DBNull.Value) return null;
            else return ConvertToByteNullable(Value.ToString());
        }

        public static short AvoidNullShort(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return ConvertToByte(Value.ToString());
        }

        public static short? AvoidNullShortNullable(object Value)
        {
            if (Value == null || Value == DBNull.Value) return null;
            else return ConvertToShortNullable(Value.ToString());
        }

        #region static decimal AvoidNullDecimal(object Value)
        public static decimal AvoidNullDecimal(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return decimal.Parse(Value.ToString());
        }
        #endregion

        #region Single AvoidNullSingle(object Value)
        public static Single AvoidNullSingle(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return Single.Parse(Value.ToString());
        }
        #endregion

        #region Single AvoidNullSingleCurrency(object Value)
        public static Single AvoidNullSingleCurrency(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else
            {
                Single s = Single.Parse(Value.ToString());
                return Single.Parse(s.ToString("#,###.00"));

            }
        }
        #endregion

        #region double AvoidNulldouble(object Value)
        public static double AvoidNulldouble(object Value)
        {
            if (Value == null || Value == DBNull.Value) return 0;
            else return double.Parse(Value.ToString());
        }
        #endregion

        #region DateTime AvoidNullDateTime(object objValue)
        public static DateTime AvoidNullDateTime(object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
            {
                DateTime dateTime = new DateTime();
                dateTime = DateTime.MinValue;
                return dateTime;

            }
            else return Convert.ToDateTime(objValue);
        }
        #endregion

        #region DateTime? AvoidNullDateTime(object objValue)
        public static DateTime? AvoidNullDateTimeNullable(object objValue)
        {
            if (objValue == null || objValue == DBNull.Value)
            {
                return null;
            }
            else return Convert.ToDateTime(objValue);
        }
        #endregion

        #region string TextOf(bool Value)
        public static string TextOf(bool Value)
        {
            if (Value)
                return "Yes";
            else
                return "No";
        }
        #endregion

        #region object GetDBNull(DateTime Value)
        public static object GetDBNull(DateTime Value)
        {
            if (Value == SetMinimumdate()) return DBNull.Value;
            else return Value;

        }

        #endregion

        #region object GetDBNull(DateTime? Value)
        public static object GetDBNull(DateTime? Value)
        {
            if (Value == null) return DBNull.Value;
            else return Value;

        }

        #endregion

        #region int ConvertToInt(string value)
        public static int ConvertToInt(string value)
        {
            int i = 0;
            int.TryParse(value, out i);
            return i;
        }
        #endregion

        #region int? ConvertToIntNullable(string value)
        public static int? ConvertToIntNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return ConvertToInt(value);
            }
        }

        public static int? ConvertToIntNullable(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == "")
            {
                return null;
            }
            else
            {
                return ConvertToInt(value.ToString());
            }
        }
        #endregion
        
        #region int? ConvertToIntNullableZeroNull(string value)
        public static int? ConvertToIntNullableZeroNull(string value)
        {
            if (value == null || value == "" || value == "0")
            {
                return null;
            }
            else
            {
                return ConvertToInt(value);
            }
        }
        #endregion

        #region DateTime? ConvertToDateTimeNullable(string value)
        public static DateTime? ConvertToDateTimeNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(value);
            }
        }
        public static DateTime? ConvertToDateTimeNullable(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(value);
            }
        }
        #endregion

        #region Double? ConvertToDoubleNullable(string value)
        public static Double? ConvertToDoubleNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDouble(value);
            }
        }
        #endregion

        #region Double? ConvertToDoubleNullable(object value)
        public static Double? ConvertToDoubleNullable(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDouble(value.ToString());
            }
        }
        #endregion

        #region Decimal? ConvertToDecimalNullable(string value)
        public static Decimal? ConvertToDecimalNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }

        public static Decimal? ConvertToDecimalNullable(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }
        #endregion

        public static byte ConvertToByte(string value)
        {
            byte i = 0;
            byte.TryParse(value, out i);
            return i;
        }

        #region byte? ConvertToByteNullable(string value)
        public static byte? ConvertToByteNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return ConvertToByte(value);
            }
        }
        #endregion

        #region byte? ConvertToByteNullableZeroNull(string value)
        public static byte? ConvertToByteNullableZeroNull(string value)
        {
            if (value == null || value == "" || value == "0")
            {
                return null;
            }
            else
            {
                return ConvertToByte(value);
            }
        }
        #endregion
        
        public static short ConvertToShort(string value)
        {
            short i = 0;
            short.TryParse(value, out i);
            return i;
        }

        #region short? ConvertToShortNullable(string value)
        public static short? ConvertToShortNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else
            {
                return ConvertToShort(value);
            }
        }
        #endregion

        #region short? ConvertToShortNullable(object value)
        public static short? ConvertToShortNullable(object value)
        {
            if (value == DBNull.Value || value == null || value.ToString() == "")
            {
                return null;
            }
            else
            {
                return ConvertToShort(value.ToString());
            }
        }
        #endregion

        #region short? ConvertToShortNullableZeroNull(string value)
        public static short? ConvertToShortNullableZeroNull(string value)
        {
            if (value == null || value == "" || value == "0")
            {
                return null;
            }
            else
            {
                return ConvertToShort(value);
            }
        }
        #endregion
        
        #region DateTime? AvoidNullDateTimeNullable(string value)
        public static DateTime? AvoidNullDateTimeNullable(string value)
        {
            if (value == null || value == "")
            {
                return null;
            }
            else return DateTime.Parse(value);
        }
        #endregion

        #region string GetShortDateStr(DateTime? value)
        public static string GetShortDateStr(DateTime? value)
        {
            if (value == null)
            {
                return "";
            }
            else return DateTime.Parse(value.ToString()).ToShortDateString();
        }
        #endregion
        
        public static string NullHandlerForString(object obj, string alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToString(obj);
            }
            catch (Exception)
            {
               
                return alternate;
            }
        }

        public static int NullHandlerForInt(object obj, int alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
               
                return alternate;
            }
        }

        public static short NullHandlerForShort(object obj, short alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToInt16(obj);
            }
            catch (Exception)
            {
                
                return alternate;
            }
        }

        public static decimal NullHandlerForDecimal(object obj, decimal alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToDecimal(obj);
            }
            catch (Exception)
            {
                
                return alternate;
            }
        }

        public static double NullHandlerForDouble(object obj, double alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToDouble(obj);
            }
            catch (Exception)
            {

                return alternate;
            }
        }

        public static DateTime NullHandlerForDate(object obj, DateTime alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToDateTime(obj);
            }
            catch (Exception)
            {
                
                return alternate;
            }
        }

        public static bool NullHandlerForBoolean(object obj, bool alternate)
        {
            try
            {
                if (obj == null || obj == DBNull.Value)
                    return alternate;
                else
                    return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                
                return alternate;
            }
        }


    #endregion

        #region Private Methods

        #endregion
    }
}

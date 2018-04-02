using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace Kangaroochinhhang.Models
{
    public class StringClass
    {
        /// <summary>
        /// Ma hoa chuoi ky tu (MD5)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 
       public static KangaroochinhhangContext db = new KangaroochinhhangContext();

        public static void DropDownListNews(int cateid, List<SelectListItem> carlist, string strReturn)
        {
            var cars = db.tblGroupNews.Where(p => p.ParentID == cateid).ToList();
            foreach (var item in cars)
            {
                carlist.Add(new SelectListItem { Text = strReturn + " " + item.Name, Value = item.id.ToString() });
                strReturn = strReturn + "---";
                DropDownListNews(item.id, carlist, strReturn);
                strReturn = strReturn.Substring(0, strReturn.Length - 3);

            }
        }
        public static string StripHtml(string source)
        {
            string output;

            //get rid of HTML tags
            output = Regex.Replace(source, "<[^>]*>", string.Empty);

            //get rid of multiple blank lines
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

            return output;
        }
        public static void DropDownListFor(int cateid, List<SelectListItem> carlist, string strReturn)
        {
            var cars = db.tblGroupProducts.Where(p => p.ParentID == cateid).ToList(); 
            foreach (var item in cars)
            {
                carlist.Add(new SelectListItem { Text = strReturn + " " + item.Name, Value = item.id.ToString() });
                strReturn = strReturn + "---";
                DropDownListFor(item.id, carlist, strReturn);
                strReturn = strReturn.Substring(0, strReturn.Length - 3);

            }
        }
        public static double Round(double value, int digits)
        {
            if (digits >= 0) return Math.Round(value, digits);
            double n = Math.Pow(10, -digits);
            return Math.Round(value / n, 0) * n;
        }
        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            byte[] valueArray = Encoding.ASCII.GetBytes(value);
            valueArray = md5.ComputeHash(valueArray);
            var sb = new StringBuilder();
            for (int i = 0; i < valueArray.Length; i++)
                sb.Append(valueArray[i].ToString("x2").ToLower());
            return sb.ToString();
        }
        /// <summary>
        /// Tao chuoi dung cho rewrite url
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        #region Name To Tag
        public static string NameToTag(string strName)
        {
            string text = strName;
            for (int i = 32; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), " ");

            }
            text = text.Replace(".", " ");
            text = text.Replace(" ", " ");
            text = text.Replace(",", " ");
            text = text.Replace(";", " ");
            text = text.Replace(":", " ");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            string strTitle = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        #region Generate SEO Friendly URL based on Title
        //Trim Start and End Spaces.
        strTitle = strTitle.Trim();
        //Trim "-" Hyphen
        strTitle = strTitle.Trim('-');
        strTitle = strTitle.ToLower();
        char[] chars = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
        strTitle = strTitle.Replace("c#", "C-Sharp");
        strTitle = strTitle.Replace("vb.net", "VB-Net");
        strTitle = strTitle.Replace("asp.net", "Asp-Net");
        //Replace . with - hyphen
        strTitle = strTitle.Replace(".", "-");
        //Replace Special-Characters
        for (int i = 0; i < chars.Length; i++)
        {
            string strChar = chars.GetValue(i).ToString();
            if (strTitle.Contains(strChar))
            {
                strTitle = strTitle.Replace(strChar, string.Empty);
            }
        }
        //Replace all spaces with one "-" hyphen
        strTitle = strTitle.Replace(" ", "-");
        //Replace multiple "-" hyphen with single "-" hyphen.
        strTitle = strTitle.Replace("--", "-");
        strTitle = strTitle.Replace("---", "-");
        strTitle = strTitle.Replace("----", "-");
        strTitle = strTitle.Replace("-----", "-");
        strTitle = strTitle.Replace("----", "-");
        strTitle = strTitle.Replace("---", "-");
        strTitle = strTitle.Replace("--", "-");
        //Run the code again...
        //Trim Start and End Spaces.
        strTitle = strTitle.Trim();

        //Trim "-" Hyphen
        strTitle = strTitle.Trim('-');
        #endregion

        return strTitle;
           
        }
        #endregion
        public static string ConvertToUnSign(string text)
        {

            for (int i = 32; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), " ");

            }

            text = text.Replace(".", " ");

            text = text.Replace(" ", " ");

            text = text.Replace(",", " ");

            text = text.Replace(";", " ");

            text = text.Replace(":", " ");



            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");



            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

        }
        public static string COnvertToUnSign1(string text)
        {
            for (int i = 32; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), " ");

            }

        
            text = text.Replace("-", " ");

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = text.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        /// <summary>
        /// Xoa ky tu unicode tu chuoi
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        #region Remove Unicode
        public static string RemoveUnicode(string strString)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string strFormD = strString.Normalize(NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }
        #endregion
        #region[ShowNameLevel]
        public static string ShowNameLevel(int level)
        {

            string strReturn = "";
            for (int i = 0; i < level; i++)
                strReturn = strReturn + "----";
            return strReturn;
        }
        #endregion
        /// <summary>
        /// Tao mot chuoi ngau nghien
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        #region Random String
        public static string RandomString(int size)
        {
            Random rnd = new Random();
            string srds = "";
            string[] str = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            for (int i = 0; i < size; i++)
            {
                srds = srds + str[rnd.Next(0, 61)];
            }
            return srds;
        }
        #endregion
        public static string ShowPageType(string ActiveCode)
        {
            return ActiveCode == "1" ? "Trang liên kết" : "Trang nội dung";
        }
        public static string ShowSupportType(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "0,yahoo", "1,skype","2,hotline" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #region[Xem kieu thanh vien]
        public static string ShowTypeMember(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "0,Account thường", "1,Account Vip" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region[Xem hinh thuc thanh toan]
        public static string ShowTypePay(string type)
        {
            string strString = "";
            string[] myArr = new string[] { "1,Thanh toán tại nhà", "2,Thanh toán tại cửa hàng" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(type))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region[Xem kieu tinh trang don hang]
        public static string ShowStateBill(string state)
        {
            string strString = "";
            string[] myArr = new string[] { "1,Mới đặt hàng", "2,Đã nhận tiền", "3,Đã gửi hàng", "4,Đã hủy đơn hàng" };
            char[] splitter = { ',', ';' };
            for (int i = 0; i < myArr.Length; i++)
            {
                string[] arr = myArr[i].Split(splitter);
                if (arr[0].Equals(state))
                {
                    strString = arr[1];
                    break;
                }
            }
            return strString;
        }
        #endregion
        #region [Format_Price]
        public static string Format_Price(string Price)
        {
            Price = Price.Replace(".", "");
            Price = Price.Replace(",", "");
            string tmp = "";
            while (Price.Length > 3)
            {
                tmp = "." + Price.Substring(Price.Length - 3) + tmp;
                Price = Price.Substring(0, Price.Length - 3);
            }
            tmp = Price + tmp;
            return tmp;
        }
        #endregion
        #region [NumberStr]
        public static string NumberStr(string str)
        {
            return str.Replace(".", "");
        }
        #endregion


         
    }
}
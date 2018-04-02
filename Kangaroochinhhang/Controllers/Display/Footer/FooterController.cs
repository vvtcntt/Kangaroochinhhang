using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
namespace Kangaroochinhhang.Controllers.Display.Footer
{
    
    public class FooterController : Controller
    {
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        //
        // GET: /Footer/
        public ActionResult Index()
        {
            return View();
        }
        //[OutputCache(Duration = 2400)]

        public PartialViewResult FooterPartial()
        { var TblConfig = db.TblConfigs.First();
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < listSupport.Count;i++ )
            {
                chuoi += " <div class=\"Tear_Support\">";
                chuoi += "<div class=\"Image\" style=\" background:url("+listSupport[i].Images+") no-repeat;\"></div>";
                chuoi += "<span class=\"Name\">"+listSupport[i].Name+"</span>";
                chuoi += "<span class=\"Fun\">"+listSupport[i].Mission+"</span>"; 
                chuoi += "<a href=\"Skype:"+listSupport[i].Skyper+"?chat\"><img class=\"imgSkype\" src=\"/Content/Display/iCon/skype-icon.png\" alt=\"Skype\" /></a>";
                chuoi += "</div>";
            }
            ViewBag.chuoi = chuoi;
            StringBuilder chuoinew = new StringBuilder();
            var listnew = db.tblNews.Where(p => p.Active == true && p.ViewHomes == true && p.idCate==9).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < listnew.Count;i++ )
            {
               
                    chuoinew.Append(" <li><a href=\"/3/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\">" + listnew[i].Name + "</a> </li>");
                
            }
            ViewBag.chuoinew = chuoinew.ToString();
            ViewBag.chuoi = chuoi;
            StringBuilder services = new StringBuilder();
            var listServer = db.tblNews.Where(p => p.Active == true && p.ViewHomes == true && p.idCate ==8).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < listServer.Count; i++)
            {

                services.Append(" <li><a href=\"/3/" + listServer[i].Tag + "\" title=\"" + listServer[i].Name + "\">" + listServer[i].Name + "</a> </li>");

            }
            ViewBag.services = services.ToString();
            StringBuilder chuoisp =new StringBuilder();
            var listsp = db.tblProducts.Where(p => p.Active == true && p.ViewHomes == true).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < listsp.Count;i++ )
            {
                chuoisp.Append("<li><a href=\"/1/" + listsp[i].Tag + "\" title=\"" + listsp[i].Name + "\">› " + listsp[i].Name + "</a></li>");
            }
            ViewBag.chuoisp = chuoisp.ToString();

            string menu = "";
            var listmenu = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).Take(5).ToList();
            for(int i = 0; i < listmenu.Count;i++ )
            {
                menu += "  <li><h2><a href=\"/0/" + listmenu[i].Tag + "\" title=\"" + listmenu[i].Name + "\">" + listmenu[i].Name + "</a></h2></li>";
            }
            ViewBag.chuoimenu = menu; 

            var listAgency = db.tblGroupAgencies.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(5).ToList();
            string chuoiagen = "";
            for (int i = 0; i < listAgency.Count;i++ )
            {
                if (listAgency[i].Name.Length > 35)
                {
                    chuoiagen += "<li><a href=\"/4/" + listAgency[i].Tag + "\" title=\"Thay lõi lọc nước tại " + listAgency[i].Name + "\">Thay lõi lọc nước tại " + listAgency[i].Name.Substring(0, 35) + "...</a></li>";
                }
                else
                    chuoiagen += "<li><a href=\"/4/" + listAgency[i].Tag + "\" title=\"Thay lõi lọc nước tại " + listAgency[i].Name + "\" >Thay lõi lọc nước tại " + listAgency[i].Name + "</a></li>";
            }
            ViewBag.chuoiagen = chuoiagen;

            //list url
            var listurl = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string chuoiurrl = "";
            for (int i = 0; i < listurl.Count;i++ )
            {
                chuoiurrl += "<a href=\"/Websites/" + listurl[i].id + "\" title=\"" + listurl[i].Name + "\" rel=\"nofollow\">" + listurl[i].Name + "</a>" + ", ";
            }
            ViewBag.url = chuoiurrl;
            StringBuilder Chuoiimg = new StringBuilder();
            //var Imagesadw = db.tblImages.Where(p => p.Active == true && p.idMenu == 3).OrderByDescending(p => p.Ord).Take(1).ToList();
            //if (Imagesadw.Count > 0)
            //{

              
                if(Request.Browser.IsMobileDevice)
                {
                    Chuoiimg.Append("<div id=\"adwfooter\"><div class=\"support\">");
                    Chuoiimg.Append("<div class=\"leftSupport\">");
                    Chuoiimg.Append("<p><i class=\"fa fa-comments-o\" aria-hidden=\"true\"></i> Hỗ trợ khách hàng</p>");
                    Chuoiimg.Append("<a href=\"tel:"+TblConfig.HotlineIN+ "\"> " + TblConfig.HotlineIN + "</a>");
                    Chuoiimg.Append("<a href=\"tel:" + TblConfig.HotlineOUT + "\">" + TblConfig.HotlineOUT + "</a>");
                    Chuoiimg.Append("</div>");
                    Chuoiimg.Append("<div class=\"rightSupport\">");
                    Chuoiimg.Append("<p><i class=\"fa fa-clock-o\" aria-hidden=\"true\"></i> Thời gian làm việc</p>");
                    Chuoiimg.Append("<span class=\"sp1\"> 7H đến 22H</span>");
                    Chuoiimg.Append("<span class=\"sp2\"> Làm cả thứ 7 & Chủ nhật</span>");
                    Chuoiimg.Append("</div>");
                    Chuoiimg.Append("</div></div>");

                }


                ViewBag.Chuoiimg = Chuoiimg.ToString();
            //}
            //load danh sách địa chỉ
            var listGroupAddress = db.TblGroupAddress.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            StringBuilder resultList = new StringBuilder();
            int dem = 0;
             for (int i=0;i<listGroupAddress.Count;i++)
            {
                //list con
                int id = listGroupAddress[i].id;
                //var kiemtra = db.TblAddress.Where(p => p.Active == true && p.idCate == id).OrderBy(p => p.Ord).ToList();
                //int check = kiemtra.Count / 2;
                var listAddress = db.TblAddress.Where(p => p.Active == true && p.idCate == id).OrderBy(p => p.Ord).ToList();
                if (listAddress.Count > 0)
                {
                    if (i == 0)
                        resultList.Append("<div class=\"contentLocation showLocation\" id=\"location" + listGroupAddress[i].id + "\">");
                    else
                        resultList.Append("<div class=\"contentLocation\" id=\"location" + listGroupAddress[i].id + "\">");
               
                    for (int j = 0; j < listAddress.Count; j++)
                    {
                        resultList.Append("<div class=\"tearLoca\">");
                        resultList.Append("<p class=\"p1\">Vincom VN " + listAddress[j].Name + "</p>");
                        resultList.Append("<p class=\"p2\">" + listAddress[j].Address + "</p>");
                        resultList.Append("<a href=\"\" title=\"\"> <i class=\"fa fa-map-marker\" aria-hidden=\"true\"></i> Bản đồ đường đi</a>");
                        resultList.Append("</div>");
                    }

               
                    resultList.Append("</div>");
                }
                if (i % 8 == 0)
                {
                    result.Append("<ul class=\"listLocation\">");
                    dem += 1;
                }
                if(i==0)
                result.Append("<li><a title=\"" + listGroupAddress[i].Name + "\" class=\" location" + listGroupAddress[i].id + " set\" onclick=\"javascript:return getLocation('location" + listGroupAddress[i].id + "');\">" + listGroupAddress[i].Name + "</a></li>");
                else
                    result.Append("<li><a title=\"" + listGroupAddress[i].Name + "\" class=\" location" + listGroupAddress[i].id + "\" onclick=\"javascript:return getLocation('location" + listGroupAddress[i].id + "');\">" + listGroupAddress[i].Name + "</a></li>");

                if (i + 1 == 8 * dem)
                {
                    result.Append("</ul >");
                    result.Append(resultList.ToString());
                   resultList.Clear();
                }
                


            }
            result.Append("</ul>");
            ViewBag.result = result.ToString();
            ViewBag.resultList = resultList.ToString();

            string baogia = "";
            var listbaogia = db.tblGroupProducts.Where(p => p.Active == true && p.Baogia == true).OrderBy(p => p.Ord).Take(5).ToList();
            for (int i = 0; i < listbaogia.Count; i++)
            {
                baogia += "<li class=\"li2\">";
                baogia += "<a href=\"/Bao-gia/Bao-gia-" + listbaogia[i].Tag + "\" title=\"Bảng Báo giá " + listbaogia[i].Name + "\">› Bảng báo giá " + listbaogia[i].Name + "</a>";
                baogia += "</li>";
            }
            ViewBag.baogia = baogia;


            return PartialView(TblConfig);
        }
        public ActionResult CommandCall(string phone, string content)
        {
            string result = "";
            if (phone != null && phone != "")
            {

                var config = db.TblConfigs.First();
                var fromAddress = config.UserEmail;
                string fromPassword = config.PassEmail;
                var toAddress = config.Email;
                MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
                mailMessage.Subject = "Bạn nhận yêu cầu gọi điện thay lõi lọc nước " + DateTime.Now + "";
                mailMessage.Body = "Số điện thoại " + phone + ", nội dung " + content + "";
                //try
                //{

                SmtpClient smtpClient = new SmtpClient();
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = fromAddress,
                    Password = fromPassword
                };
                //smtpClient.UseDefaultCredentials = false;
                smtpClient.Send(mailMessage);
                result = "Bạn đã yêu cầu gọi điện thành công, bạn vui lòng cầm điện thoại trong khoảng 1-2 phút, chúng tôi sẽ liên hệ với bạn ngay !";
            }

            //}
            //catch(Exception ex)
            //{
            //    result = "Rất tiếc hiện chúng tôi không thể gọi cho bạn được, bạn có thể liên hệ qua hotline ở trên !" + ex;
            //}


            return Json(new { result = result });

        }

    }
}
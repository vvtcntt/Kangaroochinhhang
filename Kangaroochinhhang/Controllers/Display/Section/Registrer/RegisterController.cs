using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
 using System.Net.Mail;
namespace Kangaroochinhhang.Controllers.Display.Section.Registrer
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        public ActionResult Index()
        {
            var listloiloc = db.tblLoilocs.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            ViewBag.Title = "<title>Đăng ký thay lõi lọc nước tại nhà định kỳ !</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Đăng ký thay lõi lọc nước định kỳ tại nhà tại VINCOM giúp khách hàng quản lý được chiếc máy lọc nước nhà mình một cách tốt nhất. Dịch vụ nhanh và hoàn hảo !\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Thay lõi lọc nước định kỳ, thay lõi lọc nước tại nhà định kỳ nhanh chóng, dịch vụ dăng ký thay lõi lọc nước tại nhà định kỳ\" /> ";
            string chuoi = "";
            for (int i = 0; i < listloiloc.Count; i++)
            {
                chuoi += "<div class=\"loi\">";
                chuoi += " <input type=\"checkbox\" name=\"chk_" + listloiloc[i].id + "\" id=\"chk_" + listloiloc[i].id + "\" /> <span>: " + listloiloc[i].Name + "</span>";
                chuoi += "  </div>";
            }
            ViewBag.loiloc = chuoi;
            ViewBag.h1=" <h1 style=\"margin:0px; font-size:10px; opacity:0.2\">Đăng ký thay lõi lọc nước tại nhà định kỳ</h1>";
            ViewBag.favicon = " <link href=\"" + db.TblConfigs.First().Favicon + "\" rel=\"icon\" type=\"image/x-icon\" />";
            return View();
        }
        public PartialViewResult PartialRegister()
        {
            var listloiloc = db.tblLoilocs.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();

            string chuoi = "";
            for (int i = 0; i < listloiloc.Count; i++)
            {
                chuoi+="<div class=\"loi\">";
                chuoi += " <input type=\"checkbox\" name=\"chk_" + listloiloc[i].id + "\" id=\"chk_" + listloiloc[i].id + "\" /> <span>: " + listloiloc[i].Name + "</span>";
                  chuoi+="  </div>";
            }
            ViewBag.loiloc = chuoi;
           
           
                return PartialView();
        }
        public ActionResult Command(FormCollection collection)
        {

            tblRegister register = new tblRegister();
            register.Name = collection["txtName"];
            register.Mobile = collection["txtMobile"];
            register.Address = collection["txtAddress"];
            register.Email = collection["txtEmail"];
            if (collection["txtDatemua"] != null)
            {
                register.DateTimebyy = DateTime.Parse(collection["txtDatemua"]);
            }
            else
            { register.DateTimebyy = DateTime.Now; }

            if (collection["txtDatethay"] != null)
            { register.DateTime = DateTime.Parse(collection["txtDatethay"]); }
            else
            { register.DateTime = DateTime.Now; }

            db.tblRegisters.Add(register);
            db.SaveChanges();

            foreach (string key in Request.Form)
            {
                var checkbox = "";
                if (key.StartsWith("chk_"))
                {
                    checkbox = Request.Form["" + key];
                    if (checkbox != "false")
                    {
                        Int32 idll = Convert.ToInt32(key.Remove(0, 4));
                        int idkh = int.Parse(db.tblRegisters.OrderByDescending(p => p.id).ToList()[0].id.ToString());
                        tblConnectLoiloc clloiloc = new tblConnectLoiloc();
                        clloiloc.idkh = idkh;
                        clloiloc.idll = idll;
                        db.tblConnectLoilocs.Add(clloiloc);
                        db.SaveChanges();
                    }
                }
            }
            TblConfig config = db.TblConfigs.First();
            var fromAddress = config.UserEmail;
            var toAddress = config.Email;
            var MaxRigister = db.tblRegisters.OrderByDescending(p => p.id).Take(1).ToList();
            string fromPassword = config.PassEmail;
            string ararurl = Request.Url.ToString();
            var listurl = ararurl.Split('/');
            string urlhomes = "";
            for (int i = 0; i < listurl.Length - 2; i++)
            {
                if (i > 0)
                    urlhomes += "/" + listurl[i];
                else
                    urlhomes += listurl[i];
            }
            int Makh = int.Parse(MaxRigister[0].id.ToString());
            string subject = "Đăng ký thay lõi lọc nước thành công";
            string chuoihtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title>Thông tin đơn hàng</title></head><body style=\"background:#f2f2f2; font-family:Arial, Helvetica, sans-serif\"><div style=\"width:750px; height:auto; margin:5px auto; background:#FFF; border-radius:5px; overflow:hidden\">";
            chuoihtml += "<div style=\"width:100%; height:40px; float:left; margin:0px; background:#1c7fc4\"><span style=\"font-size:14px; line-height:40px; color:#FFF; margin:auto 20px; float:left\">" + DateTime.Now.Date + "</span><span style=\"font-size:14px; line-height:40px; float:right; margin:auto 20px; color:#FFF; text-transform:uppercase\">Hotline : " + config.HotlineIN + "</span></div>";
            chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px\"><div style=\"width:35%; height:100%; margin:0px; float:left\"><a href=\"/\" title=\"\"><img src=\"" + urlhomes + "" + config.Logo + "\" alt=\"Logo\" style=\"margin:8px; display:block; max-height:95% \" /></a></div><div style=\"width:60%; height:100%; float:right; margin:0px; text-align:right\"><span style=\"font-size:18px; margin:20px 5px 5px 5px; display:block; color:#ff5a00; text-transform:uppercase\">" + config.Name + "</span><span style=\"display:block; margin:5px; color:#515151; font-size:13px; text-transform:uppercase\">Lớn nhất - Chính hãng - Giá rẻ nhất việt nam</span> </div>  </div>";
            chuoihtml += "<span style=\"text-align:center; margin:10px auto; font-size:20px; color:#000; font-weight:bold; text-transform:uppercase; display:block\">Thông tin dăng ký</span>";
            chuoihtml += " <div style=\"width:90%; height:auto; margin:10px auto; background:#f2f2f2; padding:15px\">";
            chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Website đăng ký : <span style=\"color:#1c7fc4\">" + urlhomes + "</span></p>";
            chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Ngày đăng ký : <span style=\"color:#1c7fc4\">Vào lúc " + DateTime.Now.Hour + " giờ " + DateTime.Now.Minute + " phút ( ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + ") </span></p>";
            chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Mã khách hàng : <span style=\"color:#1c7fc4\">" + Makh + " </span></p>";

            chuoihtml += "<div style=\" width:100%; margin:15px 0px\">";
            chuoihtml += "<div style=\"width:90%; height:auto; float:left; margin:0px; border:1px solid #d5d5d5\">";
            chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">    	Thông tin đăng ký   </div>";
            chuoihtml += "<div style=\"width:100%; height:auto; float:left\">";
            chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Họ và tên :<span style=\"font-weight:bold\"> " + collection["txtName"] + "</span></p>";
            chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Địa chỉ :<span style=\"font-weight:bold\"> " + collection["txtAddress"] + "</span></p>";
            chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Điện thoại :<span style=\"font-weight:bold\"> " + collection["txtMobile"] + "</span></p>";
            chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Email :<span style=\"font-weight:bold\">" + collection["txtEmail"] + "</span></p>";
            chuoihtml += "</div>";
            chuoihtml += "</div>";

            chuoihtml += "</div>";
            chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px\">";
            chuoihtml += "<hr style=\"width:80%; height:1px; background:#d8d8d8; margin:20px auto 10px auto\" />";
            chuoihtml += "<p style=\"font-size:12px; text-align:center; margin:5px 5px\">" + config.Address + "</p>";
            chuoihtml += "<p style=\"font-size:12px; text-align:center; margin:5px 5px\">Điện thoại : " + config.MobileIN + " - " + config.HotlineIN + "</p>";
            chuoihtml += " <p style=\"font-size:12px; text-align:center; margin:5px 5px; color:#ff7800\">Thời gian mở cửa : Từ 7h30 đến 18h30 hàng ngày (làm cả thứ 7, chủ nhật). Hãy liên hệ để được giảm giá.</p>";
            chuoihtml += "</div>";
            chuoihtml += "<div style=\"clear:both\"></div>";
            chuoihtml += " </div>";
            chuoihtml += " <div style=\"width:100%; height:40px; float:left; margin:0px; background:#1c7fc4\">";
            chuoihtml += "<span style=\"font-size:12px; text-align:center; color:#FFF; line-height:40px; display:block\">Copyright (c) 2002 – 2015 VINCOM VIET NAM. All Rights Reserved</span>";
            chuoihtml += " </div>";
            chuoihtml += "</div>";
            chuoihtml += "</body>";
            chuoihtml += "</html>";
            string body = chuoihtml;
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = config.Host;
                smtp.Port = int.Parse(config.Port.ToString());
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = int.Parse(config.Timeout.ToString());
            }
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,


            })
            {
                smtp.Send(message);

            }
            string emails = collection["txtEmail"];
            using (var messages = new MailMessage(fromAddress, emails)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,


            })
            {
                smtp.Send(messages);

            }


            Session["Register"] = "<script>$(document).ready(function(){ alert('Bạn đã đăng ký thành công !') });</script>";
            return Redirect("/");
        }
             
         
         
	}
}
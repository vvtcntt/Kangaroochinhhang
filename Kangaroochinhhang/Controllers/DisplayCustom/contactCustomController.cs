using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;

namespace Kangaroochinhhang.Controllers.DisplayCustom
{
    public class contactCustomController : Controller
    {
        // GET: contactCustom
        KangaroochinhhangContext db = new KangaroochinhhangContext();
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.Title = "<title>Liên hệ Thay lõi máy lọc nước tại nhà</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Liên hệ địa chỉ thay lõi lọc nước tại nhà dành cho khách hàng Việt Nam\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"thay lõi lọc nước tại nhà\" /> ";
            if (Session["Status"] != null && Session["Status"] != "")
            {
                ViewBag.States = Session["Status"].ToString();
                Session["Status"] = "";
            }
            ViewBag.Maps = db.tblMaps.FirstOrDefault().Content;
            var TblConfig = db.TblConfigs.FirstOrDefault();
            ViewBag.address = TblConfig.Address;
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                 int idManu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.name = "Kênh phân phối chính hãng " + manufactures.Name;
                ViewBag.color = manufactures.Color;
                ViewBag.hotline = manufactures.Hotline1 +" - "+ manufactures.Hotline2;
                ViewBag.mobile = manufactures.Phone;
                ViewBag.logo = manufactures.Logo;
                ViewBag.email = manufactures.Email;
                ViewBag.website = manufactures.Website;
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                ViewBag.Title = "<title>Liên hệ "+manufactures.Name+"</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"Liên hệ địa chỉ " + manufactures.Name + " tại nhà dành cho khách hàng Việt Nam\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + manufactures.Name + "\" /> ";
                return View(manufactures);
            }
            else
            {
                ViewBag.logo = TblConfig.Logo;
                ViewBag.email = TblConfig.Email;
                ViewBag.hotline = TblConfig.HotlineIN;
                ViewBag.mobile = TblConfig.MobileIN;
                ViewBag.layout = "~/Views/Shared/_LayoutDefault.cshtml";

                return View(db.TblConfigs.FirstOrDefault());
            }
            ViewBag.address = db.TblConfigs.FirstOrDefault().Address;
            return View(db.SelectListItem.First());
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            if (collection["btnSend"] != null)
            {
                string nName = collection["txtName"];
                string nAddress = collection["txtAddress"];
                string nMobile = collection["txtMobile"];
                string nEmail = collection["txtEmail"];
                string nContent = collection["txtContent"];
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
                tblContact tblcontact = new tblContact();
                tblcontact.Name = nName;
                tblcontact.Address = nAddress;
                tblcontact.Mobile = nMobile;
                tblcontact.Email = nEmail;
                tblcontact.Content = nContent;
                db.tblContacts.Add(tblcontact);
                db.SaveChanges();

                var config = db.TblConfigs.First();
                var fromAddress = config.UserEmail;
                string fromPassword = config.PassEmail;
                var toAddress = config.Email;
                string subject = "Bạn có liên hệ mới từ  " + urlhomes + "";
                string chuoihtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title>Thông tin đơn hàng</title></head><body style=\"background:#f2f2f2; font-family:Arial, Helvetica, sans-serif\"><div style=\"width:750px; height:auto; margin:5px auto; background:#FFF; border-radius:5px; overflow:hidden\">";
                chuoihtml += "<div style=\"width:100%; height:40px; float:left; margin:0px; background:#1c7fc4\"><span style=\"font-size:14px; line-height:40px; color:#FFF; margin:auto 20px; float:left\">" + DateTime.Now.Date + "</span><span style=\"font-size:14px; line-height:40px; float:right; margin:auto 20px; color:#FFF; text-transform:uppercase\">Hotline : " + config.HotlineIN + "</span></div>";
                chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px\"><div style=\"width:35%; height:100%; margin:0px; float:left\"><a href=\"/\" title=\"\"><img src=\"" + urlhomes + "" + config.Logo + "\" alt=\"Logo\" style=\"margin:8px; display:block; max-height:95% \" /></a></div><div style=\"width:60%; height:100%; float:right; margin:0px; text-align:right\"><span style=\"font-size:18px; margin:20px 5px 5px 5px; display:block; color:#ff5a00; text-transform:uppercase\">" + config.Name + "</span><span style=\"display:block; margin:5px; color:#515151; font-size:13px; text-transform:uppercase\">Lớn nhất - Chính hãng - Giá rẻ nhất việt nam</span> </div>  </div>";
                chuoihtml += "<span style=\"text-align:center; margin:10px auto; font-size:20px; color:#000; font-weight:bold; text-transform:uppercase; display:block\">Thông tin Liên hệ</span>";
                chuoihtml += " <div style=\"width:90%; height:auto; margin:10px auto; background:#f2f2f2; padding:15px\">";
                chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Thông tin liên hệ từ website : <span style=\"color:#1c7fc4\">" + urlhomes + "</span></p>";
                chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Ngày gửi liên hệ : <span style=\"color:#1c7fc4\">Vào lúc " + DateTime.Now.Hour + " giờ " + DateTime.Now.Minute + " phút ( ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + ") </span></p>";

                chuoihtml += "<div style=\" width:100%; margin:15px 0px\">";
                chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px; border:1px solid #d5d5d5\">";
                chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">    	Thông tin người gửi     </div>";

                chuoihtml += "<div style=\"width:100%; height:auto; float:left\">";
                chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Họ và tên :<span style=\"font-weight:bold\"> " + nName + "</span></p>";
                chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Địa chỉ :<span style=\"font-weight:bold\"> " + nAddress + "</span></p>";
                chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Điện thoại :<span style=\"font-weight:bold\"> " + nMobile + " </span></p>";
                chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Email :<span style=\"font-weight:bold\">" + nEmail + "</span></p>";

                chuoihtml += "<div style=\"width:90%; height:auto; margin:10px auto; border:1px solid #d5d5d5\">";
                chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">   	Nội dung     </div>";
                chuoihtml += " <div style=\"width:100%; height:auto; float:left\">";
                chuoihtml += "<p style=\"font-size:12px; margin:5px 10px; font-weight:bold; color:#F00\"> - " + nContent + "</p>";
                chuoihtml += "</div>";
                chuoihtml += "</div>";

                chuoihtml += "</div>";


                chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px\">";
                chuoihtml += "<hr style=\"width:80%; height:1px; background:#d8d8d8; margin:20px auto 10px auto\" />";
                chuoihtml += "<p style=\"font-size:12px; text-align:center; margin:5px 5px\">" + config.Address + "</p>";
                chuoihtml += "<p style=\"font-size:12px; text-align:center; margin:5px 5px\">Điện thoại : " + config.MobileIN + " - " + config.HotlineIN + "</p>";
                chuoihtml += " <p style=\"font-size:12px; text-align:center; margin:5px 5px; color:#ff7800\">Thời gian mở cửa : Từ 7h30 đến 18h30 hàng ngày (làm cả thứ 7, chủ nhật). Khách hàng đến trực tiếp xem hàng giảm thêm giá.</p>";
                chuoihtml += "</div>";
                chuoihtml += "<div style=\"clear:both\"></div>";
                chuoihtml += " </div>";
                chuoihtml += " <div style=\"width:100%; height:40px; float:left; margin:0px; background:#1c7fc4\">";
                chuoihtml += "<span style=\"font-size:12px; text-align:center; color:#FFF; line-height:40px; display:block\">Copyright (c) 2002 – 2015 SONHA VIET NAM. All Rights Reserved</span>";
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


                Session["Status"] = "<script>$(document).ready(function(){ alert('Bạn đã đặt hàng thành công !') });</script>";
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}
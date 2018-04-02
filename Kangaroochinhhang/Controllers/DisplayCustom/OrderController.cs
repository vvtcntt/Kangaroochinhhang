using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kangaroochinhhang.Models;
using System.Net;
using System.Net.Mail;

namespace Kangaroochinhhang.Controllers.Display.Product
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        float tongtien = 0;
        //
        // GET: /Order/

        KangaroochinhhangContext db = new KangaroochinhhangContext();
         [HttpPost]
        public ActionResult Command(FormCollection collection, string tag)
        {
            if (collection["btnorder"] != null)
            {
                try
                {
                    string Name = collection["Name"];
                    string Address = collection["Address"];
                    string DateByy = collection["DateByy"];
                    string Mobile = collection["Mobile"];
                    string Mobiles = collection["Mobiles"];
                    string Email = collection["Email"]; 
                    string Name1 = collection["Name1"];
                    string Address1 = collection["Address"];
                     string Mobile1 = collection["Mobile1"];
                    string Mobile1s = collection["Mobile1s"]; 
                    string Email1 = collection["Email1"];
                    string rdtt = collection["rdtt"];
                    string rdvc = collection["rdvc"];
                    string NameCP = collection["NameCP"];
                    string AddressCP = collection["AddressCP"];
                    string MST = collection["MST"];
                    string Description = collection["Description"];
                    var Sopping = (clsGiohang)Session["giohang"];
                    tblOrder order = new tblOrder();
                    order.Name = Name;
                    order.Name1 = Name1;
                    order.Address = Address;
                    order.Address1 = Address1;
                    order.DateByy = DateTime.Now;
                    order.Mobile = Mobile;
                    order.Mobile1 = Mobile1; 
                    order.Mobiles = Mobiles; 
                    order.Mobile1s = Mobile1s;
                    order.TypePay = int.Parse(rdtt);
                    order.Email1 = Email1; 
                     order.Email = Email;
                     order.NameCP = NameCP;
                     order.AddressCP = AddressCP;
                     order.MST = MST;
                     order.TypePay = int.Parse(rdtt);
                     order.TypeTransport = int.Parse(rdvc);
                    order.Status = false;
                    order.Description = Description;
                    order.Active = true;
                    tblOrderDetail orderdetail = new tblOrderDetail();
                    var MaxOrd = db.tblOrders.OrderByDescending(p => p.id).Take(1).ToList();
                    int idOrder = 1;
                    
                    if (MaxOrd.Count > 0)
                        idOrder = MaxOrd[0].id;
                    for (int i = 0; i < Sopping.CartItem.Count; i++)
                    {
                        orderdetail.idProduct = Sopping.CartItem[i].id;
                        orderdetail.Name = Sopping.CartItem[i].Name;
                        orderdetail.Price = Sopping.CartItem[i].Price;
                        orderdetail.Quantily = Sopping.CartItem[i].Ord;
                        orderdetail.SumPrice = Sopping.CartItem[i].SumPrice;
                        orderdetail.idOrder = idOrder;
                        db.tblOrderDetails.Add(orderdetail);
                        db.SaveChanges();
                    }
                    TblConfig config = db.TblConfigs.First();
                    var fromAddress = config.UserEmail;
                    var toAddress = config.Email;
                    var orders = db.tblOrders.OrderByDescending(p => p.id).Take(1).ToList();
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
                     order.Tolar = Sopping.CartTotal;
                    db.tblOrders.Add(order);
                    db.SaveChanges();
                    string subject = "Đơn hàng mới từ "+urlhomes+"";
                    string chuoihtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title>Thông tin đơn hàng</title></head><body style=\"background:#f2f2f2; font-family:Arial, Helvetica, sans-serif\"><div style=\"width:750px; height:auto; margin:5px auto; background:#FFF; border-radius:5px; overflow:hidden\">";
                    chuoihtml += "<div style=\"width:100%; height:40px; float:left; margin:0px; background:#1c7fc4\"><span style=\"font-size:14px; line-height:40px; color:#FFF; margin:auto 20px; float:left\">" + DateTime.Now.Date + "</span><span style=\"font-size:14px; line-height:40px; float:right; margin:auto 20px; color:#FFF; text-transform:uppercase\">Hotline : " + config.HotlineIN + "</span></div>";
                    chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px\"><div style=\"width:35%; height:100%; margin:0px; float:left\"><a href=\"/\" title=\"\"><img src=\"" + urlhomes + "" + config.Logo + "\" alt=\"Logo\" style=\"margin:8px; display:block; max-height:95% \" /></a></div><div style=\"width:60%; height:100%; float:right; margin:0px; text-align:right\"><span style=\"font-size:18px; margin:20px 5px 5px 5px; display:block; color:#ff5a00; text-transform:uppercase\">" + config.Name + "</span><span style=\"display:block; margin:5px; color:#515151; font-size:13px; text-transform:uppercase\">Lớn nhất - Chính hãng - Giá rẻ nhất việt nam</span> </div>  </div>";
                    chuoihtml += "<span style=\"text-align:center; margin:10px auto; font-size:20px; color:#000; font-weight:bold; text-transform:uppercase; display:block\">Thông tin đơn hàng</span>";
                    chuoihtml += " <div style=\"width:90%; height:auto; margin:10px auto; background:#f2f2f2; padding:15px\">";
                    chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Đơn hàng từ website : <span style=\"color:#1c7fc4\">" + urlhomes + "</span></p>";
                    chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Ngày gửi đơn hàng : <span style=\"color:#1c7fc4\">Vào lúc " + DateTime.Now.Hour + " giờ " + DateTime.Now.Minute + " phút ( ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + ") </span></p>";
                    chuoihtml += "<p style=\"font-size:14px; color:#464646; margin:5px 20px\">Mã đơn hàng : <span style=\"color:#1c7fc4\">" + idOrder + " </span></p>";
                    chuoihtml += "<p style=\"font-size:14px; color:#1b1b1b; margin:5px 20px; text-decoration:underline; text-transform:uppercase\">Danh sách sản phẩm : </p>";
                    chuoihtml += "<table style=\"width:100%; height:auto; margin:10px auto; background:#FFF; border:1px\" border=\"0\">";
                    chuoihtml += " <tr style=\" background:#1c7fc4; color:#FFF; text-align:center; line-height:25px; font-size:12px\">";

                    chuoihtml += "<td>STT</td>";
                    chuoihtml += "<td>Tên sản phẩm</td>";
                    chuoihtml += "<td>Đơn giá (vnđ)</td>";
                    chuoihtml += "<td>Số lượng</td>";
                    chuoihtml += "<td>Thành tiền (vnđ)</td>";
                    chuoihtml += "</tr>";

                    for (int i = 0; i < Sopping.CartItem.Count; i++)
                    {
                        chuoihtml += "<tr style=\"line-height:20px; font-size:13px; color:#000; text-indent:5px; border-bottom:1px dashed #cecece; margin:1px 0px;\">";
                        chuoihtml += "<td style=\"text-align:center; width:7%\">" + i + "</td>";
                        chuoihtml += "<td style=\"width:45%\">" + Sopping.CartItem[i].Name + "</td>";
                        int gia = Convert.ToInt32(Sopping.CartItem[i].Price.ToString());
                        chuoihtml += "<td style=\"text-align:center; width:15%\">" + gia.ToString().Replace(",", "") + "</td>";
                        chuoihtml += "<td style=\"text-align:center; width:10%\">" + Sopping.CartItem[i].Ord + "</td>"; 
                        float thanhtien = Sopping.CartItem[i].Price * Sopping.CartItem[i].Ord;
                        chuoihtml += "<td style=\"text-align:center; font-weight:bold\">" + thanhtien.ToString().Replace(",", "") + "</td>";
                        chuoihtml += " </tr>";

                    }
                    chuoihtml += "<tr style=\"font-size:12px; font-weight:bold\">";
                    chuoihtml += "<td colspan=\"4\" style=\"text-align:right\">Tổng giá trị đơn hàng : </td>";
                    chuoihtml += "<td style=\"font-size:14px; color:#F00\">" + Sopping.CartTotal + " vnđ</td>";
                    chuoihtml += " </tr>";
                    chuoihtml += "</table>";
                    chuoihtml += "<div style=\" width:100%; margin:15px 0px\">";
                    chuoihtml += "<div style=\"width:100%; height:auto; float:left; margin:0px; border:1px solid #d5d5d5\">";
                    chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">    	Thông tin người gửi     </div>";

                    chuoihtml += "<div style=\"width:100%; height:auto; float:left\">";
                    chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Họ và tên :<span style=\"font-weight:bold\"> " + Name + "</span></p>";
                    chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Địa chỉ :<span style=\"font-weight:bold\"> " + Address + "</span></p>";
                    chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Điện thoại :<span style=\"font-weight:bold\"> " + Mobile + " - "+Mobiles+"</span></p>";
                    chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Email :<span style=\"font-weight:bold\">" + Email + "</span></p>";
                    if(rdtt=="1")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Hình thức thanh toán :<span style=\"font-weight:bold\">Chuyển hàng thu tiền tại nhà</span></p>";

                    }
                    if (rdtt == "2")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Hình thức thanh toán :<span style=\"font-weight:bold\">Nhận hàng và thanh toán tại Kangaroochinhhang</span></p>";

                    }
                    if (rdtt == "3")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Hình thức thanh toán :<span style=\"font-weight:bold\">Chuyển khoản ATM & Ngân hàng</span></p>";

                    }
                    if (rdtt == "4")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Hình thức thanh toán :<span style=\"font-weight:bold\">Thanh toán visa, mastercard</span></p>";

                    }
                    if (rdvc == "1")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Thời gian giao hàng :<span style=\"font-weight:bold\">Bất kể giờ nào</span></p>";

                    }
                    if (rdvc == "2")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Thời gian giao hàng :<span style=\"font-weight:bold\">Trong giờ hàng chính</span></p>";

                    }
                    if (rdvc == "3")
                    {
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Thời gian giao hàng :<span style=\"font-weight:bold\">Ngoài giờ hành chính</span></p>";

                    }
                     
                    chuoihtml += "</div>";
                    chuoihtml += "</div>";
                    if(Name1!=null)
                    {
                        chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">    	Người nhận hàng   </div>";

                        chuoihtml += "<div style=\"width:100%; height:auto; float:left\">";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Họ và tên :<span style=\"font-weight:bold\"> " + Name1 + "</span></p>";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Địa chỉ :<span style=\"font-weight:bold\"> " + Address1 + "</span></p>";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Điện thoại :<span style=\"font-weight:bold\"> " + Mobile1 + " - " + Mobile1s + "</span></p>";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Email :<span style=\"font-weight:bold\">" + Email1 + "</span></p>";
                        chuoihtml += "</div>";
                        chuoihtml += "</div>";

                    }
                    if (NameCP != null)
                    {
                        chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">    	Thông tin yêu cầu hóa đơn  </div>";

                        chuoihtml += "<div style=\"width:100%; height:auto; float:left\">";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Tên công ty :<span style=\"font-weight:bold\"> " + NameCP + "</span></p>";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">Địa chỉ :<span style=\"font-weight:bold\"> " + AddressCP + "</span></p>";
                        chuoihtml += "<p style=\"font-size:12px; margin:5px 10px\">MST:<span style=\"font-weight:bold\"> " + MST+ "</span></p>";
                         chuoihtml += "</div>";
                        chuoihtml += "</div>";

                    }
                    chuoihtml += "<div style=\"width:90%; height:auto; margin:10px auto; border:1px solid #d5d5d5\">";
                    chuoihtml += "<div style=\" width:100%; height:30px; float:left; background:#1c7fc4; font-size:12px; color:#FFF; text-indent:15px; line-height:30px\">   	Yêu cầu của người gửi       </div>";
                    chuoihtml += " <div style=\"width:100%; height:auto; float:left\">";
                    chuoihtml += "<p style=\"font-size:12px; margin:5px 10px; font-weight:bold; color:#F00\"> - " + Description + "</p>";
                    chuoihtml += "</div>";
                    chuoihtml += "</div>";
                    var listo = db.tblOrders.OrderByDescending(p => p.id).Take(1).ToList();
                    chuoihtml += " <a href=\"" + urlhomes + "/Orderad/ActiveOrder?id=" + listo[0].id + "\" title=\"\" style=\"padding:5px; color:#FFF; background:#F00; display:inline-block; text-align:center; margin:10px auto\">Đã check thông tin >></a>";
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
                    chuoihtml += "<span style=\"font-size:12px; text-align:center; color:#FFF; line-height:40px; display:block\">Copyright (c) 2002 – 2015 Kangaroochinhhang VIET NAM. All Rights Reserved</span>";
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
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    Session["Status"] = "<script>$(document).ready(function(){ alert('Bạn đã đặt hàng không thành công " + ex.Message + "!') });</script>";
                    return RedirectToAction("OrderIndex");
                }

            }
            return RedirectToAction("OrderIndex");
        }
        public ActionResult Success()
        {
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefault.cshtml";
            }
            ViewBag.Title = "<title> Đặt hàng thành công !</title>";
            return View();
        }
        public ActionResult OrderIndex()
        {
            if (Session["idManu"] != null && Session["idManu"] != "")
            {
                ViewBag.layout = "~/Views/Shared/_LayoutDefaultCustom.cshtml";
                int idManu = int.Parse(Session["idManu"].ToString());
                var manufactures = db.SelectListItem.Find(idManu);
                ViewBag.manuName = manufactures.Name;
                ViewBag.color = manufactures.Color;
                var listIdNews = db.tblConnectManuToNews.Where(p => p.idManu == idManu).Select(p => p.idNews).ToList();
 
            }
            var giohang = (clsGiohang)Session["giohang"];
            ViewBag.Title = "<title>Hoàn thành đơn đặt hàng </title>";
            string chuoi = "";
          
            chuoi += "<table align=\"left\" cellpadding=\"2\">";
            chuoi += "<tr class=\"top\">";
            chuoi += "<td class=\"Name\">Tên sản phẩm</td>";
            chuoi += "<td class=\"Price\">Đơn giá</td>";
            chuoi += "<td class=\"Ord\">SL</td>";
            chuoi += "<td class=\"PriceSale\">Thành tiền</td>";
            chuoi += "</tr>";
            if(giohang!=null)
            { 
                for (int i = 0; i < giohang.CartItem.Count; i++)
                {
                    chuoi += "<tr class=\"row" + giohang.CartItem[i].id + "\" >";
                    chuoi += "<td class=\"Name\" >";
                    chuoi += "<a href=\"/San-pham/" + giohang.CartItem[i].Tag + "\" title=\"" + giohang.CartItem[i].Name + "\" id=\"UpdateOrd" + giohang.CartItem[i].id + "\"><img src=\"" + giohang.CartItem[i].Images + "\" alt=\"" + giohang.CartItem[i].Name + "\" title=\"" + giohang.CartItem[i].Name + "\" /></a>";
                    chuoi += "<a href=\"/San-pham/" + giohang.CartItem[i].Tag + "\" title=\"" + giohang.CartItem[i].Name + "\" class=\"Namepd\">" + giohang.CartItem[i].Name + "</a>";
                    chuoi += "<a href=\"javascript:void(0)\" title=\"\" class=\"Delete\" onclick=\"javascript:return DeleteOrder(" + giohang.CartItem[i].id + ")\">Xóa</a>";
                    chuoi += "</td>";
                    chuoi += "<td class=\"Price\"><span>" + string.Format("{0:#,#}", giohang.CartItem[i].Price) + " vnđ</span></td>";
                    chuoi += "<td class=\"Ord\"><input type=\"number\" name=\"Ord\"  class=\"txtOrd" + giohang.CartItem[i].id + "\" value=\"" + giohang.CartItem[i].Ord + "\" onchange=\"javascript:return UpdateOrd(" + giohang.CartItem[i].id + ")\" /></td>";
                    chuoi += "<td class=\"PriceSale\"><span id=\"Gia" + giohang.CartItem[i].id + "\">" + string.Format("{0:#,#}", giohang.CartItem[i].SumPrice) + " vnđ</span></td>";
                    chuoi += "</tr>";
                }
         
            chuoi += "</table>";
            chuoi += "  <div class=\"Sum\">";
            chuoi += "  <div class=\"LeftSUM\">";
            chuoi += "      <span>Bạn có <span class=\"count\">" + giohang.CartItem.Count + "</span> sản phẩm trong giỏ hàng</span>";
            chuoi += " </div>";
            chuoi += " <div class=\"RightSUM\">";
            chuoi += "  <span class=\"Sum1\">Tổng cộng :  <span class=\"tt\">" + string.Format("{0:#,#}", giohang.CartTotal) + "</span> vnđ </span>";
            chuoi += "  <span class=\"Sum2\">Thành tiền: <span class=\"tt\">" + string.Format("{0:#,#}", giohang.CartTotal) + "</span> vnđ </span>";
            chuoi += "  </div>";
            }
            else
            {

                chuoi += "</table>";
                chuoi += "  <div class=\"Sum\">";
                chuoi += "  <div class=\"LeftSUM\">";
                chuoi += "      <span>Bạn có <span class=\"count\">0</span> sản phẩm trong giỏ hàng</span>";
                chuoi += " </div>";
                chuoi += " <div class=\"RightSUM\">";
                chuoi += "  <span class=\"Sum1\">Tổng cộng :  <span class=\"tt\">0</span> vnđ </span>";
                chuoi += "  <span class=\"Sum2\">Thành tiền: <span class=\"tt\">0</span> vnđ </span>";
                chuoi += "  </div>";
            }
            chuoi += "</div>";
            chuoi += "<div class=\"OrderNows\">";
            chuoi += " <a href=\"/\" title=\"\" onclick=\"close_popup()\" class=\"nexorder\">Tiếp tục mua hàng</a>";
                                chuoi += "  <button type=\"submit\" style=\"float:right; margin-right:7px\" name=\"btnorder\" id=\"btnorder\" class=\"btnorder\">Gửi đơn hàng</button>";

 
            chuoi += "  </div>";
             ViewBag.chuoi=chuoi;

            if (Session["Status"] != null)
            {
                ViewBag.Status = Session["Status"];
                Session["Status"] = "";
            }
             
            ViewBag.Title = "<title>Giỏ hàng của bạn</title>";
            ViewBag.Description = "<meta name=\"description\" content=\" Giỏ hàng đặt hàng Thiết bị vệ sinh TOTO dành cho khách hàng mua sản phẩm\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Giỏ hàng của bạn\" /> ";

            //Load info công ty
            var TblConfig = db.TblConfigs.First();
            string chuoicty = "";
            chuoicty += "<div class=\"Tear_ttck\">";
            chuoicty += "<div class=\"Left_Tear_ttck\">";
            chuoicty += "<img src=\""+TblConfig.Logo+"\" alt=\"Logo công ty\" />";
            chuoicty += " </div>";
            chuoicty += "<div class=\"Right_Tear_ttck\">";

            chuoicty += "<div class=\"row_ttck\">" + TblConfig.Name + "</div>";
            chuoicty += "<div class=\"row_ttck1\">";
            chuoicty += "<span class=\"sp1\">Địa chỉ:</span><span class=\"sp2\"> " + TblConfig.Address + "</span>";
            chuoicty += "</div>";
            chuoicty += " <div class=\"row_ttck1\">";
            chuoicty += "<span class=\"sp1\">Số điện thoại:</span><span class=\"sp2\"> " + TblConfig.MobileIN + " - " + TblConfig.HotlineIN + "</span>";
            chuoicty += "</div>";
            chuoicty += "<div class=\"row_ttck1\">";
            chuoicty += "<span class=\"sp1\">Email:</span><span class=\"sp2\"> " + TblConfig.Email + "</span>";
            chuoicty += "</div>";
            chuoicty += "</div>";
            chuoicty += "</div>";
            ViewBag.chuoicongty = chuoicty;
            //Load danh sách ngân hàng
            var ListBank = db.tblBanks.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string chuoibank = "";
            for (int i = 0; i < ListBank.Count;i++ )
            {
                chuoibank += "<div class=\"Tear_ttck\">";
                chuoibank += "<div class=\"Left_Tear_ttck\">";
                chuoibank += "<img src=\"" + ListBank[i].Images + "\" alt=\"" + ListBank[i].NameBank + "\" />";
                chuoibank += " </div>";
                chuoibank += "<div class=\"Right_Tear_ttck\">";

                chuoibank += "<div class=\"row_ttck\">" + ListBank[i].NameBank + "</div>";
                chuoibank += "<div class=\"row_ttck1\">";
                chuoibank += "<span class=\"sp1\">Chi nhánh:</span><span class=\"sp2\"> " + ListBank[i].Address + "</span>";
                chuoibank += "</div>";
                chuoibank += " <div class=\"row_ttck1\">";
                chuoibank += "<span class=\"sp1\">Chủ TK:</span><span class=\"sp2\"> " + ListBank[i].Name + "</span>";
                chuoibank += "</div>";
                chuoibank += "<div class=\"row_ttck1\">";
                chuoibank += "<span class=\"sp1\">Số TK:</span><span class=\"sp2\"> " + ListBank[i].NumberBank + "</span>";
                chuoibank += "</div>";
                chuoibank += "</div>";
                chuoibank += "</div>";

            }
            ViewBag.chuoibank = chuoibank;
                return View();
        }       
        public ActionResult OrderAdd(int id, int Ord)
        {
           
            int sl = 0;
            var Sopping = (clsGiohang)Session["giohang"];
            if (Sopping == null)
            {
                Sopping = new clsGiohang();
            }
            if (Kiemtra(id) == 1)
            {
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    if (Sopping.CartItem[i].id == id)
                    {
                        Sopping.CartItem[i].Ord = Sopping.CartItem[i].Ord + Ord;
                        Sopping.CartItem[i].SumPrice = Sopping.CartItem[i].Ord * Sopping.CartItem[i].Price;
                    }
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;
            }
            else
            {
                var Sanpham = new clsProduct();
                Sanpham.id = id;
                var Product = db.tblProducts.Find(id);
                Sanpham.Price = int.Parse(Product.PriceSale.ToString());
                Sanpham.Ord = Ord;
                Sanpham.Name = Product.Name;
                Sanpham.SumPrice = Sanpham.Price * Sanpham.Ord;
                Sanpham.Tag = Product.Tag;
                Sopping.CartItem.Add(Sanpham);
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;

            }

         Session["giohang"] = Sopping;
         sl = Sopping.CartItem.Count;
         var s = (clsGiohang)Session["giohang"];


         Session["soluong"] = sl;
         return RedirectToAction("OrderIndex", "Order");            
        }
        public int Kiemtra(int idProduct)
        {
            int so = 0;
            var Sopping =(clsGiohang) Session["giohang"];
            if (Sopping != null)
            {
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    if (Sopping.CartItem[i].id == idProduct)
                    {
                        so = 1; break;
                    }



                }

            }
            return so;
        }
        public ActionResult UpdatOder(int id, int ord)
        {
            float tt = 0;
            float tien=0;
            int sl=0;
            var s = (clsGiohang)Session["giohang"];
            if (s != null)
            {
                for (int i = 0; i < s.CartItem.Count; i++) 
                {
                    if (id == s.CartItem[i].id) {
                        s.CartItem[i].Ord = ord;
                        s.CartItem[i].SumPrice = ord * s.CartItem[i].Price;
                        tien = float.Parse(s.CartItem[i].SumPrice.ToString());
                    }

                    tt += s.CartItem[i].SumPrice;

                }

                s.CartTotal = Convert.ToInt32(tt);
                sl = s.CartItem.Count;
            }
            Session["giohang"] = s;
            tt = Convert.ToInt32(tt);
             return Json(new { gia = string.Format("{0:#,#}", tien), tt = string.Format("{0:#,#}", tt), sl = sl });
        }
        public ActionResult DeleteOrder(int id)
        {
            var s = (clsGiohang)Session["giohang"];
            int sl = 0;
            for (int i = 0; i < s.CartItem.Count; i++)
            {
                if (s.CartItem[i].id == id)
                    s.CartItem.Remove(s.CartItem[i]);
            }
            sl = s.CartItem.Count;

            for (int i = 0; i < s.CartItem.Count; i++)
            {
                tongtien += s.CartItem[i].SumPrice;
            }
            s.CartTotal = tongtien;

            Session["soluong"] = sl;

            return Json(new { thongtin = "", sl = sl, tt = string.Format("{0:#,#}", s.CartTotal), count = s.CartItem.Count });
        }
        public PartialViewResult OrderPartial()
        {
            return PartialView();
        }
        public PartialViewResult InputOrder()
        {
            return PartialView();
        }
        public ActionResult Create(string idp, string ord)
        {
            int id = int.Parse(idp);
            int Ord = int.Parse(ord);

            int sl = 0;
            var Sopping = (clsGiohang)Session["giohang"];
            if (Sopping == null)
            {
                Sopping = new clsGiohang();
            }
            if (Kiemtra(id) == 1)
            {
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    if (Sopping.CartItem[i].id == id)
                    {
                        Sopping.CartItem[i].Ord = Sopping.CartItem[i].Ord + Ord;
                        Sopping.CartItem[i].SumPrice = Sopping.CartItem[i].Ord * Sopping.CartItem[i].Price;
                    }
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;
            }
            else
            {
                var Sanpham = new clsProduct();
                Sanpham.id = id;
                var Product = db.tblProducts.Find(id);
                Sanpham.Price = int.Parse(Product.PriceSale.ToString());
                Sanpham.Ord = Ord;
                Sanpham.Name = Product.Name;
                Sanpham.SumPrice = Sanpham.Price * Sanpham.Ord;
                Sanpham.Tag = Product.Tag;
                Sanpham.Images = Product.ImageLinkThumb;
                Sopping.CartItem.Add(Sanpham);
                for (int i = 0; i < Sopping.CartItem.Count; i++)
                {
                    tongtien += Sopping.CartItem[i].SumPrice;
                }
                Sopping.CartTotal = tongtien;

            }

            Session["giohang"] = Sopping;
            sl = Sopping.CartItem.Count;
            var s = (clsGiohang)Session["giohang"];


            Session["soluong"] = sl;
            var giohang = (clsGiohang)Session["giohang"];
            string chuoi = "";
            chuoi += " <div id=\"OrderPopup\">";
            chuoi += "<a href=\"javascript:void(0)\" title=\"\" class=\"close_popup\" onclick=\"close_popup()\"></a>";
            chuoi += "<table    align=\"left\" cellpadding=\"2\" >";
            chuoi += "<tr class=\"top\">";
            chuoi += "<td class=\"Name\">Tên sản phẩm</td>";
            chuoi += "<td class=\"Price\">Đơn giá</td>";
            chuoi += "<td class=\"Ord\">SL</td>";
            chuoi += "<td class=\"PriceSale\">Thành tiền</td>";
            chuoi += "</tr>";
            chuoi += "</table>";
            chuoi += "<div id=\"Content_OrderPopup\">";
            chuoi += "<table align=\"left\" cellpadding=\"2\">";

            for (int i = 0; i < giohang.CartItem.Count; i++)
            {
                chuoi += "<tr class=\"row" + giohang.CartItem[i].id + "\" >";
                chuoi += "<td class=\"Name\" >";
                chuoi += "<a href=\"/San-pham/" + giohang.CartItem[i].Tag + "\" title=\"" + giohang.CartItem[i].Name + "\" id=\"UpdateOrd" + giohang.CartItem[i].id + "\"><img src=\"" + giohang.CartItem[i].Images + "\" alt=\"" + giohang.CartItem[i].Name + "\" title=\"" + giohang.CartItem[i].Name + "\" /></a>";
                chuoi += "<a href=\"/San-pham/" + giohang.CartItem[i].Tag + "\" title=\"" + giohang.CartItem[i].Name + "\" class=\"Namepd\">" + giohang.CartItem[i].Name + "</a>";
                chuoi += "<a href=\"javascript:void(0)\" title=\"\" class=\"Delete\" onclick=\"javascript:return DeleteOrder(" + giohang.CartItem[i].id + ")\">Xóa</a>";
                chuoi += "</td>";
                chuoi += "<td class=\"Price\"><span>" + string.Format("{0:#,#}", giohang.CartItem[i].Price) + " vnđ</span></td>";
                chuoi += "<td class=\"Ord\"><input type=\"number\" name=\"Ord\"  class=\"txtOrd" + giohang.CartItem[i].id + "\" value=\"" + giohang.CartItem[i].Ord + "\" onchange=\"javascript:return UpdateOrd(" + giohang.CartItem[i].id + ")\" /></td>";
                chuoi += "<td class=\"PriceSale\"><span id=\"Gia" + giohang.CartItem[i].id + "\">" + string.Format("{0:#,#}", giohang.CartItem[i].SumPrice) + " vnđ</span></td>";
                chuoi += "</tr>";
            }
            chuoi += "</table>";
            chuoi += "</div>";
            chuoi += "  <div class=\"Sum\">";
            chuoi += "  <div class=\"LeftSUM\">";
            chuoi += "      <span>Bạn có <span class=\"count\">" + giohang.CartItem.Count + "</span> sản phẩm trong giỏ hàng</span>";
            chuoi += " </div>";
            chuoi += " <div class=\"RightSUM\">";
            chuoi += "  <span class=\"Sum1\">Tổng cộng :  <span class=\"tt\">" + string.Format("{0:#,#}", giohang.CartTotal) + "</span> vnđ </span>";
            chuoi += "  <span class=\"Sum2\">Thành tiền: <span class=\"tt\">" + string.Format("{0:#,#}", giohang.CartTotal) + "</span> vnđ </span>";
            chuoi += "  </div>";

            chuoi += "</div>";
            chuoi += "<div class=\"OrderNows\">";
            chuoi += " <a href=\"javascript:void(0)\" title=\"\" onclick=\"close_popup()\" class=\"nexorder\">Tiếp tục mua hàng</a>";
            chuoi += " <a href=\"/Order/OrderIndex\" title=\"\" class=\"noworder\">Tiến hành đặt hàng</a>";

            chuoi += "  </div>";
            chuoi += "</div>";
            var result = chuoi;

            return Json(new { result = result, sl = sl });

        }
    }
}

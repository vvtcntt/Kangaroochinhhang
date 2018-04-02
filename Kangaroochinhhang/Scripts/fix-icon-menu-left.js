/**
 * Created by tranngoclam1909 on 9/8/14.
 */
function menu_icon_left() {
    var $style_menu_icon_left = $('.site-navigation');
    if ($(window).scrollTop() > 450)
        $style_menu_icon_left.css({'opacity': 1,'left': '2%'});
    else
        $style_menu_icon_left.css({'opacity': 0,'left': '-2%'});
}
$(window).scroll(menu_icon_left);
menu_icon_left();
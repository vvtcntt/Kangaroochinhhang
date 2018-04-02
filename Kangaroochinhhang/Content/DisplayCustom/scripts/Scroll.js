// JavaScript Document
 $("document").ready(function($){
    var nav = $('#header');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
			$("#header").css("display", "block");
            nav.addClass("f-nav");
        } else {
			$("#header").css("display", "block");
            nav.removeClass("f-nav");
        }
    });
});
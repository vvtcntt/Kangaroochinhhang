// JavaScript Document
 $("document").ready(function($){
    var nav = $('#Box_MXH1');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 600) {
			$("#Box_MXH1").css("width", "198px");
			$("#Box_MXH1").css("display", "block");
			
            nav.addClass("f-nav");
        } else {
			$("#Box_MXH1").css("display", "none");
            nav.removeClass("f-nav");
        }
		 
    });
});
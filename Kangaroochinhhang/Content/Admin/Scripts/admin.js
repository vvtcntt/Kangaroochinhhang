function ShowLoadingIndicator() {
    if (typeof(disableLoadingIndicator) != 'undefined' && disableLoadingIndicator) {
	    return;
    }
    var windowWidth = $j(window).width();
    var scrollTop;
    if(self.pageYOffset) {
	    scrollTop = self.pageYOffset;
    }
    else if(document.documentElement && document.documentElement.scrollTop) {
	    scrollTop = document.documentElement.scrollTop;
    }
    else if(document.body) {
	    scrollTop = document.body.scrollTop;
    }
    $j('#AjaxLoading').css('position', 'absolute');
    $j('#AjaxLoading').css('top', scrollTop+'px');
    $j('#AjaxLoading').css('left', parseInt((windowWidth-150)/2)+"px");
    $j('#AjaxLoading').show();
}

function HideLoadingIndicator() {
    //$j('#AjaxLoading').hide();
    $j('#AjaxLoading').delay(800).slideUp(300);
}

$j(document).ready(function() {
    $j('html').ajaxStart(function() {
	    ShowLoadingIndicator();
    });

    $j('html').ajaxComplete(function() {
	    HideLoadingIndicator();
    });
    $j('.InitialFocus').focus();

    $j(window).scroll(function () {
        var offset = $j(".widget-title").offset();
        var curWin = $j(window);
        var top = offset.top - curWin.scrollTop();
        var bottom = $j(window).height() - $j(".widget-title").height();
        bottom = bottom - offset.top;

        var compare = bottom - curWin.scrollTop();
        if (top <= 0) {
            if ($j(".scrollBox").hasClass("hidden")) {
                $j(".scrollBox").removeClass("hidden");

            }
        } else {
            if (!$j(".scrollBox").hasClass("hidden")) {
                $j(".scrollBox").addClass("hidden");
            }

        }
    });
});
$(document).ready(function () {
    $('#tab2').removeClass('tab2');
    $('#tab2').hide();
    $('#tab3').removeClass('tab3');
    $('#tab3').hide();
    $('a.number').click(function () {
        var an = $('a.set').attr('title');
        $('div#' + an).hide();
        $('a.set').removeClass('set');
        $(this).addClass('set');
        var hien = $(this).attr('title');
        $('div#' + hien).show();


    })
});
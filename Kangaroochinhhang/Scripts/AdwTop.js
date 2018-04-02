try {
    var Tat_quangcao = localStorage.getItem("Tat_quangcao");
    if (Tat_quangcao != null) {
        if ((new Date).getDate() == new Date(parseInt(localStorage.getItem("Tat_quangcao"))).getDate()) {
            open_motog = false;
        }
    }
} catch (e) { console.log(e) }

function Tatquangcao(obj) {
    $(obj).parent().remove();
    try {

        $('#Top_Banner').css({ 'height': '32px' });
        $('#Content_Top_Banner').css({ 'position': 'relative', 'display': 'block' });

        localStorage.setItem("Tat_quangcao", (new Date).getTime())
    } catch (e) { console.log(e); }
}
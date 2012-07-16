$(function () {
    
});
function show(div_id, height, width) {
    $(div_id).show().css("top", ($(document).height() - height) / 2).css("left", ($(document).width() - width) / 2);
}

function hide(div_id) {
    $(div_id).hide();
}   
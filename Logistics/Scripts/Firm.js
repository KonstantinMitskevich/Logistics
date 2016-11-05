
$(function(){
	$("td").mouseover(function(){
	    $(this).css("background-color", "#DCDCDC");
	});
	$("td")
    .mouseout(function(){
        $(this).css("background-color", "#FAFAFA");
    });
});
$(function(){
    $("td").mouseover(function () {
        $(this).css("background-color", "#DCDCDC");
    });
    $("td")
    .mouseout(function () {
        $(this).css("background-color", "#FAFAFA");
    });
});

$(function () {
        $('#but').click(function () {
            var tarif = {
                Destination: Minsk,
                Price: 200
            }
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: "Firm/AddTarif2",
                data: "{tarif2:" + JSON.stringify(tarif) + "}",
                success: function (data) {
                    alert(data);
                }
            });
        });
    });



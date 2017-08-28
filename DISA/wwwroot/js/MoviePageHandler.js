$(".theater-movie-is-related-to").on("click", function () {
    var value = $(this).text();
    var number = value.replace("Sal: ", "");
    console.log(number);
    $.ajax({
        url: '/api/getTheater',
        type: "POST",
        data: JSON.stringify(number),
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (data) {
     
            $("#line-and-seats-wrapper").html("");

            // Lets control the theater screen size
            if (parseInt(number) <= 2) {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-big">Lærred</div>');
                SetupTheater("big", data);
            }
            else if (parseInt(number) <= 5) {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-medium">Lærred</div>');
                SetupTheater("medium", data);
            }
            else {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-small">Lærred</div>');
                SetupTheater("small", data);
            }
        }
    });
    console.log(number);
});

function SetupTheater(theaterSize, data) {
    var html = '<div class="theater-line-wrapper">';
    $.each(data, function (lineIndex, line) {
        
        html += '<div class="theater-line">';
        html += '<div class="theater-line-seats-wrappper">';

        $.each(line.seats, function (seatIndex, seat) {
            html += '<div class="theater-line-seat-general theater-line-seat-' + theaterSize + '" data-seat-Number="' + seat.number + '"></div>';
        });
        html += '<div class="theater-line-number">' + line.number + '</div>';
        html += '</div>';
        html += '</div>';
        
        
    });
    html += '</div>';
    $("#line-and-seats-wrapper").append(html);
}
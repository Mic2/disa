$(".theater-movie-is-related-to").on("click", function () {

    $.each(document.getElementsByClassName("theater-movie-is-related-to"), function (value) {
        $(this).removeClass("choosen");
    });
    $(this).addClass("choosen");

    var value = $(this).text();
    var number = value.replace("Sal: ", "");

    choosenTheater = number;
    showTimeId = $(this).next().next('.show-time-id').data("showtime-id");

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


    // Controlling the seats choosen by the user.
    $(".theater-line-seat-general").on("click", function () {
        var alreadyChoosen = false;
        var firstSeatChoosen = choosenSeats[0];
        var lastSeatChoosen = choosenSeats[choosenSeats.length - 1];
        var seatNumber = $(this).data("seat-number");
        var lineNumber = $(this).nextAll('.theater-line-number').text();
        console.log(choosenLine[0]);

        if (typeof choosenLine[0] === 'undefined' || choosenSeats.length === 0) {
            choosenLine[0] = lineNumber;
        }

        if ($.inArray(seatNumber, choosenSeats) !== -1) {
            alreadyChoosen = true;
            if (lineNumber === choosenLine[0] && firstSeatChoosen === seatNumber || lastSeatChoosen === seatNumber) {              
                $(this).removeClass("seat-choosen");
                var indexToRemove = choosenSeats.indexOf(seatNumber);
                choosenSeats.splice(indexToRemove, 1);
            }
        }

        
        if (alreadyChoosen === false && lineNumber === choosenLine[0]) {
            if (choosenSeats.length === 0 || lineNumber === choosenLine[0] && seatNumber === lastSeatChoosen + 1 || seatNumber === lastSeatChoosen - 1 || seatNumber === firstSeatChoosen + 1 || seatNumber === firstSeatChoosen - 1) {
                choosenSeats.push(seatNumber);
                $(this).addClass("seat-choosen");
                choosenSeats.sort();
            }
            
        }
    });

}

$("#reserveTicketForm").submit(function (e) {
    e.preventDefault();

    var fullname = document.getElementById('fullNameInput').value;
    var phoneNumber = document.getElementById('phoneNumberInput').value;
    var movieTitle = $('#movieTitle p').text();

    var obj = { "fullName": fullname, "phoneNumber": phoneNumber };
    /*obj[fullName] = fullname;
    obj[phoneNumber] = phoneNumber;*/


    //console.log(JSON.stringify(data));

    // Send data for reservation
    $.ajax({
        url: '/api/makeReservation',
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
        }

    });

});




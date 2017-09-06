$(".theater-movie-is-related-to").on("click", function () {
    if ($(".reserve-status-msg").length > 0) {
        $(".reserve-status-msg").remove();
    }
    $("#choose-theater-msg").hide();
     
    // Resseting values
    choosenSeats = [];
    choosenSeatsIds = [];
    choosenLine = [];

    $.each(document.getElementsByClassName("theater-movie-is-related-to"), function (value) {
        $(this).removeClass("choosen");
    });
    $(this).addClass("choosen");

    var value = $(this).text();
    var number = value.replace("Theater: ", "");

    choosenTheater = number;
    showTimeId = $(this).next().next('.show-time-id').data("showtime-id");

    var theater = { "Number": number};
    var showTime = { "ShowTimeId": showTimeId };

    $.ajax({
        url: '/api/getTheater',
        type: "POST",
        data: JSON.stringify({ Theater: theater, ShowTime: showTime }),
        contentType: "application/json; charset=UTF-8",
        //dataType: "json",
        success: function (data) {
            console.log(data);
            $("#line-and-seats-wrapper").html("");

            // Lets control the theater screen size
            if (parseInt(number) <= 2) {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-big">Canvas</div>');
                SetupTheater("big", data);
            }
            else if (parseInt(number) <= 5) {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-medium">Canvas</div>');
                SetupTheater("medium", data);
            }
            else {
                $("#line-and-seats-wrapper").append('<div id="theater-screen-small">Canvas</div>');
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

            if (seat.reserved !== "reserved") {
                html += '<div class="theater-line-seat-general theater-line-seat-' + theaterSize + '" data-seat-Number="' + seat.number + '"></div>';
            } else {
                html += '<div class="theater-line-seat-reserved theater-line-seat-' + theaterSize + '" data-seat-Number="' + seat.number + '"></div>';
            }
            
            html += '<div data-seat-id="' + seat.seatId + '" hidden></div>';
        });
        html += '<div class="theater-line-number">' + line.number + '</div>';
        html += '</div>';
        html += '</div>';
        
        
    });
    html += '</div>';
    $("#line-and-seats-wrapper").append(html);


    // Controlling the seats choosen by the user.
    $(".theater-line-seat-general").on("click", function () {
        if ($(".reserve-status-msg").length > 0) {
            $(".reserve-status-msg").remove();
        }
        var alreadyChoosen = false;
        var firstSeatChoosen = choosenSeats[0];
        var lastSeatChoosen = choosenSeats[choosenSeats.length - 1];
        var seatNumber = $(this).data("seat-number");
        var lineNumber = $(this).nextAll('.theater-line-number').text();
        var choosenSeatId = $(this).next().data("seat-id");

        if (typeof choosenLine[0] === 'undefined' || choosenSeats.length === 0) {
            choosenLine[0] = lineNumber;
        }

        // Lets make sure we are on the same line number
        if ($(this).nextAll('.theater-line-number').text() == choosenLine[0]) {

            if ($.inArray(seatNumber, choosenSeats) !== -1) {
                alreadyChoosen = true;
                if (lineNumber === choosenLine[0] && firstSeatChoosen === seatNumber || lastSeatChoosen === seatNumber) {              
                    $(this).removeClass("seat-choosen");
                    var indexToRemove = choosenSeats.indexOf(seatNumber);
                    choosenSeatsIds.splice(indexToRemove, 1);
                    choosenSeats.splice(indexToRemove, 1);
                    choosenSeats.sort(function (a, b) {
                        return a - b;
                    });
                    console.log(choosenSeats);
                }
            }

        
            if (alreadyChoosen === false && lineNumber === choosenLine[0]) {
                if (choosenSeats.length === 0 || lineNumber === choosenLine[0] && seatNumber === lastSeatChoosen + 1 || seatNumber === lastSeatChoosen - 1 || seatNumber === firstSeatChoosen + 1 || seatNumber === firstSeatChoosen - 1) {
                    console.log(firstSeatChoosen);
                    choosenSeats.push(seatNumber);
                    choosenSeatsIds.push(choosenSeatId);
                    $(this).addClass("seat-choosen");
                    choosenSeats.sort(function (a, b) {
                        return a - b;
                    });
                    console.log(choosenSeats);
                }
            
            }

            // Lets control the price for the reservation based on seats choosen.
            var ticketPrice = 90;
            var totalPrice = 0;
            if (choosenSeats.length > 0) {
                $.each(choosenSeats, function (index, value) {
                    totalPrice = totalPrice + ticketPrice;
                    $('#total-reservation-price').html('Total price: ' + totalPrice);
                });
            } else {
                $('#total-reservation-price').html('Total price: ');
            }
        }
    });


}

$("#reserveTicketForm").submit(function (e) {
    e.preventDefault();

    var fullname = document.getElementById('fullNameInput').value;
    var phoneNumber = document.getElementById('phoneNumberInput').value;
    var movieTitle = $('#movieTitle p').text();

    var customer = { "FullName": fullname, "PhoneNumber": phoneNumber };
    var showTime = { "ShowTimeId": showTimeId };

    if (fullname !== "" && phoneNumber !== "" && choosenSeats.length > 0) {
        $.ajax({
            url: '/api/insertCustomer',
            type: "POST",
            async: false,
            data: JSON.stringify({ Customer: customer }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                console.log(data);
            }
        });

        $.each(choosenSeatsIds, function (index, value) {
            console.log(value);
            var seatId = { "SeatId": value };
    
            // Send data for reservation
            $.ajax({
                url: '/api/insertTicket',
                type: "POST",
                async: false,
                data: JSON.stringify({ Customer: customer, ShowTime: showTime, SeatId: seatId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    console.log("We are done");
                }
            });
        });
    }
    if (fullname !== "" && phoneNumber !== "" && choosenSeats.length > 0) {
        if ($(".reserve-status-msg").length > 0) {
            $(".reserve-status-msg").remove();
        }
        $('#moviepage-section-two').prepend('<div class="alert alert-success reserve-status-msg"><p>Thanks for using Disa Bio, Your reservation is now created!</p></div>');
        setTimeout(function () {
            location = '';
        }, 5000);
    }
    else {
        if ($(".reserve-status-msg").length > 0) {
            $(".reserve-status-msg").remove();
        }
        $('#moviepage-section-two').prepend('<div class="alert alert-danger reserve-status-msg"><p>Something went wrong, you need to specify (Name, Phonenumber and select the seats you want to reserve.)</p></div>');
    }
    
});




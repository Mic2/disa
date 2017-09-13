/*This js controls the whole movie page UI. */

// First we wait for the user to click on a theater to see the movie in.
$(".theater-movie-is-related-to").on("click", function () {

    // Now we reset the the price if the user has picket another theater than the original choice.
    $('#total-reservation-price').html('Total price: ');

    // Removing status message if the message is displayed.
    if ($(".reserve-status-msg").length > 0) {
        $(".reserve-status-msg").remove();
    }

    // Since the user now have made a choice about theater, we remove the text that notify the user to choose a theater.
    $("#choose-theater-msg").hide();
     
    // Resseting values
    choosenSeats = [];
    choosenSeatsIds = [];
    choosenLine = [];

    // Looping throug all of the theaters and removing higlight of that theater if a new one is choosen.
    $.each(document.getElementsByClassName("theater-movie-is-related-to"), function (value) {
        $(this).removeClass("choosen");
    });

    // Now setting the new value choosen by the user to be highligted
    $(this).addClass("choosen");

    // Collecting information that we need for the Web api controller to collect theater data.
    // The number of theater.
    var value = $(this).text();  
    var number = value.replace("Theater: ", "");

    // Storing data in more readble variables
    choosenTheater = number;
    showTimeId = $(this).next().next('.show-time-id').data("showtime-id");

    // Now wrapping data in Json format to be sent by Ajax
    var theater = { "Number": number};
    var showTime = { "ShowTimeId": showTimeId };

    // Sending data to Web api controller and displaying returned theater on the UI.
    $.ajax({
        url: '/api/getTheater',
        type: "POST",
        data: JSON.stringify({ Theater: theater, ShowTime: showTime }),
        contentType: "application/json; charset=UTF-8",
        success: function (data) {

            // Now we empty the theater wrapper, since the user might have choosen a new theater since the first choice.
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

// Handles the theater line and seats to be displayed to the user
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

    // Updating the view with the theater choosen.
    $("#line-and-seats-wrapper").append(html);


    // Controlling the seats choosen by the user.
    $(".theater-line-seat-general").on("click", function () {

        // Now if we have a status message we resset it.
        if ($(".reserve-status-msg").length > 0) {
            $(".reserve-status-msg").remove();
        }

        // Storing controller variables, wich will be used to validate when the user makes choices on a seat, if its clickable or not.
        var alreadyChoosen = false;
        var firstSeatChoosen = choosenSeats[0];
        var lastSeatChoosen = choosenSeats[choosenSeats.length - 1];
        var seatNumber = $(this).data("seat-number");
        var lineNumber = $(this).nextAll('.theater-line-number').text();
        var choosenSeatId = $(this).next().data("seat-id");

        // If no seat has been choosen the the choosen line number will be empty
        if (typeof choosenLine[0] === 'undefined' || choosenSeats.length === 0) {
            choosenLine[0] = lineNumber;
        }

        // Lets make sure we are on the same line number
        if ($(this).nextAll('.theater-line-number').text() === choosenLine[0]) {

            // If the user choose to remove a seat from the reservation we pull the data out of the choosenSeats array, and updating the view.
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
                }
            }

            // If the user choose to make reservation on one more seat we append the new seat to the choosenSeats array
            if (alreadyChoosen === false && lineNumber === choosenLine[0]) {
                if (choosenSeats.length === 0 || lineNumber === choosenLine[0] && seatNumber === lastSeatChoosen + 1 || seatNumber === lastSeatChoosen - 1 || seatNumber === firstSeatChoosen + 1 || seatNumber === firstSeatChoosen - 1) {
                    console.log(firstSeatChoosen);
                    choosenSeats.push(seatNumber);
                    choosenSeatsIds.push(choosenSeatId);
                    $(this).addClass("seat-choosen");
                    // Sorting the array, so we can make validation on the first and last picked user choice.
                    choosenSeats.sort(function (a, b) {
                        return a - b;
                    });
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

// When the user clicks on the make reservation button
$("#reserveTicketForm").submit(function (e) {
    // Prevending default action of the form
    e.preventDefault();

    // Collecting information to make the reservation
    var fullname = document.getElementById('fullNameInput').value;
    var phoneNumber = document.getElementById('phoneNumberInput').value;
    var movieTitle = $('#movieTitle p').text();

    // Converting the informations to Json format, so that our web api controller can use the data.
    var customer = { "FullName": fullname, "PhoneNumber": phoneNumber };
    var showTime = { "ShowTimeId": showTimeId };

    // Now we want to make sure we got enough information to make the reservation, if not we tell the user to fill the information before pressing the reservation button
    if (fullname !== "" && phoneNumber !== "" && choosenSeats.length > 0) {
        $.ajax({
            url: '/api/insertCustomer',
            type: "POST",
            async: false,
            data: JSON.stringify({ Customer: customer }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
            }
        });

        // Since every seat is treatend as 1 reservation in the system, we make call the web api controller for every seat choosen by the user.
        var result = 0;
        // Send data for reservation
        $.ajax({
            url: '/api/insertTicket',
            type: "POST",
            async: true,
            data: JSON.stringify({ Customer: customer, ShowTime: showTime, SeatIds: choosenSeatsIds }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data) {
                result = data;
                // Now if we have a message to the user that we need information, but the user now have filled the missing parts, we remove the message.
                if (fullname !== "" && phoneNumber !== "" && choosenSeats.length > 0 && result > 0) {
                    if ($(".reserve-status-msg").length > 0) {
                        $(".reserve-status-msg").remove();
                    }
                    // If all information is valid we tell the user that the reservation is complete.
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
            }
        });
    }
    
    
});




/*This JS script will update the frontpage movie selection based on the user choice of date */
$("#frontPageSelectBox").change(function () {

    // Collecting the date to sort view on, provided by the user.
    var date = $(this).find(':selected').data("real-date");

    // Calling web api AjaxRequestAPIController, to perform action on behalf of the view.
    $.ajax({
        url: '/api/getMoviesByDate',
        type: "POST",
        data: JSON.stringify(date),
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (data) {

            // On success return value, the view will be updated based on the returned values from the controller.
            $("#front-page-movie-outer-wrapper").html("");

            // Looping throug all of the movies return from the controller.
            $.each(data, function (MovieIndex, Movie) {
                var html = '<div class="col-md-3 movie-wrapper">';
                html += '<p>'+ Movie.name + '</p>';
                html += '<img src="'+ Movie.coverImage +'" class="cover-image"/>';

                html += '<form action="/Home/Movie" method="get">';
                html += '<input type="text" name="movieName" value="' + Movie.name + '" hidden />';
                html += '<input class="btn reserver-button" type="submit" value="Reserver" />';
                html += '</form>';

                html += '</div>';
                $("#front-page-movie-outer-wrapper").append(html);
            });
        }
    });


});
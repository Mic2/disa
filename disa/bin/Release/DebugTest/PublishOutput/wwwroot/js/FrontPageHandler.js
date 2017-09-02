$("#frontPageSelectBox").change(function () {

    var date = $(this).find(':selected').data("real-date");

    console.log(date);

    $.ajax({
        url: '/api/getMoviesByDate',
        type: "POST",
        data: JSON.stringify(date),
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (data) {


            $("#front-page-movie-outer-wrapper").html("");


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



            console.log(data);

        }
    });


});
$("#addShowTimeToForm").on("click", function (e) {
    e.preventDefault();

    // Appending to bottom of wrapper div everytime user presses the add button
    $("#multiShowTimeInputWrapper").append('<span class="input-remove-group"><div class="input"><input type="datetime" name="showtime" class="form-control" placeholder="YYYY-MM-DD HH:MI:SS" /><input id="showtimeTheaterInput" type="number" name="theater" class="form-control theaterNumberInput" placeholder="1-10"/><img src="/images/site/Delete-128.png" /></span>');

    // removing on user click red cross
    $("#multiShowTimeInputWrapper span img").on("click", function () {
        $(this).parent().remove();
    });
    


});
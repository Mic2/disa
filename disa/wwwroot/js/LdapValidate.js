$(document).ready(function () {
    console.log("ldap validation page is active");
    // Getting the values from user currently on the page
    var username = $('#username').text();
    console.log(username);

    // Splitting username to get Domain and username seperate
    var domainAndUsername = username.split("/");

    // Checking if the user is part of the domain
    if (domainAndUsername[0] === "disa") {
        console.log("part of the domain");

        // Asking php to get the group the user is part of
        $.ajax({
            url: "ldap.disa.com",
            data: domainAndUsername[1],
            method: "post",
            dataType: "json",
            success: function (data)
            {
                console.log("data");
            }
        });
    }
    else {
        console.log("not part of the domain");
    }
});
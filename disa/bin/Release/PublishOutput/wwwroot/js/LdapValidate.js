$(document).ready(function () {
    console.log("ldap validation page is active");
    // Getting the values from user currently on the page
    var username = $('#username').text();
    console.log(username);

    // Splitting username to get Domain and username seperate
    var domainAndUsername = username.split("/");

    // TEST DATA
    domainAndUsername[0] = "disa";
    domainAndUsername[1] = "JJ";

    // Checking if the user is part of the domain
    if (domainAndUsername[0] === "disa") {
        console.log("part of the domain");

        // Asking php to get the group the user is part of
        $.ajax({
            url: "https://ldap.disa.com",
            data: { userToCheck: domainAndUsername[1] },
            method: "post",
            type: "application/json",
            dataType: "jsonp",
            crossDomain: true,
            success: function (data) {
                console.log("dont work");
                console.log(data);
                console.log(JSON.parse(data));
            }
        });
    }
    else {
        console.log("not part of the domain");
    }
});
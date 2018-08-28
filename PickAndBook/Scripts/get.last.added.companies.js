$(function () {
    // Declare a proxy to reference the hub.
    var notifications = $.connection.lastAddedCompaniesHub;
       
    // Create a function that the hub can call to broadcast messages.
    notifications.client.displayCompanies = function () {
        getLastAddedCompanies()
    };

    // Start the connection.
    $.connection.hub.start();
    getLastAddedCompanies();
});


function getLastAddedCompanies()
{
    var tbl = $('#lastAddedCompanies');
    $.ajax({
        url: '/home/LastAddedCompanies',
        contentType: 'application/html ; charset:utf-8',
        type: 'GET',
        dataType: 'html'
    }).success(function (result) {
        tbl.empty().append(result);
    }).error(function () {
        alert('error')    
    });
}
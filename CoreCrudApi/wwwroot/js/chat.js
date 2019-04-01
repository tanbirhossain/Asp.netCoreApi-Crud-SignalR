"use strict";

// Create and start a connection.
// Add to the submit button a handler that sends messages to the hub.
// Add to the connection object a handler that receives messages from the hub
// Then add them to the list.
//alert("Chill");
loadUserList();
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start().catch(function (err) {
    return console.error(err.toString());
});

//connection.on -- that means this is wait for recive data
connection.on("ReceiveMessage", function (user, message) {

    //alert("recivve");
    loadUserList();
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);



});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    // connection.invoke-- that means its send
    connection.invoke("SendMessage", user, message).catch(function (err) { 
        return console.error(err.toString());
    });
    event.preventDefault();

});





function loadUserList() {

    var url = '/api/users/List/';
    
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            createTableTemplate(data);
        },
        error: function () {

        },
        complete: function () {
        }

    });

}


function createTableTemplate(data) {
    console.log(data);
    var html = '';
    $.each(data, function (key, item) {
        html += '<tr>';
        html += '<td>' + item.id + '</td>';
        html += '<td>' + item.name + '</td>';
        html += '<td>' + item.isActive + '</td>';
        html += '</tr>';
    });
    $('.tbodyX').html(html);
}
"use strict"

//const { signalR } = require("./signalr/dist/browser/signalr")

//const { signalR } = require("./signalr/dist/browser/signalr")

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$("#btnSend").attr("disabled", true);
/*
const Text = document.getElementById("text");
const User = document.getElementById("user");
const Time = document.getElementById("time");
*/
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp").replace(/</g, "&lt").replace(/>/g, "&gt");
    var user1 = $("#user").val();
    const timeElapsed = Date.now();
    const today = new Date(timeElapsed);
    let time = document.getElementById("time").innerHTML = today.toLocaleTimeString();
    //var encodeMessage = user + " " + msg;
    /*if ($("#user") == user) {
        $("#messageList").append("<li>" + right + "</li>");
    }*/

    if (user != user1) {
        $("#messageList").append( '<div class="chat-message-left pb-4 " >' +'<div>'+'<div class="text-muted small text-nowrap mt-2">' + time+  '</div>' +'</div>'+  '<div class=" flex-shrink-1 bg-light rounded py-2 px-3 ml-3 ">' + '<div>' + user + '</div>' + msg + '</div>');
    }
    if (user == user1) {
        $("#messageList").append( '<div class="chat-message-right pb-4 ">' + '<div>'+'<div class="text-muted small text-nowrap mt-2">' + time+  '</div>' +'</div>'+'<div class=" flex-shrink-1 bg-light rounded py-2 px-3 mr-3 " >' + '<div>' + user +'</div>'+ msg + '</div>');
     }  

});



connection.start().then(function () {
    $("#btnSend").attr("disabled", false);
}).catch(function (err) {
    return alert(err.toString());
});

$("#btnSend").on("click", function (event) {
    var user = $("#user").val();
    var message = $("#txtMessage").val();
    
    if (user != "" && message != "") {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return alert(err.toString())
        });

        event.preventDefault();
    }
    const inputs = document.querySelectorAll('#txtMessage');

    inputs.forEach(input => {
        input.value = '';
    });




});
// Client handlers 

var lobbyHub = null;

function appendMessage(name, message) {

    // Html encode display name and message.
    var encodedName = $('<div />').text(name).html();
    var encodedMsg = $('<div />').text(message).html();
    // Add the message to the page.
    $('#discussion').append('<li><strong>' + encodedName
        + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
}

function displayErrorMessage(message) {

    var encodedMsg = $('<div />').text(message).html();

    $('#errorList').append('<li><strong>Error: <strong/>' + encodedMsg);
}

function addPlayer(player) {
    var encodedName = $('<div />').text(player.Username).html();
    var encodedGamesPlayed = $('<div />').text(player.GamesPlayed).html();
    var encodedScore = $('<div />').text(player.Score).html();

    var nameElement = '<div class="col-sm-4">' + encodedName + '</div>';
    var gameNumberElement = '<div class="col-sm-4">' + encodedGamesPlayed + '</div>';
    var scoreElement = '<div class="col-sm-4">' + encodedScore + '</div>';

    var rowElement = nameElement + gameNumberElement + scoreElement;

    $("#players").append('<div id="' + encodedName + '" class="row">' + rowElement + '</div>');
}

function addExistingPlayers(players) {

    $(players).each(function (index, value) {

        addPlayer(value);
    });
}

function removePlayer(username) {
    var encodedName = $('<div />').text(username).html();

    $("#players").find('#' + encodedName).remove();
}

function removeRoom(groupId) {        

    $("#rooms").find('#' + 'room-' + groupId).remove();
}

function makeMove(username) {

    var encodedName = $('<div />').text(username).html();

    $('#discussion').append('<li><strong>' + encodedName
        + '</strong>&nbsp;&nbsp;' + 'made a move' + '</li>');
}

function addRoom(group) {

    var $roomElement = createRoomElement(group);

    $("#rooms").append($roomElement);
}

function addExistingRooms(groups) {

    $(groups).each(function (index, value) {

        addRoom(value);
    });

}

function addRoomMember(group) {
    
    var id = 'room-' + group.Id;

    var $room = $('#' + id);

    var $roomElement = createRoomElement(group);
    $("#rooms").find($room).remove();
    $("#rooms").append($roomElement);
}

function removeRoomMember(group) {
    
    var id = 'room-' + group.Id;

    var $room = $('#' + id);

    var $roomElement = createRoomElement(group);

    $("#rooms").find($room).remove();
    $("#rooms").append($roomElement);
}




//      **********  
//      Client events


function configureSendMessageEvent(lobby) {

    $('#sendmessage').click(function () {
        // Call the Send method on the hub.
        lobby.server.send($('#message').val());
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    });
}

function configureMakeMoveEvent(lobby) {
    $("#makemove").click(function () {
        lobby.server.makeMove();
        $('#message').val('').focus();
    });
}

function configureCreateRoomEvent(lobby) {

    $('#creategame').click(function () {

        lobby.server.createRoom();
    })
}

function joinRoom(groupId) {
    lobbyHub.server.joinRoom(groupId);
}

//      ************
//      GUI elements 


function createRoomElement(group) {

    var encodedName = $('<div />').text(group.Id).html();

    var id = "room-" + encodedName;

    var $roomElement = $('<div id="' + id + '" class="row">' + '</div>');
    
    for (i = 0; i < 3; i++) {        

        var $player = createPlayerSlot(group.Members[i], group.Id);

        $roomElement.append($player);                
    }

    return $roomElement;
}

function createPlayerSlot(player, groupId) {

    var openingTag = '<div class=col-sm-4 style="color:';
    var color = 'green">';
    var content = 'empty';

    if (player != null) {
        content = player;
        color = 'black">';
    }

    var closingTag = '</div>';

    var $player = $(openingTag + color + content + closingTag);

    $player.click(function () {
        joinRoom(groupId);
    })

    return $player;
}


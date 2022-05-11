(async function () {
    const chatBody = $(".chat-in-page__body");
    const messageInput = $(".chat-in-page__input__message-input");

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    hubConnection.on("DownloadMessagesFromUnauthorisedUser", function (messages) {
        $.post({
            url: "/TechnicalSupport/ParseMessagesFromUnauthorizedUser",
            data: messages,
            success: function (html) {
                updateChat(html);
            },
        });
    });

    hubConnection.on("DownloadMessagesFromDb", function (userId) {
        $.post({
            url: `/TechnicalSupport/UserMessagesForSupportPage?userId=${userId}`,
            success: function (html) {
                updateChat(html);
            },
        });
    });

    hubConnection.on("OnUserDisconnect", function () {
        $.post({
            url: "/TechnicalSupport/NotificationAboutUserDisconnect",
            success: function (messageHtml) {
                chatBody.append(messageHtml);
            },
        });
    });

    hubConnection.on("ReceiveNewMessage", function (message) {
        $.post({
            url: "/TechnicalSupport/MessageForSupportPage",
            data: message,
            success: function (messageHtml) {
                chatBody.append(messageHtml);
            },
        });
    });
    
    async function sendMessage() {
        let textMessage = messageInput.val();
        if (textMessage == null || textMessage.length === 0){
            return;
        }
        messageInput.val("");
        await hubConnection.invoke("SendMessageToUser", textMessage);
    }
    
    async function disconnect(){
        await hubConnection.invoke("DisconnectFromChatWithUser");
    }

    function updateChat(htmlWithMessages) {
        chatBody.empty();
        chatBody.append(htmlWithMessages);
    }
    
    $("#chat-in-page__send-message-btn").on("click", sendMessage);
    $("#disconnectBtnInSupportPage").on("click", disconnect);
    
    await hubConnection.start();

    await hubConnection.invoke("ConnectSupport");
})()
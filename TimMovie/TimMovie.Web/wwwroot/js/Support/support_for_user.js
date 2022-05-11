(async function () {
    const chatBody = $(".chat-container-body");
    const messageInput = $(".chat-container-answer-textarea");

    
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    hubConnection.on("GetMessages", function (supportConnectionId) {
        let messages = [];

        chatBody.children().each(function () {
            let el = $(this);
            let content, isSupport;
            let supportText = el.find(".chat-container-body-support-text");

            if (supportText.get().length !== 0) {
                content = supportText.text();
                isSupport = true;
            } else{
                let userText =el.find(".chat-container-body-user-text");
                content = userText.text();
                isSupport = false;
            } 

            messages.push({
                content,
                isSupport
            });
        })
        
        hubConnection.invoke("SendMessagesFromUnauthorisedUser", messages, supportConnectionId);
    });
    
    hubConnection.on("ReceiveNewMessage", function (message){
        $.post({
            url: "/TechnicalSupport/MessageForUserChat",
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
        await hubConnection.invoke("SendMessageToSupport", textMessage);
    }
    
    function showAllUserMessage(){
        $.get({
            url: "/TechnicalSupport/GetAllUserMessages",
            success: function (messagesHtml) {
                chatBody.append(messagesHtml);
            },
        });
    }


    $(".chat-container-answer-send").on("click", sendMessage);
    $(".support-chat-button").one("click", showAllUserMessage);

    await hubConnection.start();

    await hubConnection.invoke("ConnectUser");
})()
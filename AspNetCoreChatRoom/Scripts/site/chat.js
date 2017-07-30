app.chat = (function () {
    function renderMessage(message) {
        var template = $('#message-template').html();
        Mustache.parse(template);   // optional, speeds up future uses
        return Mustache.render(template, message);
    }

    return {
        init: function () {

            function focusLastMessage() {
                $("html, body").animate({ scrollTop: $(document).height() }, 1000);
            }

            // Reference the auto-generated proxy for the hub.
            var chat = $.connection.chatHub;
            // Create a function that the hub can call back to display messages.
            chat.client.addNewMessageToPage = function (message) {
                // Add the message to the page.
                $('.chat').append(renderMessage(message));
                focusLastMessage();
            };

            $('.input-sm').focus();
            $('.input-sm').keypress(function (event) {
                if (event.which === 13) {
                    event.preventDefault();
                    $('#btn-chat').click();
                }
            });

            $.connection.hub.start().done(function () {
                $('#btn-chat').click(function () {
                    // Call the Send method on the hub.
                    chat.server.send($('.input-sm').val());
                    // Clear text box and reset focus for next comment.
                    $('.input-sm').val('').focus();
                    focusLastMessage();
                });
            });

            $.connection.hub.error(function () {
                window.location.href = "/chat/NotAuthorized";
            });
        }
    };

})();

$(document).ready(function () {
    app.chat.init();
});
var app = app || {}

app.chat = (function (mustache) {

    function renderMessage(message) {
        var template = $('#message-template').html();
        mustache.parse(template);
        return mustache.render(template, message);
    }

    function focusLastMessage() {
        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    }

    return {
        $inputMsg: undefined,
        $btnSend: undefined,
        $chatArea: undefined,
        init: function () {
            var self = this;
            self.$inputMsg = $('.input-sm');
            self.$btnSend = $('#btn-chat');
            self.$chatArea = $('.chat');
            var protocol = location.protocol === "https:" ? "wss:" : "ws:";
            var port = document.location.port ? (":" + document.location.port) : "";
            var wsUri = protocol + "//" + window.location.hostname + port;
            var socket = new WebSocket(wsUri);
            socket.onopen = e => {
                console.log("socket opened", e);
            };

            socket.onclose = function (e) {
                console.log("socket closed", e);
            };

            socket.onmessage = function (e) {
                console.log(e);
                self.$chatArea.append(renderMessage(JSON.parse(e.data)));
                focusLastMessage();
            };

            socket.onerror = function (e) {
                console.error(e.data);
            };

            self.$btnSend.click(function () {
                var request = JSON.stringify({ "MessageType": "MessageRequestModel", "Data": JSON.stringify({ "Message": self.$inputMsg.val() }) });
                socket.send(request);
                self.$inputMsg.val('').focus();
                focusLastMessage();
            });

            this.$inputMsg.focus();
            this.$inputMsg.keypress(function (event) {
                if (event.which === 13) {
                    event.preventDefault();
                    self.$btnSend.click();
                }
            });
        }
    }
})(Mustache);
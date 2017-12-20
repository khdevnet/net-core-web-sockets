var app = app || {}

app.chat = (function (mustache, socketFactory) {

    function renderMessage(message) {
        var template = $('#message-template').html();
        mustache.parse(template);
        return mustache.render(template, message);
    }

    function focusLastMessage() {
        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    }

    function getWsUri() {
        var protocol = location.protocol === "https:" ? "wss:" : "ws:";
        var port = document.location.port ? (":" + document.location.port) : "";
        return protocol + "//" + window.location.hostname + port;
    }

    return {
        $inputMsg: undefined,
        $btnSend: undefined,
        $chatArea: undefined,
        $btnClose: undefined,
        init: function () {
            var self = this;
            self.$inputMsg = $('.input-sm');
            self.$btnSend = $('#btn-chat');
            self.$btnClose = $('#btn-close');
            self.$chatArea = $('.chat');

            var socket = socketFactory.create(getWsUri(), "SendMessageModel");

            socket.onMessage(function (data) {
                console.log(data);
                self.$chatArea.append(renderMessage(data));
                focusLastMessage();
            });

            self.$btnSend.click(function () {
                socket.send({ "Message": self.$inputMsg.val() });
                self.$inputMsg.val('').focus();
                focusLastMessage();
            });

            self.$btnClose.click(function () {
                socket.close();
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
})(Mustache, utils.soketFactory);
var utils = utils || {}

utils.soketFactory =
    (function () {
        return {
            create: function (wsUri, messageType) {
                var socket = new WebSocket(wsUri);
                return {
                    onOpen: function (callback) {
                        socket.onopen = e => {
                            console.log("socket opened", e);
                            callback(e);
                        };
                    },
                    onClose: function (callback) {
                        socket.onclose = function (e) {
                            console.log("socket closed", e);
                            callback(e);
                        };
                    },
                    onError: function (callback) {
                        socket.onerror = function (e) {
                            console.error(e.data);
                            callback(e.data);
                        };
                    },
                    onMessage: function (callback) {
                        socket.onmessage = function (e) {
                            console.log(e);
                            callback(JSON.parse(e.data));
                        };
                    },
                    send: function (data) {
                        var request = JSON.stringify({ "MessageType": messageType, "Data": JSON.stringify(data) });
                        socket.send(request);
                    }
                }
            }
        }
    })();
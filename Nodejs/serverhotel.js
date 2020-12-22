var express = require("express");
var request = require('request');
var getJSON = require('get-json');
var intersect = require('intersect');
var os = require('os');
var app = express();
app.use(express.static("public"));
app.set("view engine", "ejs");
app.set("views", "./views");
var server = require("http").Server(app);
var io = require("socket.io")(server);
server.listen(process.env.PORT || 2000);
// obj khởi tạo ban đầu
var o = 1800, b = 600;
var lstOrder = [];
var lstContactOrder = [];
var lstOnline = [];
var lstSchedule = [];
var checkConnect = false;
var datedefault = new Date(2016, 0, 1);
//var domain = 'http://123.31.30.27:81/';
var domain = 'http://localhost:13655/';
var key = 'Fdi@123';

ListSchedule();
ListOrderByAgency();
ListContactOrder();

function TotalSeconds(date) {
    return parseInt((date.getTime() - datedefault.getTime()) / 1000);
}

function ListOrderByAgency() {
    if (lstOrder.length == 0) {
        request(domain + 'Order/ListOrderByDateNow?key=' + key, function (error, response, body) {
            if (!error && response.statusCode == 200) {
                lstOrder = JSON.parse(body);
            }
        });
    }
    return lstOrder;
}

function ListContactOrder() {
    if (lstContactOrder.length == 0) {
        request(domain + 'ContactOrder/ListContactOrderByDateNow?key=' + key, function (error, response, body) {
            if (!error && response.statusCode == 200) {
                lstContactOrder = JSON.parse(body);
            }
        });
    }
    return lstContactOrder;
}

function ListSchedule() {
    if (lstSchedule.length == 0) {
        request(domain + 'DNSchedule/GetAll?key=' + key, function (error, response, body) {
            if (!error && response.statusCode == 200) {
                lstSchedule = JSON.parse(body);
            }
        });
    }
    return lstSchedule;
}

app.get("/", function (req, res) {
    res.render("Index");
});

app.get("/checkinout/:id/:check/:key", function (req, res) {
    if (req.params.key == key) {
        lstOnline.push(req.params.id, req.params.check);
    }
    res.render("Out");
});

app.get("/addorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        lstOrder.push(JSON.parse(req.params.id));
    }
    res.render("Out");
});

app.get("/addcontactorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        lstContactOrder.push(JSON.parse(req.params.id));
    }
    res.render("Out");
});

app.get("/updateorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].ID == req.params.id) {
                lstOrder[i].EndDate = dns + 2;
                break;
            }
        }
    }
    res.render("Out");
});

app.get("/updatecontact/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (lstContactOrder[i].ID == req.params.id) {
                lstContactOrder[i].EndDate = dns + 1;
                break;
            }
        }
    }
    res.render("Out");
});

function resetInterval(idInterval) {
    clearInterval(idInterval);
}
// kết nối

io.on("connection", function (socket) {
    console.log("ID ket noi server: " + socket.id);
    //LoadMain
    socket.on('client-load-main', function () {
        if (!checkConnect) {
            setInterval(function () {
                var dn = new Date();
                var dns = TotalSeconds(dn);
                var h = dn.getHours();
                var m = dn.getMinutes();
                var ss = dn.getSeconds();
                if (h == 0 && m == 0 && lstContactOrder.length == 0 && ss == 0) {
                    ListSchedule();
                }

                // Checkinout
                if (lstOnline.length > 0) {
                    io.sockets.emit('server-load-main', { ds: lstOnline, aid: 0, bid: null });
                    lstOnline = [];
                }
                var i = lstSchedule.length;
                while (i--) {
                    if (parseInt(lstSchedule[i].HoursStart) == h && parseInt(lstSchedule[i].MinuteStart) == m && ss == 0) {
                        io.sockets.emit('server-load-main', { ds: lstOnline, aid: lstSchedule[i].AgencyID, bid: null });
                        lstSchedule.splice(i, 1);
                    }
                }
                // Active Order
                i = lstOrder.length;
                while (i--) {
                    if (parseInt(lstOrder[i].StartDate) - o <= dns && lstOrder[i].Status == 0) {
                        lstOrder[i].Status = 1;
                        io.sockets.emit('server-load-main', { ds: lstOnline, aid: "red", bid: lstOrder[i] });
                    }
                    if (parseInt(lstOrder[i].EndDate) == dns) {
                        io.sockets.emit('server-load-main', { ds: lstOnline, aid: "blue", bid: lstOrder[i] });
                        lstOrder.splice(i, 1);
                    }
                }
                // Active ContactOrder
                i = lstContactOrder.length;
                while (i--) {
                    if (parseInt(lstContactOrder[i].StartDate) - o <= dns && lstContactOrder[i].Status == 0) {
                        lstContactOrder[i].Status = 1;
                        io.sockets.emit('server-load-main', { ds: lstOnline, aid: "gold", bid: lstContactOrder[i] });
                    }
                    if (parseInt(lstContactOrder[i].EndDate) == dns) {
                        io.sockets.emit('server-load-main', { ds: lstOnline, aid: "blue", bid: lstContactOrder[i] });
                        lstContactOrder.splice(i, 1);
                    }
                }

            }, 1000);
        }
        checkConnect = true;
    });
});

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
server.listen(process.env.PORT || 5000);
// obj khởi tạo ban đầu
var o = 1800, b = 600;
var lstOrder = [];
var lstContactOrder = [];
var lstOnline = [];
var lstSchedule = [];
var lstnotify = [];
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
    request(domain + 'Order/ListOrderByDateNow?key=' + key, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstOrder = JSON.parse(body);
        }
    });
    return lstOrder;
}
app.get("/checkinout/:id/:check/:key", function (req, res) {
    if (req.params.key == key) {
        lstOnline.push(req.params.id, req.params.check);
    }
    res.render("Out");
});
function ListContactOrder() {
    request(domain + 'ContactOrder/ListContactOrderByDateNow?key=' + key, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstContactOrder = JSON.parse(body);
        }
    });
    return lstContactOrder;
}

function ListSchedule() {
    request(domain + 'DNSchedule/GetAll?key=' + key, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstSchedule = JSON.parse(body);
        }
    });
    return lstSchedule;
}

app.get("/", function (req, res) {
    res.render("Index");
});

app.get("/addorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        lstOrder.push(JSON.parse(req.params.id));
        lstOrder.reverse(function (a, b) {
            return a.StartDate - b.StartDate;
        });
    }
    res.render("Out");
});

app.get("/addnotify/:id/:key", function (req, res) {
    if (req.params.key == key) {
        lstnotify.push(JSON.parse(req.params.id));
        //lstnotify.reverse(function (a, b) {
        //    //return a.StartDate - b.StartDate;
        //});
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
        //var dn = new Date();
        //var dns = TotalSeconds(dn);
        for (var i = 0; i < lstOrder.length; i++) {
            if (parseInt(lstOrder[i].ID) == parseInt(req.params.id)) {
                //lstOrder[i].EndDate = dns;
                lstOrder[i].Status = 3;
                break;
            }
        }
    }
    res.render("Out");
});

app.get("/updatecontact/:id/:key", function (req, res) {
    if (req.params.key == key) {
        //var dn = new Date();
        //var dns = TotalSeconds(dn);
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (parseInt(lstContactOrder[i].ID) == parseInt(req.params.id)) {
                //lstContactOrder[i].EndDate = dns;
                lstContactOrder[i].Status = 3;
                break;
            }
        }
    }
    res.render("Out");
});

// kết nối
io.on("connection", function (socket) {
    console.log("ID ket noi server: " + socket.id);
    socket.on('client-listorder-main', function (data) {
        var list = [];
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].AgencyId == data.id) {
                list.push(lstOrder[i]);
            }
        }
        io.sockets.emit('server-listorder-main', { list: list, aid: data.id });
    });
    socket.on('client-listcontact-main', function (data) {
        var list = [];
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (lstContactOrder[i].AgencyId == data.id && lstContactOrder[i].EndDate > dns) {
                list.push(lstContactOrder[i]);
            }
        }
        io.sockets.emit('server-listcontact-main', { list: list, aid: data.id });
    });
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
                    io.sockets.emit('server-schedulein-main', { ds: lstOnline, aid: 0, bid: null });
                    lstOnline = [];
                }
                // Active Schedule
                var i = lstSchedule.length;
                while (i--) {
                    if (parseInt(lstSchedule[i].HoursStart) == h && parseInt(lstSchedule[i].MinuteStart) == m && ss == 0) {
                        io.sockets.emit('server-schedulein-main', { ds: lstOnline, aid: lstSchedule[i].AgencyID, bid: null });
                        lstSchedule.splice(i, 1);
                    }
                }
                // Active Order
                var cco = lstOrder.length;
                i = cco;
                while (i--) {
                    if (lstOrder[i].Status == 0) {
                        lstOrder[i].Status = 1;
                        io.sockets.emit('server-count-main', { aid: "red", bedid: lstOrder[i] });
                    }
                    if (parseInt(lstOrder[i].Status) == 3 || (parseInt(lstOrder[i].EndDate) <= dns && parseInt(lstOrder[i].Status) < 2)) {
                        io.sockets.emit('server-ready-main', { ds: "red", bid: lstOrder[i] });
                        lstOrder.splice(i, 1);
                    }
                }
                // Active ContactOrder
                cco = lstContactOrder.length;
                i = cco;
                while (i--) {
                    if (lstContactOrder[i].Status == 0) {
                        lstContactOrder[i].Status = 1;
                        io.sockets.emit('server-count-main', { aid: "gold", bedid: lstContactOrder[i] });
                    }
                    if (parseInt(lstContactOrder[i].EndDate) <= dns && parseInt(lstContactOrder[i].Status) < 2) {
                        io.sockets.emit('server-ready-main', { ds: "gold", bid: lstContactOrder[i] });
                        lstContactOrder.splice(i, 1);
                    }
                    else if (parseInt(lstContactOrder[i].Status) == 3) {
                        io.sockets.emit('server-ready-main', { ds: "blue", bid: lstContactOrder[i] });
                        lstContactOrder.splice(i, 1);
                    }
                }

            }, 1000);
        }
        checkConnect = true;
    });

    socket.on('client-exchange-main', function (data) {
        io.sockets.emit('server-exchange-main', { bedid: data.bedid, bedeid: data.bedeid });
    });
    // offline
    socket.on('disconnect', function () {
        socket.disconnect();
    });
});
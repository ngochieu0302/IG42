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
server.listen(process.env.PORT || 4000);
// obj khởi tạo ban đầu
var o = 5400, b = 600;
var lstOrder = [];
var lstContactOrder = [];
var lstuser = [];
var lstnotify = [];
var checkConnect = false;
var datedefault = new Date(2016, 0, 1);
//var domain = 'http://123.31.30.27:81/';
var domain = 'http://localhost:13655/';
var key = 'Fdi@123';

ListOrderByAgency();
ListContactOrder();
//ListWarehouse();
function TotalSeconds(date) {
    return parseInt((date.getTime() - datedefault.getTime()) / 1000);
}
function ListOrderByAgency() {
    request(domain + 'Order/ListRestaurantByDateNow?key=' + key, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstOrder = JSON.parse(body);
        }
    });
    return lstOrder;
}
//function ListWarehouse() {
//    debugger;
//    request(domain + 'StorageFreightWarehouse/ListWarehouseByNotActive?key=' + key, function (error, response, body) {
//        if (!error && response.statusCode == 200) {
//            lstnotify = JSON.parse(body);
//        }
//    });
//    return lstnotify;
//}
function ListContactOrder() {
    request(domain + 'ContactOrder/ListRestaurantByDateNow?key=' + key, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstContactOrder = JSON.parse(body);
        }
    });
    return lstContactOrder;
}
//function ListNotify() {
//    request(domain + 'ContactOrder/ListRestaurantByDateNow?key=' + key, function (error, response, body) {
//        if (!error && response.statusCode == 200) {
//            lstContactOrder = JSON.parse(body);
//        }
//    });
//    return lstContactOrder;
//}
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
    }
    res.render("Out");
});
app.get("/updateorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].ID == req.params.id) {
                //lstOrder[i].Status = 4;
                lstOrder[i].EndDate = dns;
            }
        }
    }
    res.render("Out");
});
app.get("/updateorderbed/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].BedDeskID == req.params.id) {
                lstOrder[i].EndDate = dns;
                break;
            }
        }
    }
    res.render("Out");
});
app.get("/statuseorder/:id/:status/:key", function (req, res) {
    if (req.params.key == key) {
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].ID == req.params.id) {
                lstOrder[i].Status = req.params.status;
            }
        }
    }
    res.render("Out");
});
app.get("/addcontactorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        lstContactOrder.push(JSON.parse(req.params.id));
        lstContactOrder.reverse(function (a, b) {
            return a.StartDate - b.StartDate;
        });
    }
    res.render("Out");
});
app.get("/updatecontact/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (lstContactOrder[i].ID == req.params.id) {
                lstContactOrder[i].EndDate = dns;
            }
        }
    }
    res.render("Out");
});
app.get("/updatecontactbed/:id/:key/:bid", function (req, res) {
    if (req.params.key == key) {
        var dn = new Date();
        var dns = TotalSeconds(dn);
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (lstContactOrder[i].BedDeskID == req.params.bid && lstContactOrder[i].ID == req.params.id) {                
                lstContactOrder[i].EndDate = dns;
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
        for (var i = 0; i < lstContactOrder.length; i++) {
            if (lstContactOrder[i].AgencyId == data.id) {
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
                // Active Order
                var cco = lstOrder.length;
                var i = cco;
                while (i--) {
                    if (lstOrder[i].Status == 0) {
                        lstOrder[i].Status = 3;
                        io.sockets.emit('server-count-main', { aid: "serving-desk", bedid: lstOrder[i] });
                    }
                    if (lstOrder[i].Status == 1) {
                        lstOrder[i].Status = 2;
                        io.sockets.emit('server-status-main', { aid: "serving-desk", bedid: lstOrder[i] });
                    }
                    if (parseInt(lstOrder[i].EndDate) <= dns) {
                        io.sockets.emit('server-ready-main', { ds: "serving-desk", bid: lstOrder[i] });
                        lstOrder.splice(i, 1);
                    }
                }
                // Active ContactOrder
                cco = lstContactOrder.length;
                i = cco;
                while (i--) {
                    if (lstContactOrder[i].Status == 0) {
                        lstContactOrder[i].Status = 1;
                        io.sockets.emit('server-count-main', { aid: "booking-desk", bedid: lstContactOrder[i] });
                    }
                    if (parseInt(lstContactOrder[i].EndDate) <= dns) {
                        io.sockets.emit('server-ready-main', { ds: "booking-desk", bid: lstContactOrder[i] });
                        lstContactOrder.splice(i, 1);
                    }
                }
            }, 1000);
        }
        checkConnect = true;
    });
    //notify
    socket.on('client-approved-main', function (data) {
        console.log("push- : " + data);
        io.sockets.emit('server-approved-main', { data: data, uid: obj });
    });
    socket.on('client-userId-online', function (user, aid, codelogin) {
        var today = new Date();
        var dd = today.getDate();
        var i = lstuser.length;
        var check = false;
        while (i--) {
            if (lstuser[i].uid === user) {
                lstuser[i].o = true;
                lstuser[i].lid.push(codelogin);
                if (lstuser[i].d !== dd) {
                    lstuser[i].d = dd;
                    lstuser[i].s = TotalSeconds(today);
                    lstuser[i].q = 0;
                }
                check = true;
                break;
            }
        }
        if (!check) {
            var item = { id: codelogin, uid: user, q: 0, d: dd, o: true, a: parseInt(aid), s: TotalSeconds(today), lid: [] };
            item.lid.push(codelogin);
            lstuser.push(item);
        }
    });
    setInterval(function () {
        var i = lstnotify.length;
        if (i > 0) {
            while (i--) {
                lstuser.sort(function (a, b) {
                    return a.s - b.s;
                });
                var today = new Date();
                var j = lstuser.length;
                var order = JSON.stringify(lstnotify[i]);
                while (j--) {
                    if (lstuser[j].o) {
                        lstuser[j].q++;
                        lstuser[j].s = TotalSeconds(today);
                        io.sockets.emit('server-warehouse-online', { obj: order, uid: lstuser[j].uid });
                        io.sockets.emit('server-approved-main', { count: i, uid: lstuser[j].uid });
                    }
                }
            }
        }
        lstnotify = [];
    }, 1000);
});
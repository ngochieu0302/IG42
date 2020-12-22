var express = require("express");
var request = require('request');
//var getJSON = require('get-json');
//var intersect = require('intersect');
//var os = require('os');
var app = express();
//app.use(express.static("public"));
app.set("view engine", "ejs");
app.set("views", "./views");
var server = require("http").Server(app);
var io = require("socket.io")(server);
var port = process.env.PORT || 3000;
server.listen(port);
console.log("localhost:"+port);
// obj khởi tạo ban đầu
var o = 1800, b = 600;
var lstOrder = [];
var checkConnect = false;
var datedefault = new Date(2016, 0, 1);
//var domain = 'http://124.158.4.87:94/';
var domain = 'http://localhost:13655/';
var key = 'Fdi@123';
ListOrderByAgency();

var count=0;

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

app.get("/updateorder/:id/:key", function (req, res) {
    if (req.params.key == key) {
        for (var i = 0; i < lstOrder.length; i++) {
            if (parseInt(lstOrder[i].ID) == parseInt(req.params.id)) {
                lstOrder[i].Status = 3;
                break;
            }
        }
    }
    res.render("Out");
});
app.get("/updateorderAdd/:id/:key", function (req, res) {
    if (req.params.key == key) {
        var obj = JSON.parse(req.params.id);
        for (var i = 0; i < lstOrder.length; i++) {
            if (parseInt(lstOrder[i].ID) == parseInt(obj.ID)) {
                lstOrder[i].EndDate = obj.EndDate;
                lstOrder[i].StartDate = obj.StartDate;
                lstOrder[i].Status = -1;
                break;
            }
        }
    }
    res.render("Out");
});

// kết nối
io.on("connection", function (socket) {
    console.log("ID ket noi server: " + socket.id);
    count++;
    console.log("Total connect: " + count);
    
    socket.on('client-listorder-main', function (data) {
        var list = [];
        for (var i = 0; i < lstOrder.length; i++) {
            if (lstOrder[i].AgencyId == data.id) {
                list.push(lstOrder[i]);
            }
        }      
        io.sockets.emit('server-listorder-main', { list: list, aid: data.id });
    });
    socket.on('pingScalesCate', function (data) {
        console.log("weight cate: ",data);  
        io.sockets.emit('pingScalesCate', data);    
    });
    
    socket.on('calesvalue', function (data) {
        console.log("server vua nhan dc ",data);
        io.sockets.emit('calesvalue', data);
    });
    socket.on('calesDetailvalue', function (data) {
        console.log("server vua nhan dc ",data);
        io.sockets.emit('calesDetailvalue', data);
    });
    //LoadMain
    socket.on('client-load-main', function () {
        if (!checkConnect) {
            setInterval(function () {
                var dn = new Date();
                var dns = TotalSeconds(dn);
                // Active Order
                var i = lstOrder.length;
                while (i--) {
                    if (lstOrder[i].Status == 0) {
                        lstOrder[i].Status = 1;
                        io.sockets.emit('server-count-main', { aid: "red", bedid: lstOrder[i] });
                    }
                    if (lstOrder[i].Status == -1) {
                        lstOrder[i].Status = 2;
                        io.sockets.emit('server-add-main', { aid: "red", bedid: lstOrder[i] });
                    }
                    if (parseInt(lstOrder[i].Status) == 3 || (parseInt(lstOrder[i].EndDate) <= dns && parseInt(lstOrder[i].Status) < 2)) {
                        io.sockets.emit('server-ready-main', { ds: "red", bid: lstOrder[i] });
                        lstOrder.splice(i, 1);
                    }
                }
            }, 1000);
        }
        checkConnect = true;
    });
    // offline
    socket.on('disconnect', function () {
        console.log("ID disconnect server: " + socket.id);
        count--;
        socket.disconnect();
        console.log("connects: " + count);
    });
});

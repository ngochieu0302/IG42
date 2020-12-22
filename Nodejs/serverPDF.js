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
var o = 5400, b = 600;

var checkConnect = false;
var datedefault = new Date(2016, 0, 1);
//var domain = 'http://123.31.30.27:81/';
var domain = 'http://localhost:13655/';
var key = 'Fdi@123';
var Num = 1;

//function ListWarehouse() {
//    debugger;
//    request(domain + 'StorageFreightWarehouse/ListWarehouseByNotActive?key=' + key, function (error, response, body) {
//        if (!error && response.statusCode == 200) {
//            lstnotify = JSON.parse(body);
//        }
//    });
//    return lstnotify;
//}

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
// kết nối
io.on("connection", function (socket) {
    console.log("ID ket noi server: " + socket.id);
    //LoadMain
    socket.on('client-load-main', function (data) {
        Num = data.pagenum;
        io.sockets.emit("server-load-main", { pagenum: Num });
    });
    socket.on('client-click-prev', function (data) {
        Num = data.pagenum;
        if (Num <= 1) {
            return;
        }
        Num--;
        io.sockets.emit("server-click-prev", { pagenum: Num });
    });
    socket.on('client-click-next', function (data) {
        Num = data.pagenum;
        if (Num >= data.totalpage) {
            return;
        }
        Num++;
        io.sockets.emit("server-click-next", { pagenum: Num });
    });
});

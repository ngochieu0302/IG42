//var http = require('http');
//var request = require('request');
//var server = http.createServer(function (request, response) {
//    response.writeHead(200, { "Content-Type": "text/plain" });
//    response.end("Hello World\n");
//});
//server.listen(8000); 
var express = require("express");
var request = require('request');
var app = express();
app.use(express.static("public"));
app.set("view engine", "ejs");
app.set("views", "./views");
var server = require("http").Server(app);
server.listen(process.env.PORT || 4000);
// obj khởi tạo ban đầu
var o = 5400, b = 600;
var lstOrder = [];
var datedefault = new Date(2019, 0, 1);
var domain = 'http://localhost:13657/';
var key = 'Fdi@123';
// var domain = 'http://gstoreapi.fditech.vn/';

function TotalSeconds(date) {
    return parseInt((date.getTime() - datedefault.getTime()) / 1000);
}
app.get("/addorder/:json/:key", function (req, res) {
    console.log("vào add");
    if (req.params.key === key) {
        lstOrder.push(JSON.parse(req.params.json));
    }
    res.render("Out");
});

app.get("/SpliceOrder/:id/:key", function (req, res) {
    if (req.params.key === key) {
        var cco = lstOrder.length;
        i = cco;
        while (i--) {
            if (lstOrder[i].OrderId === req.params.id) {
                lstOrder.splice(i, 1);
            }
        }
    }
    res.render("Out");
});
setInterval(() => {
    ProcessOrderShip();
}, 5000);
function ProcessOrderShip() {
    var dn = new Date();
    var dns = TotalSeconds(dn);
    var cco = lstOrder.length;
    i = cco;
    while (i--) {

        console.log(lstOrder[i].EndDate);
        if (lstOrder[i].EndDate <= dns) {
            request(domain + 'OrderApp/ProcessOrderShip?key=' + key + "&json=" + JSON.stringify(lstOrder[i]), function (error, response, body) {
                var json = JSON.parse(body);
                if (!error && json.Code === 200) {
                    console.log("success");
                    lstOrder.splice(i, 1);
                }
            });
        }
    }
}
var express = require("express");
var request = require('request');
var getJSON = require('get-json');
var intersect = require('intersect');
var os = require('os');
var app =  express();
app.use(express.static("public"));
app.set("view engine", "ejs");
app.set("views", "./views");
var server = require("http").Server(app);
var io = require("socket.io")(server);
server.listen(process.env.PORT || 1000);
var lstInbox;
var lstSpam;
var o = 1800, b = 600;
var lstSpam;
var refreshIntervalId;
var lstOrder = [];
var lstContactOrder = [];
var lstOnline = [];
var lstSchedule = [];
var order = {};
var idCheckIn = "";
var idCheckOut = "";
var flag = 0;
var checkConnect = false;
var datedefault = new Date(2016, 0, 1);
var domain = 'http://123.31.30.27:81/';
var key = 'Fdi@123';

//setInterval(function () {
//    console.log('Het 10 giay bi timeout !');
//}, 10000);
//SendMailAuto();

function SendMailAuto(){
  request(domain+'DNMailSSC/GetListBirthDay?key='+key, function (error, response, body) {
      if (!error && response.statusCode == 200) {
        console.log(body);
      }
    })
}

function TotalSeconds(date) {
  return parseInt((date.getTime() - datedefault.getTime()) / 1000);
}

app.get("/", function(req, res){
  res.render("Index");
});


io.on("connection", function(socket){
  console.log("ID ket noi server: " + socket.id);
// khi gửi mail
  socket.on("client-send-mail", function(data){
    request(domain+`DNMailSSC/CountInboxNew?key=` + key + `&type=1&userId=`+ data.userId, function (error, response, body) {
      if (!error && response.statusCode == 200) {
        console.log(body);
        console.log(data.userId);
        io.sockets.emit("server-send-mail", body);
      }
    })
  });

  // luu thu nhap
  socket.on("client-save-drafts", function(data){
    request(domain+`DNMailSSC/CountDrafts?key=` + key + `&type=3&userId=`+ data.userId, function (error, response, body) {
      if (!error && response.statusCode == 200) {
        io.sockets.emit("server-save-drafts", body);
      }
    })
  });

  // chuyển thư nháp đến hộp thư đến
  socket.on("client-send-move-mailbox", function(data){
    console.log(data.type + " " + data.userId);
    request(domain+`DNMailSSC/CountDrafts?key=` + key + `&userId=`+ data.userId, function (error, response, body) {
      if (!error && response.statusCode == 200) {
        io.sockets.emit("server-send-move-mailbox", body);
      }
    })
  });

  // tự động lưu hộp thư nháp
  socket.on("client-send-auto-draft", function(data){
    console.log(data);
    request(domain+`DNMailSSC/CountDrafts?key=` + key + `&userId=`+ data.userId, function (error, response, body) {
      if (!error && response.statusCode == 200) {
        io.sockets.emit("server-send-auto-draft", body);
      }
    })
  });

  // báo cáo spam
  socket.on("client-send-spam", function(data){
    // xử lý hộp thư đến
    if(data.type == "1")
    {
      request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=`+data.type+`&userId=`+ data.userId, function (error1, response, body1) {
        console.log(body1);
        if (!error1 && response.statusCode == 200) {
            request(domain+`DNMailSSC/CountInbox?key=` + key + `&type=1&userId=`+ data.userId, function (error2, response, body2) {
                //io.sockets.emit("server-save-spam", body1);
                if (!error2 && response.statusCode == 200) {
                  lstSpam = body1;
                  lstInbox = body2;
                  io.sockets.emit("server-save-spam", { ListSpam: lstSpam, ListInbox: lstInbox });
                }
            });
        }
      });
    }
    else if(data.type == "5")
    {
      request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=4&userId=`+ data.userId, function (error1, response, body1) {
        if (!error1 && response.statusCode == 200) {
            request(domain+`DNMailSSC/GetListSimpleByRequest?key=` + key + `&type=5&userId=`+ data.userId, function (error2, response, body2) {
                //io.sockets.emit("server-save-spam", body1);
                if (!error2 && response.statusCode == 200) {
                  lstSpam = body1;
                  lstInbox = body2;
                  io.sockets.emit("server-save-spam", { ListSpam: lstSpam, ListInbox: lstInbox });
                }
            });
        }
      });
    }
    // xử lý hộp thư đã gửi
    else
    {
      request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=4&userId=`+ data.userId, function (error1, response, body1) {
        if (!error1 && response.statusCode == 200) {
            request(domain+`DNMailSSC/SentMail?key=` + key + `&userId=`+ data.userId, function (error2, response, body2) {
                if (!error2 && response.statusCode == 200) {
                  lstSpam = body1;
                  lstInbox = body2;
                  io.sockets.emit("server-save-spam", { ListSpam: lstSpam, ListInbox: lstInbox });
                }
            });
        }
      });
    }
  });

// không phải spam
  socket.on("client-send-no-spam", function(data){
    request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=4&userId=`+ data.userId, function (error, response, body) {
      if (!error && response.statusCode == 200) {
          lstSpam = body;
          io.sockets.emit("server-save-no-spam", lstSpam);
      }
    });
  });

  // Cho vào thùng rác
    socket.on("client-send-recyclebin", function(data){
      if(data.type == "1")
      {
        request(domain+`DNMailSSC/CountInboxNew?key=` + key + `&type=1&userId=`+ data.userId, function (error, response, body) {
            if (!error && response.statusCode == 200) {
              io.sockets.emit("server-save-recyclebin", { lstEmail : body, type: data.type });
            }
        });
      }
      else if(data.type == "3"){
        request(domain+`DNMailSSC/CountDrafts?key=` + key + `&type=3&userId=`+ data.userId, function (error, response, body) {
            if (!error && response.statusCode == 200) {
              console.log(data.type);
              io.sockets.emit("server-save-recyclebin", { lstEmail : body, type: data.type });
            }
        });
      }
      else if(data.type == "4"){
        request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=4&userId=`+ data.userId, function (error, response, body) {
            if (!error && response.statusCode == 200) {
              io.sockets.emit("server-save-recyclebin", { lstEmail : body, type: data.type });
            }
        });
      }
      else if(data.type == "5"){
        request(domain+`DNMailSSC/CountSpam?key=` + key + `&type=4&userId=`+ data.userId, function (error, response, body) {
            //if (!error && response.statusCode == 200) {
            //  console.log(data.type);
            //  io.sockets.emit("server-save-recyclebin", { lstEmail : body, type: data.type });
            //}
        });
      }
      else {
        request(domain+`DNMailSSC/SentMail?key=` + key + `&userId=`+ data.userId, function (error, response, body) {
          if (!error && response.statusCode == 200) {
              io.sockets.emit("server-save-recyclebin", { lstEmail : body, type: data.type });
          }
        });
      }
    });

  // xóa vĩnh viễn
    socket.on("client-send-delete", function(data){
      request(domain+`DNMailSSC/GetListDelete?key=` + key + `&userId=`+ data.userId, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            lstSpam = body;
            io.sockets.emit("server-save-delete", lstSpam);
        }
      })
    });
});
